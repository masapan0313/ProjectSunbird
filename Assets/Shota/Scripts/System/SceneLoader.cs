using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    [SerializeField]
    string TargetScene;
    

    public void LoadScene()
    {
        // inspectorで指定された名前のシーンをロードする
        SceneManager.LoadScene(TargetScene);
    }

}

// 事前に読み込むシーンを登録するのを忘れずに！