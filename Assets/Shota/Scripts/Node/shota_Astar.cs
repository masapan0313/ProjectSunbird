using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shota_Astar : MonoBehaviour {

    public List<shota_NodePoint> Astar(shota_NodePoint StartNode, shota_NodePoint GoalNode)
    {
        int cost = 0; // 実コスト
        bool isGoal = false; // ゴールを発見しているか
        bool isStart = false; // 開始地点まで遡れたか
        int index = 0; // 遡るための添字
        List<shota_NodePoint> astar_root = new List<shota_NodePoint>(); // 出た最短経路をこのリストに入れる
        List<shota_NodePoint> openlist = new List<shota_NodePoint>(); // 今探査中のノードを格納するリスト

        // スタートノードのコスト設定
        StartNode.SetMoveToTargetcost(cost);
        // 通過コストと実コストを加算してスコアを算出
        StartNode.CalcTotalCost();
        // 探査範囲を広げる度にコストを増やす
        cost++;
        // 一通りの設定が完了し、探査範囲を広げる際自身の状態を探査終了に更新する
        StartNode.SetNodeStatus(shota_NodePoint.NodeStatus.CLOSE);

        // 隣接するノードの親に自身を設定しつつ、状態を探査中に変更し、探査中リストに入れる
        for (int i = StartNode.GetConnectNode().Count -1; i >= 0; i--)
        {
            if(StartNode.GetNodePos() == GoalNode.GetNodePos())
            {
                // スタート地点がゴールならそのまま返す
                astar_root.Add(StartNode);
                return astar_root;
                // 意味不明なので解説
                // 現在のルートの「次の目的地」を開始地点としているので、そこにゴール地点が設定されたらそのまま進ませる
            }

            StartNode.GetConnectNode()[i].SetParentNode(StartNode);

            // ゴールチェック  
            if (GoalNode.GetNodePos() == StartNode.GetConnectNode()[i].GetNodePos())// ゴール地点だった場合
            {
                astar_root.Add(StartNode.GetConnectNode()[i]);
                isGoal = true;
                break;
            }

            StartNode.GetConnectNode()[i].SetNodeStatus(shota_NodePoint.NodeStatus.OPEN);
            StartNode.GetConnectNode()[i].SetMoveToTargetcost(cost);
            StartNode.GetConnectNode()[i].CalcTotalCost();
            openlist.Add(StartNode.GetConnectNode()[i]);
        }
        
        while (!isGoal)
        {
            // ループが逆順で回しているのでスコアソートも降順で行う
            openlist.Sort((a, b) => b.GetTotalCost() - a.GetTotalCost());
            for (int i = openlist.Count-1; i >= 0; i--)
            {
                // ゴールチェック  
                if (GoalNode.GetNodePos() == openlist[i].GetNodePos())// ゴール地点だった場合
                {
                    astar_root.Add(openlist[i]);
                    isGoal = true;
                    break;
                }
                else // 通常処理
                {

                    if (openlist[i].GetConnectNode() != null)
                    {
                        for(int j = openlist[i].GetConnectNode().Count - 1; j >= 0; j--)
                        {
                            // 未探査か確認
                            if (openlist[i].GetConnectNode()[j].GetNodeStatus() != shota_NodePoint.NodeStatus.CLOSE &&
                                openlist[i].GetConnectNode()[j].GetNodeStatus() != shota_NodePoint.NodeStatus.OPEN)
                            {
                                // 探査中に更新
                                openlist[i].GetConnectNode()[j].SetNodeStatus(shota_NodePoint.NodeStatus.OPEN);
                                // 実コストを設定しスコアを算出する
                                openlist[i].SetMoveToTargetcost(cost);
                                openlist[i].CalcTotalCost();
                                // 未探査であるなら親に自身を設定
                                openlist[i].GetConnectNode()[j].SetParentNode(openlist[i]);
                                // 探査中リストに追加
                                openlist.Add(openlist[i].GetConnectNode()[j]);
                            }
                        }
                    }
                    // 拡大完了次第自身の状態を探索済みに設定
                    openlist[i].SetNodeStatus(shota_NodePoint.NodeStatus.CLOSE);
                    // 設定後探査中リストから除外
                    openlist.RemoveAt(i);
                }
            }
            cost++;
        }
        
        // ゴールを見つけたので経路をリストに追加していく
        while (!isStart)
        {
            astar_root.Add(astar_root[index].GetParentNode());
            index++;
            // 開始ノードと確認出来たら（ここでは座標を比較）
            if(astar_root[index].GetNodePos() == StartNode.GetNodePos())
            {
                // ゴール地点　－＞　スタート地点　の順で追加されているので反転させる
                astar_root.Reverse();
                // 出た経路を返す
                return astar_root;
            }
        }

        return null; // 想定外のミスがある場合nullを投げる
    }

    public List<shota_NodePoint> Astar(shota_NodePoint StartNode, List<shota_NodePoint> GoalNodes)
    {
        int cost = 0; // 実コスト
        int index = 0; // 遡るための添字
        shota_NodePoint GoalNode = null; // 対象に含まれているなら何でもいいので入れる
        List<shota_NodePoint> openlist = new List<shota_NodePoint>(); // 今探査中のノードを格納するリスト
        List<shota_NodePoint> astar_root = new List<shota_NodePoint>(); // 出た最短経路をこのリストに入れる
        
        SetNodePropertie(StartNode, cost); // 開始地点の設定
        cost++;
        for(int i = StartNode.GetConnectNode().Count-1; i >= 0; i--)
        {
            StartNode.GetConnectNode()[i].SetParentNode(StartNode);
            SetNodePropertie(StartNode.GetConnectNode()[i],cost);
            openlist.Add(StartNode.GetConnectNode()[i]);
        }
        cost++;
        StartNode.SetNodeStatus(shota_NodePoint.NodeStatus.CLOSE); // 隣接ノードを探査リストに追加したら閉じる

        

        // 全てのノードにスコア付けをする
        while (openlist.Count != 0)
        {
            // スコアを基準にして並び変える
            openlist.Sort((a, b) => b.GetTotalCost() - a.GetTotalCost());

            // 現在探査中のノードにスコア付けをする
            for(int i = openlist.Count-1; i >= 0; i--)
            {
                for (int j = openlist[i].GetConnectNode().Count - 1; j >= 0; j--)
                {
                    // 未探査であるなら探査中リストに追加し自身を親ノードとして設定する
                    if (openlist[i].GetConnectNode()[j].GetNodeStatus() != shota_NodePoint.NodeStatus.OPEN
                        && openlist[i].GetConnectNode()[j].GetNodeStatus() != shota_NodePoint.NodeStatus.CLOSE)
                    {
                        openlist[i].GetConnectNode()[j].SetParentNode(openlist[i]);
                        SetNodePropertie(openlist[i].GetConnectNode()[j], cost);
                        openlist.Add(openlist[i].GetConnectNode()[j]);
                    }
                }
                openlist[i].SetNodeStatus(shota_NodePoint.NodeStatus.CLOSE);
                openlist.RemoveAt(i);
            }
            cost++;
        }


        for(int i = GoalNodes.Count-1; i >= 0; i--)
        {
            if (GoalNode == null)
            {
                GoalNode = GoalNodes[i];
            }
            else
            {
                // より開始地点から求めたスコアが小さいものを終点とする
                if(GoalNode.GetTotalCost() > GoalNodes[i].GetTotalCost())
                {
                    GoalNode = GoalNodes[i];
                }
            }
        }

        astar_root.Add(GoalNode);


        // 各ノードに設定された親ノードを辿り、開始地点までリストに追加していく
        while (index < 100)
        {
            if (astar_root[index].GetParentNode() != null)
            {
                astar_root.Add(astar_root[index].GetParentNode());
            }
            else
            {
                // 開始地点の親は未設定(null)
                break;
            }
            index++;
        }

        // 終点　－＞　始点　の順なので反転させる
        astar_root.Reverse();

        return astar_root;
    }

    private void SetNodePropertie(shota_NodePoint node, int cost)
    {
        node.SetNodeStatus(shota_NodePoint.NodeStatus.OPEN);
        node.SetMoveToTargetcost(cost);
        node.CalcTotalCost();
    }
}
