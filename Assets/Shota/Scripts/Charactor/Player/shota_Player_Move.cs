using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shota_Player_Move : MonoBehaviour {
//    Animator animator;

//	void Start () {
//        animator = gameObject.GetComponent<Animator>();
//	}
	[SerializeField]
    float speed = 0.5F;
    [SerializeField]
    public float rotationSpeed = 10.0F;

    private float MoveDir_H, MoveDir_V;

    Rigidbody rb;

    [SerializeField]
    shota_Player_animation_controller spac;

    [SerializeField]
    KeyCode ActionButton;

    private Vector3 lookDir;

    [SerializeField]
    AudioClip[] selist;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(MoveDir_H, 0, MoveDir_V) * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(lookDir);
    }

    void Update()
    {
        // Raw(0,1)を返す,加速入れたい場合はRawを消し、増加値と最高速度を設定
        //MoveDir_H = Input.GetAxisRaw("Horizontal");
        //MoveDir_V = Input.GetAxisRaw("Vertical");
        MoveDir_H = Input.GetAxis("Horizontal");
        MoveDir_V = Input.GetAxis("Vertical");



        if (MoveDir_H == 0 && MoveDir_V == 0)
        {
            spac.SetMove(false);
        }
        else
        {
            lookDir = new Vector3(MoveDir_H, 0, MoveDir_V);
            spac.SetMove(true);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        // 対象Objのエリア内にいるときに特定の行動をすると...

        // 仮でgimmick/oooという形を想定している
        if(TagUtility.getParentTagName(other.tag) == "gimmick")
        {
            if (Input.GetKeyDown(ActionButton))
            {
                spac.Action();
            }
        }
    }
}
