using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shota_Police : MonoBehaviour {

    [SerializeField]
    private shota_Astar _Astar; // A*アルゴリズム利用
    private List<shota_NodePoint> AStarPoints = new List<shota_NodePoint>(); // 目的地までの最短ルート

    [SerializeField]
    shota_Enemy EnemyPropertie; // 敵の基本情報

    [SerializeField]
    List<shota_NodePoint> PatrolPoints; // パトロール中の巡回ルート（手動で設定）

    private shota_NodePoint nextTargetPoint = null; // 次の対象ノード
    private int currentPointNum = 0; // 巡回ルートのインデックス番号

    [SerializeField]
    float ToleranceDistance; // 対象ノードにどれだけ近づけばいいかの長さ

    List<float> Speed = new List<float>(); // 自身の移動速度　EnemyPropertieから数値を確保する
    shota_Enemy.ENEMY_STATE myState = shota_Enemy.ENEMY_STATE.PATROL;

    private shota_NodePoint lastVisitNode = null; // 最後に触れたノード

    private bool isSite = false; // プレイヤーを目視しているか
    private float lostCount = 0; // プレイヤーが視界から外れている時間をカウント
    [SerializeField, Range(0,10)]
    private float interrupt_Time; // lostCountがこの値を超えると追跡を中止して、巡回に戻る
    

    RaycastHit hit;
    [SerializeField,Range(0.0f, 100.0f)]
    private float BoxCast_x, BoxCast_y,BoxCast_z; // 飛ばすBoxRayの大きさ
   // [SerializeField, Range(0.0f, 100.0f)] private float RayRange; // どの程度飛ばすか

    private Vector3 BoxCast_scale;

    private bool isMove = false; // 移動中か

    private int ActStep;
    private Vector3 srvec;
    [SerializeField]
    private float RotateSpeed = 10;
    [SerializeField]
    private float ToleranceAngle = 0.1f;

    private float stayTime;
    [SerializeField] float StayTimeLimit;

    [SerializeField]
    shota_Police_Animation_Controller spac;

    [SerializeField]
    AudioClip[] selist;

    [SerializeField]
    Scenemanager sm;

    private void Start()
    {
        Speed.AddRange(EnemyPropertie.GetWalkSpeed());
        BoxCast_scale = new Vector3(BoxCast_x, BoxCast_y, BoxCast_z);
    }

    private void FixedUpdate()
    {
        if (isMove)
        {
            if(!GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().Play();
        }
        if(!isMove)
        {
            if (GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().Stop();
        }
        switch (myState)
        {
            case shota_Enemy.ENEMY_STATE.PATROL:
                Patrol();
                break;
            case shota_Enemy.ENEMY_STATE.NMA_SCRPT:
                break;
            case shota_Enemy.ENEMY_STATE.ALARM:
                Alart();
                break;
            case shota_Enemy.ENEMY_STATE.RUN:
                //Run();
                // 見つかったらゲームオーバー
                // 見つけた時点でなんかプレイヤーに向けて走るアニメだけ流して
                // 残りはプレイヤーキャラに注目させて〆でいいんでない？
                break;
            case shota_Enemy.ENEMY_STATE.BACK:
                Back();
                break;
            default:
                break;
        }
    }

    // 行先が線
    private void Move(List<shota_NodePoint> path)
    {
        // 現在の位置から目的の座標まで移動
        if(nextTargetPoint == null)
        {
            nextTargetPoint = path[0];
        }
        
        transform.position 
            = Vector3.MoveTowards(transform.position,
            nextTargetPoint.GetNodePos(), 
            Speed[(int)myState] * Time.deltaTime);

        // 行き先を見る
        transform.LookAt(nextTargetPoint.GetNodePos());

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

    private void SwitchingTargetNode(List<shota_NodePoint> path, bool looping)
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
                    shota_NodePoint nd = path[path.Count - 1]; // 終点を保持
                    currentPointNum = PatrolPoints.FindIndex(nodepos => nodepos.GetNodePos() == nd.GetNodePos()); // 終点が巡回点の何番目に位置するのかをみて現在の巡回カウントを更新
                    Debug.Log(currentPointNum); // 実際に正しく動作しているかの確認
                    SetState(shota_Enemy.ENEMY_STATE.PATROL); // 状態を巡回に更新
                }
            }
        }
    }
    private void LookRotate()
    {
        Vector3 non_y_TargetAngle = nextTargetPoint.GetNodePos() - transform.position;
        non_y_TargetAngle.y = 0.0f;

        Quaternion lookrota = Quaternion.LookRotation(non_y_TargetAngle);
        lookrota.x = 0;
        lookrota.z = 0;
        if (srvec == Vector3.zero)
        {
            // ここで目的地を向き終えた状態の角度を求め、srvecに格納する
            var aim = non_y_TargetAngle = nextTargetPoint.GetNodePos() - transform.position;
            aim.y = 0.0f;
            var look = Quaternion.LookRotation(aim);
            srvec = look.eulerAngles;
        }
        var step = RotateSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Lerp(transform.rotation, lookrota, step);


        // 回転速度が遅すぎるとうまくいかない。何故？
        if (Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.y, srvec.y)) < ToleranceAngle)
        {
            ActStep++;
            srvec = Vector3.zero;
        }
    }
    
    private void Patrol()
    {

        switch (ActStep)
        {
            case 0:
                isMove = true;
                spac.SetisMove(isMove);
                spac.SetSpeed(2);
                Move(PatrolPoints);
                // 目標地点まである程度近づいたら目標地点を更新する
                if (Vector3.Distance(transform.position, nextTargetPoint.gameObject.transform.position) < ToleranceDistance)
                {
                    ActStep++;
                    isMove = false;
                    spac.SetisMove(isMove);
                }
                break;
            case 1:
                SwitchingTargetNode(PatrolPoints, true);
                Debug.Log("きりかえ");
                ActStep++;
                break;
            case 2:
                spac.SetSpeed(1);
                LookRotate();
                break;
            case 3:
                if (stayTime >= StayTimeLimit)
                {
                    stayTime = 0;
                    ActStep = 0;
                }
                break;
            default:
                break;
        }


        // 0は移動中
        if (ActStep != 0)
        {
            if (stayTime < StayTimeLimit)
            {
                stayTime += Time.deltaTime;
            }
        }

    }
    private void Alart()
    {
        spac.SetSpeed(4);
        Move(AStarPoints);
        // 目標地点まである程度近づいたら目標地点を更新する
        if (Vector3.Distance(transform.position, nextTargetPoint.gameObject.transform.position) < ToleranceDistance)
        {
            SwitchingTargetNode(AStarPoints, false);
        }
    }
    private void Back()
    {
        spac.SetSpeed(2);
        Move(AStarPoints);
        // 目標地点まである程度近づいたら目標地点を更新する
        if (Mathf.Abs(Vector3.Distance(transform.position, nextTargetPoint.gameObject.transform.position)) < ToleranceDistance)
        {
            SwitchingTargetNode(AStarPoints, false);
        }
    }
    private void GameOver()
    {
        // HQにAlart飛ばしてもいいか
        // ここ使った時点で終わってるから、確保アニメーションの再生と...と思ったけど
        // 新たに一個上の概念作ってクリアオーバーメインでステップさせてもいいかも？
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
    }

    private void SetState(shota_Enemy.ENEMY_STATE es)
    {
        myState = es;
        ActStep = 0;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Node")
        {
            lastVisitNode = other.gameObject.GetComponent<shota_NodePoint>();
        }
        if(other.gameObject.tag == "Player")
        {
            sm.Over(); // 即死で

            isSite = true;
            if (myState != shota_Enemy.ENEMY_STATE.RUN)
            {
                lostCount = 0;
                SetState(shota_Enemy.ENEMY_STATE.RUN);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isSite = false;
        }
    }

}