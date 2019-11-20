using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shota_Player_Move : MonoBehaviour {
//    Animator animator;

//	void Start () {
//        animator = gameObject.GetComponent<Animator>();
//	}
	[SerializeField] float speed = 0.5F;
    [SerializeField] public float rotationSpeed = 10.0F;

    private float MoveDir_H, MoveDir_V;

    Rigidbody rb;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(MoveDir_H, 0, MoveDir_V) * speed * Time.deltaTime;
    }

    void Update()
    {

        // Raw(0,1)を返す,加速入れたい場合はRawを消し、増加値と最高速度を設定
        // 回転に関してはモデル導入次第検討(メインカメラも巻き込まれるから階層には注意)
        MoveDir_H = Input.GetAxisRaw("Horizontal");
        MoveDir_V = Input.GetAxisRaw("Vertical");

        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    //animator.SetBool("is_Walk", true);
        //    transform.position += transform.forward * (speed * Time.deltaTime);
        //}
        //else
        //{
        //    //animator.SetBool("is_Walk", false);
        //}

        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    //animator.SetBool("is_LeftWalk", true);
        //    transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
        //}
        //else
        //{
        //    //animator.SetBool("is_LeftWalk", false);
        //}
        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    //animator.SetBool("is_RightWalk", true);
        //    transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        //}
        //else
        //{
        //    //animator.SetBool("is_RightWalk", false);
        //}
    }
}
