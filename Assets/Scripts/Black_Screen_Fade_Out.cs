using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Black_Screen_Fade_Out : MonoBehaviour
{
    SpriteRenderer sprite_rend;
    
    // Start is called before the first frame update
    void Start()
    {
        sprite_rend = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(Fade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Fade() {
        yield return new WaitForSeconds(0.5f);
        while(sprite_rend.color.a > 0) {
            sprite_rend.color += new Color(1f, 1f, 1f, -0.4f * Time.deltaTime);
            yield return null;
        }
        GameObject.Find("Hand AnimController").GetComponent<Animator>().enabled = true;
    }

    public IEnumerator FadeIn() {
        yield return new WaitForSeconds(0.5f);
        while(sprite_rend.color.a < 1) {
            sprite_rend.color += new Color(1f, 1f, 1f, 0.4f * Time.deltaTime);
            yield return null;
        }
        SceneManager.LoadScene("END");
    }
}
