﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shota_Color_Box : MonoBehaviour {

    [SerializeField] private List<shota_lock_state> LockList; // 鍵穴

    [SerializeField] private List<shota_Color_Switch> KeyTarget; // 鍵束

    [SerializeField] private Text text; // テスト用

    string str; // テスト用

    private void Start()
    {
        KeyTarget.Sort((a,b) => a.GetKeyMark() - b.GetKeyMark());
        LockList.Sort((a, b) => a.GetMark() - b.GetMark());

        str += "いろあわせ \n";
        foreach(shota_lock_state ll in LockList)
        {
            str += "マーク：";
            str += ll.GetMark().ToString();
            str += "色：";
            str += ll.GetSwitchColor().ToString();
            str += " \n ";
        }
        text.text = str;

        
    }

    public void KeyCheck()
    {
        int opencount = 0;
        int KeyIndexNum = 0;
        //List<shota_Color_Switch> keytarget_clone = new List<shota_Color_Switch>(KeyTarget);

        foreach (shota_lock_state ll in LockList)
        {
            if (KeyTarget[KeyIndexNum].GetNowColor() == ll.GetSwitchColor())
            {
                opencount++;
                KeyIndexNum++;
            }
            else
            {
                break;
            }
        }

        if(opencount == LockList.Count)
        {
            Debug.Log("すべてそろいました!!!!!!!!!!!!!");
        }
    }
    
}


// リスト二つ同期させないといけないのにも関わらず、サイズは手動で合わせないといけないのは
// いかがなものかと思うので直せるなら直してください。