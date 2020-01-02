using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ボタンにつける奴(キー・パッド操作に対応させるやつ)
public class SelectButton : MonoBehaviour
{

    [SerializeField]
    bool First_Select = false;

    private void Start()
    {
        if (First_Select)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }


    public void SelectSelf()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void NonSelectSelf()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
