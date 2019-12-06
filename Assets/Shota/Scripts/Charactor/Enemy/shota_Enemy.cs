using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ENEMY_PROPERTIE",menuName = "ENEMY_PROPERTIE")]

public class shota_Enemy : ScriptableObject {

    public enum ENEMY_STATE
    {
        PATROL, // 巡回
        NMA_SCRPT, // ナビメッシュエージェントによる移動から通常の移動方法へ切り替える
        ALARM, // 集合
        RUN, // 追跡
        BACK, // 巡回に戻る
    }

    [SerializeField] private float[] walk_Speed = new float[5]; // 各行動目標により切り替える

    public float[] GetWalkSpeed()
    {
        return walk_Speed;
    }
}
