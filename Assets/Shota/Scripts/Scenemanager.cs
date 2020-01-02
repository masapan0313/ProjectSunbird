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
        coanim[1].Play("over", 0);
        isEnd = true;
    }

    public void Clear()
    {
        MoveStop();
        coanim[0].Play("clear", 0);
        isEnd = true;
    }


    private void MoveStop()
    {
        // 動くやつ全員止める
        player.GetComponent<shota_Player_Move>().enabled = false;
        for (int i = police.Length - 1; i >= 0; i--)
        {
            police[i].GetComponent<shota_Police>().enabled = false;
        }
        for (int i = Doron.Length - 1; i >= 0; i--)
        {
            Doron[i].GetComponent<shota_Doron>().enabled = false;
        }
    }
}

// マジでギリギリなので適当
