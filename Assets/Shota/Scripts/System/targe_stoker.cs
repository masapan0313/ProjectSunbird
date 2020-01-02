using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targe_stoker : MonoBehaviour {

    [SerializeField]
    GameObject target;

    private void Update()
    {
        transform.position =
            new Vector3(
            target.transform.position.x,
            transform.position.y,
            target.transform.position.z);
    }
}
