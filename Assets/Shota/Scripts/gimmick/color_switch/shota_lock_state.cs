using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu( fileName = "Lock_State", menuName = "Lock_State")]
public class shota_lock_state : ScriptableObject {

    [SerializeField]
    shota_Enum_list.KEY_MARK myMark;

    [SerializeField]
    shota_Enum_list.SWITCH_COLOR myColor;

    public shota_Enum_list.KEY_MARK GetMark()
    {
        return myMark;
    }
    public shota_Enum_list.SWITCH_COLOR GetSwitchColor()
    {
        return myColor;
    }
}
