using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_hands : MonoBehaviour
{
    //Legs animation script
    Animator animator;
    string currentState;
    const string HANDS_IDLE = "hands_idle";
    const string HANDS_LEFT = "hands_right";
    const string HANDS_RIGHT = "hands_left";
    const string HANDS_UPDOWN = "hands_updown";

    bool a_check;
    bool d_check;
    bool w_check;
    bool s_check;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        a_check = Input.GetKey("a");
        d_check = Input.GetKey("d");
        w_check = Input.GetKey("w");
        s_check = Input.GetKey("s");

        if (a_check == true) { ChangeAnimationState(HANDS_RIGHT); }
        else if (d_check == true) { ChangeAnimationState(HANDS_LEFT); }
        else if (w_check == true || s_check == true) { ChangeAnimationState(HANDS_UPDOWN); }
        else { ChangeAnimationState(HANDS_IDLE); }
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return; //Stop animation from interrupting
        animator.Play(newState);
        currentState = newState;
    }
}
