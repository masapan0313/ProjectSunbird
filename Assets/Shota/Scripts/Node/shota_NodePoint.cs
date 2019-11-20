using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shota_NodePoint : MonoBehaviour {

    public enum NodeStatus
    {
        NONE,
        OPEN,
        CLOSE,
    }

    private Vector3 nodePos;
    [SerializeField] private int ThroughCost; // 通過するためのコスト（基本料金）// 大きいと避けがち
    private int MoveToTargetCost; // 対象へ移動するために払うコスト（追加料金）// 小さいと優先的に選ぶ
    private int TotalCost; // 計コスト
    [SerializeField] private List<shota_NodePoint> ConnectNode; // 繋がっているノード
    private NodeStatus status; // 計算済みか確認
    private shota_NodePoint parentNode = null; // 親ノード(経路探索に使用)

    private void Awake()
    {
        nodePos = gameObject.transform.position;
    }


    // 実際にかかるコスト
    public void CalcTotalCost()
    {
        TotalCost = ThroughCost + MoveToTargetCost;
    }

    // 実移動コストの設定
    public void SetMoveToTargetcost(int mttc)
    {
        MoveToTargetCost = mttc;
    }

    // 実移動コストを返す
    public int GetTotalCost()
    {
        return TotalCost;
    }

    // 親ノードの設定（経路探索で使う）
    public void SetParentNode(shota_NodePoint parentnode)
    {
        parentNode = parentnode;
    }
    // 親のノードを返す
    public shota_NodePoint GetParentNode()
    {
        return parentNode;
    }

    // 探査状態の設定
    public void SetNodeStatus(NodeStatus ns)
    {
        status = ns;
    }

    // ノードの探査状態を返す
    public NodeStatus GetNodeStatus()
    {
        return status;
    }

    // 隣接するノードを渡す
    public List<shota_NodePoint> GetConnectNode()
    {
        return ConnectNode;
    }


    // 座標の取得
    public Vector3 GetNodePos()
    {
        return nodePos;
    }

    // 初期化する
    public void Init()
    {
        MoveToTargetCost = 0;
        TotalCost = 0;
        status = NodeStatus.NONE;
        parentNode = null;
    }
}
