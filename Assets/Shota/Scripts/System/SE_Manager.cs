using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE_Manager : MonoBehaviour {

    public void PlaySE(AudioClip se)
    {
        GetComponent<AudioSource>().PlayOneShot(se);
    }
}
