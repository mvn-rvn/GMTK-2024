using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard_Input_Handler : MonoBehaviour
{
   
    public string HandleInput() {
        if(Input.GetButtonDown("Reload")) {
            return "reload";
        } else if(Input.GetButtonDown("Grab")) {
            return "grab";
        } else if(Input.anyKeyDown) {
            switch(Input.GetAxisRaw("Vertical")) {
                case 1:
                    return "up";
                case -1:
                    return "down";
            }
            switch(Input.GetAxisRaw("Horizontal")) {
                case 1:
                    return "right";
                case -1:
                    return "left";
            }
            return "bro idk what to tell you";
        } else {
            return "bro idk what to tell you";
        }
    }
}
