using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shota_Color_Switch : MonoBehaviour {

    [SerializeField] private shota_lock_state locker;

    private shota_Enum_list.SWITCH_COLOR nowColor; // 現在の色
    private shota_Enum_list.KEY_MARK myMark; // 対応するマーク

    [SerializeField] private KeyCode myKeycode; // テスト用（本来はエリア内でアクションだが、テストでは指定したキーで行動を起こす）

    private int proxyColor;

    private int enumElementCount = System.Enum.GetNames(typeof(shota_Enum_list.SWITCH_COLOR)).Length;

    [SerializeField] Image image;

    private void Start()
    {
        proxyColor = (int)nowColor;
        imageUpdate();
        myMark = locker.GetMark();
    }
    private void Update()
    {
        if (Input.GetKeyDown(myKeycode))
        {
            ChangeColor();
        }   
    }
    
    // 上から順に加算されていく
    // 順番固定か選択式か選べるようにしたほうがいいかもね
    public void ChangeColor()
    {
        proxyColor++;

        if(proxyColor >= enumElementCount)
        {
            proxyColor = 0;
        }

        nowColor = (shota_Enum_list.SWITCH_COLOR)proxyColor;
        imageUpdate();

        transform.GetComponentInParent<shota_Color_Box>().KeyCheck();
    }

    public shota_Enum_list.SWITCH_COLOR GetNowColor()
    {
        return nowColor;
    }

    public shota_Enum_list.KEY_MARK GetKeyMark()
    {
        return myMark;
    }

    private void imageUpdate()
    {
        switch (nowColor)
        {
            case shota_Enum_list.SWITCH_COLOR.RED:
                image.color = new Color(255 / 255, 0, 0, 255 / 255);

                break;
            case shota_Enum_list.SWITCH_COLOR.GREEN:
                image.color = new Color(0, 255 / 255, 0, 255 / 255);
                break;
            case shota_Enum_list.SWITCH_COLOR.BLUE:
                image.color = new Color(0, 0, 255 / 255, 255 / 255);
                break;
            default:
                break;
        }
    }
}
