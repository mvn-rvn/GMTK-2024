using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Switch_Transition : MonoBehaviour
{
    Animator animator;
    GameObject board_wall;
    public bool ink_status = true;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        board_wall = GameObject.Find("BoardWall");
        //PLACEHOLDER FOR TESTING
        //StartCoroutine(InkOut());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator InkOut() {
        animator.SetTrigger("Drip2Game");
        yield return new WaitForSeconds((1f / 8f) * 5f);
        board_wall.transform.position = new Vector3(4.44f, 0f, 6f); //positive means behind gameboard
        yield return new WaitForSeconds((1f / 8f) * 3f);
        ink_status = false;
    }

    public IEnumerator InkIn() {
        GameObject.Find("Player").GetComponent<TM_Player_Movement>().move_enabled = false;
        animator.SetTrigger("Game2Drip");
        yield return new WaitForSeconds((1f / 8f) * 3f);
        board_wall.transform.position = new Vector3(4.44f, 0f, -6f);
        yield return new WaitForSeconds((1f / 8f) * 6f);
        ink_status = true;
    }
}
