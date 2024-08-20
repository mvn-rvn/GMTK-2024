using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;

public class StageScript : MonoBehaviour
{
    Parsing parser;
    DialogueEngine engine;
    EngineNode node;
    DialogueType dialogue_type;

    public TextAsset guilt_dialogue;
    public TextAsset ambition_dialogue;

    public string[] hand_transitions;
    public string[] tutorials;

    public Animator hand_anim_controller;

    GameObject body_button;
    GameObject body_text;
    GameObject head_button;
    GameObject head_text;
    GameObject arrow;

    bool waiting_for_click = false;
    public bool waiting_for_level_completion = false;
    public bool final_level = false;


    // Start is called before the first frame update
    void Start()
    {
        parser = gameObject.GetComponent<Parsing>();
        body_button = GameObject.Find("BodyButton");
        body_text = GameObject.Find("BodyText");
        head_button = GameObject.Find("HeadButton");
        head_text = GameObject.Find("HeadText");
        arrow = GameObject.Find("Arrow");
        head_button.SetActive(false);
        body_button.SetActive(false);
        arrow.SetActive(false);
        StartCoroutine(EnterGuilt());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clicked() {
        waiting_for_click = false;
    }

    IEnumerator EnterGuilt() {
        while(!GameObject.Find("Hand AnimController").GetComponent<Animator>().enabled) {
            yield return null;
        }
        while(GameObject.Find("Hand AnimController").GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name != "RestLoop") {
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        float interpolate = 0f;
        while(interpolate != 90f) {
            interpolate += 1;
            GameObject.Find("Guilt AnimController").transform.position = new Vector3(Mathf.Lerp(-8.88f, -4.44f, Mathf.Sin(interpolate * Mathf.Deg2Rad)), 0f, 4f);
            GameObject.Find("Guilt AnimController").GetComponent<SpriteRenderer>().color = new Color(
                Mathf.Lerp(0, 1, interpolate / 90),
                Mathf.Lerp(0, 1, interpolate / 90),
                Mathf.Lerp(0, 1, interpolate / 90),
                1f
            );
            yield return new WaitForSeconds(1f / 60f);
        }

        yield return StartCoroutine(RunDialogue(guilt_dialogue));
        Debug.Log("Guilt done");

        while(interpolate != 0f) {
            interpolate += -1;
            GameObject.Find("Guilt AnimController").transform.position = new Vector3(Mathf.Lerp(-8.88f, -4.44f, Mathf.Sin(interpolate * Mathf.Deg2Rad)), 0f, 4f);
            GameObject.Find("Guilt AnimController").GetComponent<SpriteRenderer>().color = new Color(
                Mathf.Lerp(0, 1, interpolate / 90),
                Mathf.Lerp(0, 1, interpolate / 90),
                Mathf.Lerp(0, 1, interpolate / 90),
                1f
            );
            yield return new WaitForSeconds(1f / 60f);
        }
        StartCoroutine(EnterAmbition());
    }



        IEnumerator EnterAmbition() {
        final_level = false;
        yield return new WaitForSeconds(1.5f);
        float interpolate = 0f;
        while(interpolate != 90f) {
            interpolate += 1;
            GameObject.Find("Ambition AnimController").transform.position = new Vector3(Mathf.Lerp(-8.88f, -4.44f, Mathf.Sin(interpolate * Mathf.Deg2Rad)), 0f, 4f);
            GameObject.Find("Ambition AnimController").GetComponent<SpriteRenderer>().color = new Color(
                Mathf.Lerp(0, 1, interpolate / 90),
                Mathf.Lerp(0, 1, interpolate / 90),
                Mathf.Lerp(0, 1, interpolate / 90),
                1f
            );
            yield return new WaitForSeconds(1f / 60f);
        }

        yield return StartCoroutine(RunDialogue(ambition_dialogue));

        while(interpolate != 0f) {
            interpolate += -1;
            GameObject.Find("Ambition AnimController").transform.position = new Vector3(Mathf.Lerp(-8.88f, -4.44f, Mathf.Sin(interpolate * Mathf.Deg2Rad)), 0f, 4f);
            GameObject.Find("Ambition AnimController").GetComponent<SpriteRenderer>().color = new Color(
                Mathf.Lerp(0, 1, interpolate / 90),
                Mathf.Lerp(0, 1, interpolate / 90),
                Mathf.Lerp(0, 1, interpolate / 90),
                1f
            );
            yield return new WaitForSeconds(1f / 60f);
        }
        StartCoroutine(GameObject.Find("a straight up black screen").GetComponent<Black_Screen_Fade_Out>().FadeIn());
    }



    IEnumerator RunDialogue(TextAsset character_file) {
        List<string> lines = character_file.text.Split("\n").Select(line => line.Trim()).ToList();
        Parser parser = new Parser(lines);
        Node root = parser.ParseFull();
        engine = new DialogueEngine(root);

        bool ended = false;
        while(!ended) {
            node = engine.GetNext();

            if(node.Type.ToString() == "end") {
                ended = true;
            }

            if(node.Type.ToString() == "scene") {
                if(hand_transitions.Contains(node.Name)) {
                    hand_anim_controller.SetTrigger(node.Name);
                } else if(node.Name == "heart_place") {
                    GameObject.Find("Soul Anim").GetComponent<Animator>().SetTrigger(node.Name);
                } else if(node.Name == "scale_tip") {
                    node = engine.GetNext();
                    StartCoroutine(ScaleTip(float.Parse(node.Name)));
                } else if(node.Name == "level_start") {
                    StartCoroutine(GameObject.Find("Drip AnimController").GetComponent<Level_Switch_Transition>().InkOut());
                } else if(node.Name == "wait_for_level_completion") {
                    GameObject.Find("Player").GetComponent<TM_Player_Movement>().move_enabled = true;
                    waiting_for_level_completion = true;
                } else if(node.Name.Split("_")[0] == "tutorial") {
                    GameObject.Find("TutorialBox").GetComponent<TMP_Text>().text = tutorials[int.Parse(node.Name.Split("_")[1])];
                } else if(node.Name == "wait_for_level_end") {
                    GameObject.Find("Player").GetComponent<TM_Player_Movement>().move_enabled = true;
                    waiting_for_level_completion = true;
                    final_level = true;
                } else if(node.Name == "heart_dissipate") {
                    GameObject.Find("Soul Anim").GetComponent<Animator>().SetTrigger(node.Name);
                } else {
                    yield return new WaitForSeconds(float.Parse(node.Name));
                }
            }

            if(node.Type.ToString() == "person_think") {
                yield return StartCoroutine(DialogueBox(node.Name, node.Content, true));
            } else if(node.Type.ToString() == "person_say") {
                yield return StartCoroutine(DialogueBox(node.Name, node.Content, false));
            }

            if(node.Type.ToString() == "option") {
                yield return StartCoroutine(OptionSelection(node.Choices.ToArray()));
            }

            while(waiting_for_level_completion) {
                yield return null;
            }
        }
    }

    IEnumerator DialogueBox(string name, string content, bool italics) {
        head_button.SetActive(true);
        body_button.SetActive(true);
        arrow.SetActive(false);
        head_text.GetComponent<TMP_Text>().text = "";
        body_text.GetComponent<TMP_Text>().text = "";

        switch(name) {
            case "sinner":
                head_text.GetComponent<TMP_Text>().color = new Color(217f/255f, 54f/255f, 114f/255f);
                head_text.GetComponent<TMP_Text>().text = "SINNER:";
                break;
            case "jury":
                head_text.GetComponent<TMP_Text>().color = new Color(242f/255f, 188f/255f, 87f/255f);
                head_text.GetComponent<TMP_Text>().text = "YOU:";
                break;
            case "narration":
                head_text.GetComponent<TMP_Text>().color = new Color(80f/255f, 97f/255f, 191f/255f);
                head_text.GetComponent<TMP_Text>().text = "MEMORY:";
                break;
        }

        if(italics) {
            body_text.GetComponent<TMP_Text>().fontStyle = FontStyles.Italic;
        } else {
            body_text.GetComponent<TMP_Text>().fontStyle = FontStyles.Normal;
        }

        foreach(char c in content) {
            body_text.GetComponent<TMP_Text>().text += c;
            yield return new WaitForSeconds(0.02f);
        }

        arrow.SetActive(true);

        waiting_for_click = true;
        while(waiting_for_click) {
            yield return null;
        }

        head_button.SetActive(false);
        body_button.SetActive(false);
        arrow.SetActive(false);
    }

    IEnumerator OptionSelection(string[] choices) {
        head_button.SetActive(true);
        body_button.SetActive(true);
        arrow.SetActive(false);
        head_text.GetComponent<TMP_Text>().text = "";
        body_text.GetComponent<TMP_Text>().text = "";

        head_text.GetComponent<TMP_Text>().color = new Color(242f/255f, 188f/255f, 87f/255f);
        head_text.GetComponent<TMP_Text>().text = "YOU:";

        body_text.GetComponent<TMP_Text>().fontStyle = FontStyles.Normal;

        foreach(string choice in choices) {
            body_text.GetComponent<TMP_Text>().text += choice;
            body_text.GetComponent<TMP_Text>().text += "\n";
        }

        bool waiting_for_choice = true;
        while(waiting_for_choice) {
            if(Input.GetKeyDown(KeyCode.Alpha1)) {
                engine.SelectOption(1);
                waiting_for_choice = false;
            } else if(Input.GetKeyDown(KeyCode.Alpha2)) {
                engine.SelectOption(2);
                waiting_for_choice = false;
            }
            yield return null;
        }

        head_button.SetActive(false);
        body_button.SetActive(false);
        arrow.SetActive(false);
    }

    IEnumerator ScaleTip(float degrees) {
        float interpolate = 0f;
        float original_degrees = GameObject.Find("Scale_Tilt").GetComponent<Rotation_Controller>().rot_degrees;
        while(interpolate != 90f) {
            interpolate += 2f;
            GameObject.Find("Scale_Tilt").GetComponent<Rotation_Controller>().rot_degrees = Mathf.Lerp(original_degrees, degrees, Mathf.Sin(interpolate * Mathf.Deg2Rad));
            yield return new WaitForSeconds(1f / 40f);
        }
    }
}


