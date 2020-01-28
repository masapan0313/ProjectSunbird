using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenemanager : MonoBehaviour {

    [SerializeField]
    Animator[] coanim;

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject[] police;

    [SerializeField]
    GameObject[] Doron;

    [SerializeField]
    SceneLoader sl;

    bool isEnd = false;

    [SerializeField]
    AudioSource aS;

    [SerializeField]
    AudioClip[] audioClips;

    private void Update()
    {
        if (isEnd)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                sl.LoadScene();
            }
        }
    }

    public void Over()
    {
        MoveStop();
        aS.PlayOneShot(audioClips[0]);
        coanim[1].Play("over", 0);
        isEnd = true;
    }

    public void Clear()
    {
        MoveStop();
        aS.PlayOneShot(audioClips[1]);
        coanim[0].Play("clear", 0);
        isEnd = true;
    }


    private void MoveStop()
    {
        // 動くやつ全員止める
        player.GetComponent<shota_Player_Move>().enabled = false;
        for (int i = police.Length - 1; i >= 0; i--)
        {
            police[i].SetActive(false);
        }
        for (int i = Doron.Length - 1; i >= 0; i--)
        {
            Doron[i].SetActive(false);
        }
    }
}

// マジでギリギリなので適当
