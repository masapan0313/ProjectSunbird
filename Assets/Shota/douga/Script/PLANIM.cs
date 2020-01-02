using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLANIM : MonoBehaviour {

    [SerializeField]
    Animator target;

    [SerializeField]
    GameObject target_gmobj;


    bool flg;

    private void Update()
    {
        if (flg)
        {
            target_gmobj.transform.Translate(0, 0, 0.5f);
        }
    }

    public void Play()
    {
        target.Play("Walk", 0);
        flg = true;
    }
}
