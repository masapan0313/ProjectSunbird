using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor_invisible : MonoBehaviour {
    
    // 各シーンに必ず存在するオブジェクトに付与
    // マウスカーソルの非表示

	void Start () {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
	}
}
