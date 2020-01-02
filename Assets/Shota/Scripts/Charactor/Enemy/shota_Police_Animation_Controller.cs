using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shota_Police_Animation_Controller : MonoBehaviour {

    [SerializeField] Animator animator;

    private float moveSpeed; // 現在の移動速度(アニメーションの再生速度に影響)

    private bool isMove; // 移動中か、立ち止まっているか

    public void SetSpeed(float nowSpeed)
    {
        moveSpeed = nowSpeed;
        animator.SetFloat("Speed", moveSpeed);
    }
    public void SetisMove(bool a)
    {
        isMove = a;
        animator.SetBool("isMove", isMove);
    }
}
