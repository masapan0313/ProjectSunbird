using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target_Obj_Switch : MonoBehaviour {

    [SerializeField]
    GameObject Target_Obj; // 宝
    

    [SerializeField]
    GameObject[] targetimages;
    

    [SerializeField]
    Material[] Materials; // 宝の色

    [SerializeField]
    Color[] LightColor;

    [SerializeField]
    Light SpotLight; // 宝によって光色も変える




    public void SwitchSelect(int id)
    {

        for(int i = Materials.Length -1; i >= 0; i--)
        {
            targetimages[i].SetActive(false);
            if(i == id)
            {
                Target_Obj.GetComponent<MeshRenderer>().material = Materials[id];
                SpotLight.color = LightColor[id];
                targetimages[i].SetActive(true);
            }
        }
    }
}