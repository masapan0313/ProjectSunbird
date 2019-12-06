using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shota_HeadQuarters : MonoBehaviour {

    private List<shota_NodePoint> NodeList = new List<shota_NodePoint>(); // シーン上に存在する全ての通過点を保持
    private List<GameObject> Police_List = new List<GameObject>(); // 警備員保持

    [SerializeField] shota_Astar _Astar; // A*を利用するために用意

    private void Awake()
    {
        Police_List.AddRange(GameObject.FindGameObjectsWithTag("Police"));
        SecureNodes();
    }


    public void Alart(shota_NodePoint goal_point)
    {
        shota_NodePoint startPoint;
        for(int i = Police_List.Count - 1; i >= 0; i--)
        {
            startPoint = Police_List[i].GetComponent<shota_Police>().GetNextTargetNode();
            Police_List[i].GetComponent<shota_Police>().SetAStarPoints(_Astar.Astar(startPoint, goal_point), shota_Enemy.ENEMY_STATE.ALARM);
            NodeInit();
        }
    }

    public void SecureNodes()
    {
        // シーン上に存在するタグ名がNodeのオブジェクト全てを確保し、
        // Node情報を抜き出す
        List<GameObject> nodeObj = new List<GameObject>();
        nodeObj.AddRange(GameObject.FindGameObjectsWithTag("Node"));
        foreach (GameObject no in nodeObj)
        {
            NodeList.Add(no.GetComponent<shota_NodePoint>());
        }
    }

    // 初期化
    public void NodeInit()
    {
        foreach(shota_NodePoint node in NodeList)
        {
            node.Init();
        }
    }
}
