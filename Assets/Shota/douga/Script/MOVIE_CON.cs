using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVIE_CON : MonoBehaviour {
    
    [SerializeField]
    List<Animator> animators;

    [SerializeField]
    bool Title_Mode;

    [SerializeField]
    GameObject titletext,TitleTop;

    [SerializeField]
    float[] staytime; // 考えるの面倒...時間ないので時限で管理

    float timeCount;

    bool CountTrigger = false;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!Title_Mode)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                animators[0].Play("fade", 0);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                animators[1].Play("cam_1", 0);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                animators[1].Play("cam_2", 0);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                animators[2].Play("SPOT_LIGHT_MOVE", 0);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                animators[3].Play("Look_Right", 0);
                animators[3].SetBool("ISOGU", true);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                animators[4].Play("color_change2", 0);
                animators[5].Play("color_change", 0);
                animators[3].Play("out", 0);
            }
        }
        else
        {
            if (CountTrigger)
            {
                timeCount += Time.deltaTime;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CountTrigger = true;
                titletext.SetActive(false);
                animators[0].Play("fade", 0);
            }
            if (timeCount > staytime[0] && timeCount < staytime[1])
            {
                animators[1].Play("cam_1", 0);
            }
            if (timeCount > staytime[1] && timeCount < staytime[2])
            {
                animators[1].Play("cam_3", 0);
            }
            if (timeCount > staytime[2] && timeCount < staytime[2]+.2)
            {
                audioSource.loop = false;
                audioSource.Stop();
                TitleTop.SetActive(true);
            }
        }
    }
}
