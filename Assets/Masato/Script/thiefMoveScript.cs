using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thiefMoveScript : MonoBehaviour {
    public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // transformを取得
        Transform myTransform = this.transform;

        // 座標を取得
        Vector3 pos = myTransform.position;
        if (Input.GetKey(KeyCode.A)) pos.x -= speed;
        if (Input.GetKey(KeyCode.D)) pos.x += speed;
        if(Input.GetKey(KeyCode.W)) pos.y += speed;
        if (Input.GetKey(KeyCode.S)) pos.y -= speed;

        myTransform.position = pos;  // 座標を設定
    }
}
