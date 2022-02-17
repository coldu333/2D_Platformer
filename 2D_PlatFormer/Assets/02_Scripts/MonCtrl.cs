using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 몬스터 위에 못 올라가게 해야겠다.
public enum MonState
{
    Idle,
    Trace,
    Attack,
    Die
}
public class MonCtrl : MonoBehaviour
{
    MonState monState = MonState.Idle;

    //몬스터 이동
    Transform monTr;
    Transform playerTr;

    float moveSpeed = 0.0f;
    int a_key = 1;

    Vector3 Dist;

    bool isColl = false;
    float collTimer = 0.0f;

    //몬스터 지형 인식 변수
    Vector3 frontVec;
    RaycastHit2D raycasthit;
    RaycastHit2D isPlayerRay;

    //몬스터 애니메이션
    Animator animator;

    PlayerCtrl refPlayer;

    // Start is called before the first frame update
    void Start()
    {
        monTr = this.GetComponent<Transform>();
        playerTr = GameObject.Find("Player").GetComponent<Transform>();
        refPlayer = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        this.animator = GetComponent<Animator>();

        moveSpeed = 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        MonUpdate();
        if(monState != MonState.Die)
            this.animator.SetBool("isMonRun", true);

        if (isColl == true)
        {
            collTimer += Time.deltaTime;
            if (collTimer >= 0.3f)
            {
                isColl = false;
                collTimer = 0.0f;
            }
        }
    }

    void MonUpdate()
    {
        monTr.localScale = new Vector3(-1 * a_key, 1, 1);
        Dist = playerTr.position - monTr.position;

        //벼랑에서 방향을 바꿈
        frontVec = new Vector3(a_key*0.5f + monTr.position.x, monTr.position.y, 0);
        raycasthit = Physics2D.Raycast(frontVec, Vector3.down, 1);
        if (raycasthit.collider == null)
        {
            a_key = -a_key;
            monState = MonState.Idle;
        }

        if (monState == MonState.Idle) // 좌우로 왔다갔다 하다가 멈추고 반복
        {
            monTr.Translate(new Vector3(a_key, 0, 0) * moveSpeed * Time.deltaTime);
            
            if (monTr.position.x >= 3)
                a_key = -1;
            else if (monTr.position.x <= -3)
                a_key = 1;

            if (monTr.position.y == playerTr.position.y && Dist.magnitude < 3.0f)
                monState = MonState.Trace;

        }
        else if(monState == MonState.Trace) // 플레이어가 추적거리 안으로 들어오면 추격
        {
            a_key = -refPlayer.key;
            if (isColl == true)
            {
                monTr.position = new Vector3(monTr.position.x, monTr.position.y, 0);//플레이어와 충돌하면 잠시 멈춤
            }
            else
                monTr.Translate(new Vector3(a_key, 0, 0) * (moveSpeed + 1.8f) * Time.deltaTime);// 플레이어를 발견하면 돌진

            if(Dist.magnitude >= 4.0f)
                monState = MonState.Idle;
        }
        //else if(monState == MonState.Attack) // 플레이어를 공격
        //{

        //}
        else if(monState == MonState.Die) // 몬스터 사망
        {
            this.animator.SetBool("isMonRun", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name.Contains("Player"))
            isColl = true;
    }
}
