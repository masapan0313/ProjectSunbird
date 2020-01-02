using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

    [SerializeField]
    GameObject TargetObj; // カメラを見たい

    private void FixedUpdate()
    {
        transform.LookAt(TargetObj.transform);
    }
}
