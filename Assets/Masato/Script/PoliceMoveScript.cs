using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceMoveScript : MonoBehaviour {

    public Transform target;
    public float speed = 0.1f;
    private Vector3 MyVec;
    private Vector3 TargetVec;

    void Start()
    {
        target = GameObject.Find("thief").transform;
    }

    void Update()
    {
        MyVec = this.transform.position;
        TargetVec = target.transform.position;

        this.transform.LookAt(target.transform);

        if (MyVec.x - TargetVec.x < 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 0.3f);
            transform.position += transform.forward * speed;
        }
    }
}
