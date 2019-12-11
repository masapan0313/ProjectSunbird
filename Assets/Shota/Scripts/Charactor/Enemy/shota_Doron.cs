using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shota_Doron : MonoBehaviour {

    [SerializeField] shota_Enemy EnemyPropertie; // 敵の基本情報
    [SerializeField] List<shota_NodePoint> PatrolPoints; // 巡回ルート（手動で設定）
    [SerializeField] shota_HeadQuarters HQ;
    private shota_NodePoint nextTargetPoint = null; // 次の目的地
    private int currentPointNum = 0; // 今の巡回数

    [SerializeField] Light searchLight;

    private bool isMove = false;

    [SerializeField] float RotateSpeed;

    [SerializeField] float ToleranceDistance; // 許容できるガバの大きさ
    [SerializeField] float ToleranceAngle; // 許容できるガバ回転

    [SerializeField] List<float> Speed;

    [SerializeField] Color A_Color, P_Color;

    private shota_Enemy.ENEMY_STATE myState;

    private int ActStep = 0; // 今の行動指針に対してどの行動をとっているか

    private bool isAppearPlayer;

    private Vector3 srvec;

    [SerializeField, Range(0, 10)]
    private float StayTimeLimit;

    private float stayTime;

    private shota_NodePoint LastVisitNode;

    [SerializeField]
    AudioClip[] selist;

    private void Start()
    {
        Speed.AddRange(EnemyPropertie.GetWalkSpeed());
        searchLight.color = P_Color;
        srvec = Vector3.zero;
    }

    private void Update()
    {

        if (isMove)
        { 
            searchLight.GetComponent<Light>().enabled = false;
        }
        else
        {
            searchLight.GetComponent<Light>().enabled = true;
            if (isAppearPlayer && myState != shota_Enemy.ENEMY_STATE.ALARM)
            {
                SetState(shota_Enemy.ENEMY_STATE.ALARM);
            }
        }

        switch (myState)
        {
            case shota_Enemy.ENEMY_STATE.PATROL:
                Patrol();
                break;
            case shota_Enemy.ENEMY_STATE.ALARM:
                Alarm();
                break;
            default:
                break;
        }
    }
    
    private void Patrol()
    {
        switch (ActStep)
        {
            case 0:
                isMove = true;
                Move(PatrolPoints);
                // 目標地点まである程度近づいたら目標地点を更新する
                if (Vector3.Distance(
                    new Vector3(
                        transform.position.x,
                        0, 
                        transform.position.z), 
                    new Vector3(
                        nextTargetPoint.gameObject.transform.position.x,
                        0, 
                        nextTargetPoint.gameObject.transform.position.z)) 
                    < ToleranceDistance)
                {
                    ActStep++;
                    isMove = false;
                }
                break;
            case 1:
                SwitchingTargetNode(PatrolPoints, true);
                Debug.Log("きりかえ");
                break;
            case 2:
                LookRotate();
                break;
            case 3:
                if(stayTime >= StayTimeLimit)
                {
                    stayTime = 0;
                    ActStep = 0;
                }
                break;
            default:
                break;
        }


        // 0は移動中
        if(ActStep != 0)
        {
            if (stayTime < StayTimeLimit)
            {
                stayTime += Time.deltaTime;
            }
        }

    }

    private void Alarm()
    {
        switch (ActStep)
        {
            case 0:
                searchLight.color = A_Color;
                HQ.Alart(LastVisitNode);
                ActStep++;
                break;
            case 1:
                // なんか警報鳴らしてるような動きするんじゃないです？
                // アニメーションの再生が終わったらif()
                ActStep++;
                break;
            case 2:
                // プレイヤーがとどまっているなら
                if (isAppearPlayer)
                {
                    ActStep = 1; // もう一度アニメーション
                }
                else
                {
                    searchLight.color = P_Color;
                    SetState(shota_Enemy.ENEMY_STATE.PATROL); // パトロールに戻る
                    // この辺StayTime無視して即行動するけど
                    // 見失った上でその場にとどまるのも変なのでそのままで
                }
                break;
            default:
                break;
        }
    }

    // 行先が線
    private void Move(List<shota_NodePoint> path)
    {
        // 現在の位置から目的の座標まで移動
        if (nextTargetPoint == null)
        {
            nextTargetPoint = path[0];
        }

        transform.position
                    = new Vector3(Vector3.MoveTowards(transform.position,
                    nextTargetPoint.GetNodePos(),
                    Speed[(int)myState] * Time.deltaTime).x,
                    transform.position.y,
                    Vector3.MoveTowards(transform.position,
                    nextTargetPoint.GetNodePos(),
                    Speed[(int)myState] * Time.deltaTime).z);

        // 行き先を見る
        transform.LookAt(new Vector3(nextTargetPoint.GetNodePos().x,transform.position.y,nextTargetPoint.GetNodePos().z));

        for (int i = 0; i < path.Count - 1; i++)
        {
            if (i < path.Count - 1)
            {
                Debug.DrawRay(path[i].GetNodePos(), path[i + 1].GetNodePos() - path[i].GetNodePos());
            }
            if (i == path.Count - 2)
            {
                Debug.DrawRay(path[path.Count - 1].GetNodePos(), path[0].GetNodePos() - path[path.Count - 1].GetNodePos());
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
            ActStep=2;
        }
        else
        {
            // 着いた
            if (looping)
            {
                // 巡回するのでスタート地点へ
                currentPointNum = 0;
                nextTargetPoint = path[currentPointNum];
                ActStep = 2;
            }
        }

        Debug.Log(nextTargetPoint.gameObject.name);
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

        if (Mathf.Abs( Mathf.DeltaAngle(transform.rotation.eulerAngles.y, srvec.y)) < ToleranceAngle)
        {
            ActStep++;
            srvec = Vector3.zero;
        }
    }

    private void SetState(shota_Enemy.ENEMY_STATE es)
    {
        myState = es;
        ActStep = 0;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isAppearPlayer = true;
        }
        if(other.gameObject.tag == "Node")
        {
            LastVisitNode = other.gameObject.GetComponent<shota_NodePoint>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isAppearPlayer = false;
        }
    }
}
