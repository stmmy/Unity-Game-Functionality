using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_legs : MonoBehaviour
{
    //Legs animation script
    Animator animator;
    string currentState;
    const string RUN_RIGHTLEFT = "run_rightleft";
    const string RUN_UPDOWN = "run_updown";
    const string RUN_IDLE = "run_idle";

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

        if (a_check == true || d_check == true) { ChangeAnimationState(RUN_RIGHTLEFT); }
        else if (w_check == true || s_check == true) { ChangeAnimationState(RUN_UPDOWN); }
        else { ChangeAnimationState(RUN_IDLE); }
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return; //Stop animation from interrupting
        animator.Play(newState);
        currentState = newState;
    }
}
