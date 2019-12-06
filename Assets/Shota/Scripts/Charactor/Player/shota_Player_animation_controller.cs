using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shota_Player_animation_controller : MonoBehaviour {

    [SerializeField]
    private Animator animator;

    public void SetMove(bool a)
    {
        animator.SetBool("isMove", a);
    }

    public void SetSpeed()
    {

    }

    public void Action()
    {
        animator.SetTrigger("PickUp");
    }

}
