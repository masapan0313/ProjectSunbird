using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Title_UI_Utility : MonoBehaviour {

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip[] audioClips;

    [SerializeField]
    GameObject Cursor,Next_Page,Back_Page,Current_Page,Stage_Map,TargetObj;

    [SerializeField]
    GameObject Target_Page_First_Button,Current_First_Button; // ページ変更時に最初にフォーカスされるボタン

    [SerializeField]
    int myID;
    public void Quit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
        #endif
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) Quit();
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            Cursor.SetActive(true);
        }
        else
        {
            Cursor.SetActive(false);
        }
    }

    public void C_Active_Cursor()
    {
        if (!Cursor.activeSelf)
        {
            Cursor.SetActive(true);
        }
        else
        {
            Cursor.SetActive(false);
        }
    }

    public void OpenNextPage()
    {
        Next_Page.SetActive(true);
        Current_Page.SetActive(false);
        EventSystem.current.SetSelectedGameObject(Target_Page_First_Button);
    }

    public void OpenBackPage()
    {
        Back_Page.SetActive(true);
        Current_Page.SetActive(false);
        EventSystem.current.SetSelectedGameObject(Target_Page_First_Button);

    }

    public void C_Select()
    {
        TargetObj.GetComponent<Target_Obj_Switch>().SwitchSelect(myID);
    }

    public void PlaySoundEffect_0()
    {
        audioSource.PlayOneShot(audioClips[0]);
    }
    public void PlaySoundEffect_1()
    {
        audioSource.PlayOneShot(audioClips[1]);
    }

}
