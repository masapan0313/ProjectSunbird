using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableChange : MonoBehaviour {
    [SerializeField]
    shota_Player_Move spm;

    public void Offenabled()
    {
        spm.enabled = false;
    }

    public void Onenable()
    {
        spm.enabled = true;
    }
}
