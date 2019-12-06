using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_move : MonoBehaviour {
    [SerializeField]
    private Transform targetObj;

    [SerializeField]
    private Vector3 Offset;

    private void Start()
    {
        Offset = transform.position - targetObj.position;
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float x = targetObj.position.x + Offset.x;
        float z = targetObj.position.z + Offset.z;

        transform.position = new Vector3(x, transform.position.y, z);
    }
}
