using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shota_Police : MonoBehaviour {

    [SerializeField] shota_Astar _Astar; // 実体ないと使えないらしい

    [SerializeField] shota_Enemy EnemyPropertie; // 敵の基本情報

    [SerializeField] List<shota_NodePoint> PatrolPoints; // 巡回ルート（手動で設定）
    private List<shota_NodePoint> AStarPoints = new List<shota_NodePoint>(); // 目的地までの最短ルート

    private shota_NodePoint nextTargetPoint = null; // 次の目的地
    private int currentPointNum = 0; // 今の巡回数

    [SerializeField] float ToleranceDistance; // 許容できるガバの大きさ

    [SerializeField] List<float> Speed;
    [SerializeField] shota_Enemy.ENEMY_STATE myState = shota_Enemy.ENEMY_STATE.PATROL;

    private void Start()
    {
        Speed.AddRange(EnemyPropertie.GetWalkSpeed());
    }

    private void FixedUpdate()
    {
        switch (myState)
        {
            case shota_Enemy.ENEMY_STATE.PATROL:
                Patrol();
                break;
            case shota_Enemy.ENEMY_STATE.SEARCH:
                break;
            case shota_Enemy.ENEMY_STATE.ALARM:
                Alart();
                break;
            case shota_Enemy.ENEMY_STATE.RUN:
                break;
            case shota_Enemy.ENEMY_STATE.BACK:
                Back();
                break;
            default:
                break;
        }
    }

    public void Move(List<shota_NodePoint> path , bool looping)
    {
        // 現在の位置から目的の座標まで移動
        if(nextTargetPoint == null)
        {
            nextTargetPoint = path[0];
        }
        
        transform.position 
            = Vector3.MoveTowards(transform.position,
            nextTargetPoint.GetNodePos(), 
            /*Clone_EnemyPropertie.GetNowWalkSpeed()*/Speed[(int)myState] * Time.deltaTime);

        transform.LookAt(nextTargetPoint.GetNodePos());


        // 目標地点まである程度近づいたら目標地点を更新する
        if(Vector3.Distance(transform.position, nextTargetPoint.gameObject.transform.position) < ToleranceDistance )
        {
            if (currentPointNum < path.Count - 1) // 最終地点に到達しているか
            {
                // まだ道中
                currentPointNum++;
                nextTargetPoint = path[currentPointNum];
            }
            else
            {
                // 着いた
                if (looping)
                {
                    // 巡回するのでスタート地点へ
                    currentPointNum = 0;
                    nextTargetPoint = path[currentPointNum];
                }
                else // ループしない場合状態に変化が生じるので目的地の更新は行わない
                {
                    // 巡回ルートから外れているので近い地点に行く
                    if (myState != shota_Enemy.ENEMY_STATE.BACK)
                    {
                        // 巡回ルート上にいる場合
                        if (PatrolPoints.Find(nodePos => path[currentPointNum].GetNodePos() == nodePos.GetNodePos()))
                        {
                            currentPointNum = PatrolPoints.FindIndex(nodePos => path[currentPointNum].GetNodePos() == nodePos.GetNodePos());
                            SetState(shota_Enemy.ENEMY_STATE.PATROL);
                        }
                        else // やっぱり巡回ルートから外れているので戻る
                        {
                            SetAStarPoints(_Astar.Astar(path[currentPointNum], PatrolPoints), shota_Enemy.ENEMY_STATE.BACK);
                            _Astar.GetComponentInParent<shota_HeadQuarters>().NodeInit(); // 探索に用いたすべてのノードの値を初期化
                        }
                    }
                    else
                    {
                        shota_NodePoint nd = path[path.Count-1]; // 終点を保持
                        currentPointNum = PatrolPoints.FindIndex(nodepos => nodepos.GetNodePos() == nd.GetNodePos()); // 終点が巡回点の何番目に位置するのかをみて現在の巡回カウントを更新
                        Debug.Log(currentPointNum); // 実際に正しく動作しているかの確認
                        SetState(shota_Enemy.ENEMY_STATE.PATROL); // 状態を巡回に更新
                    }
                } 
            }
        }
        for (int i = 0; i < path.Count - 1; i++)
        {
            if (i < path.Count - 1)
            {
                Debug.DrawRay(path[i].GetNodePos(), path[i + 1].GetNodePos() - path[i].GetNodePos());
            }
            if(i == path.Count -2)
            {
                Debug.DrawRay(path[path.Count-1].GetNodePos(),  path[0].GetNodePos() - path[path.Count-1].GetNodePos());
            }
        }
    }

    private void Patrol()
    {
        Move(PatrolPoints, true);

        /*
        if(プレイヤーを目視){
           追いかけたり追いかけなかったりする;
        }
        */

    }

    private void Alart()
    {
        Move(AStarPoints, false);
        /*
        if(プレイヤーを目視){
           追いかけたり追いかけなかったりする;
        }
        */
    }
    private void Back()
    {
        Move(AStarPoints, false);
    }

    public shota_NodePoint GetNextTargetNode()
    {
        return nextTargetPoint;
    }

    public void SetAStarPoints(List<shota_NodePoint> ap , shota_Enemy.ENEMY_STATE state)
    {
        AStarPoints.Clear();
        AStarPoints.AddRange(ap);
        currentPointNum = 0;
        SetState(state);
        //Clone_EnemyPropertie.ChangeState(shota_Enemy.ENEMY_STATE.ALARM);
    }

    private void SetState(shota_Enemy.ENEMY_STATE es)
    {
        myState = es;
    }
}
