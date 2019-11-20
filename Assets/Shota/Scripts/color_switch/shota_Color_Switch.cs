using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shota_Color_Switch : MonoBehaviour {

    [SerializeField] private shota_lock_state locker;

    private shota_Enum_list.SWITCH_COLOR nowColor; // 現在の色
    private shota_Enum_list.KEY_MARK myMark; // 対応するマーク

    [SerializeField] private KeyCode myKeycode; // テスト用（本来はエリア内でアクションだが、テストでは指定したキーで行動を起こす）

    private int proxyColor; // 色変更の際に列挙を触るのは嫌なので代理で更新される変数

    private int enumElementCount = System.Enum.GetNames(typeof(shota_Enum_list.SWITCH_COLOR)).Length; // 色一巡した後リセットするための制限用変数

    MeshRenderer image;
    // MeshRenderは開始時に設定されたマテリアルを個別にインスタンスしているらしい。のでそれを触る

    private void Start()
    {
        proxyColor = (int)nowColor;
        image = gameObject.GetComponent<MeshRenderer>();
        imageUpdate();
        myMark = locker.GetMark();

    }
    private void Update()
    {
        if (Input.GetKeyDown(myKeycode))
        {
            //ChangeColor();
        }   
    }

    private void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.Return))
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

        // ここ親子関係にして楽してるけど、どの構造がいいのか不明なのでそのまま
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


    // 色更新
    private void imageUpdate()
    {
        switch (nowColor)
        {
            case shota_Enum_list.SWITCH_COLOR.RED:
                image.material.color = new Color(255 / 255, 0, 0, 255 / 255);

                break;
            case shota_Enum_list.SWITCH_COLOR.GREEN:
                image.material.color = new Color(0, 255 / 255, 0, 255 / 255);
                break;
            case shota_Enum_list.SWITCH_COLOR.BLUE:
                image.material.color = new Color(0, 0, 255 / 255, 255 / 255);
                break;
            default:
                break;
        }
    }
}
