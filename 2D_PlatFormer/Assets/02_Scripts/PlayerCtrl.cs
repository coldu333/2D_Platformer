using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//animation.crossfade 사용해서 매끄럽게 처리하기

//몬스터 구현 중
//몬스터랑 플레이어가 충돌할 때 튕겨나가게 만들어야 겠다.(완료)
//몬스터랑 아이템 충돌되지 않게 설정(완료)
//움직일 때 애니메이션 연결해주기(완료)
//추적(완료)

//Ground 좌표를 얻어와서 몬스터가 지형 밖으로 빠져나가지 못 하게 하자.(완료)

//몬스터가 추적 상태일 때 플레이어의 방향에 따라 진행방향을 바꾸는 버그를 없애야함
//-> 추척 state가 되면 몬스터가 바라보는 방향으로 IDLE범위 만큼 raycast를 쏜다
//-> player가 충돌되면 그대도 충돌이 안 되면 -a_Key
//UI 스크립트 연결
//죽는 거 구현
//죽고 나서 보상(점수, 골드, 아이템드롭) 구현

//플레이어의 HP 구현
//플레이어의 애니메이션 자연스럽게 구현


//데미지(쉐이더도 바꿔줄 거임), 보상, UI, 사운드
//게임 컨셉 부여하기
//맵 디자인, 게임오버 화면
//공격(중간 보스부터 구현해줘야겠다.)

public class PlayerCtrl : MonoBehaviour
{
    //캐릭터 이동 변수
    public int key = 0; //캐릭터가 바라보는 방향

    Transform tr;
    float h = 0.0f; //수평(x축)
    float v = 0.0f; //수직(y축)
    Vector3 moveDir = Vector3.zero;
    float moveSpeed = 0.0f;

    Rigidbody2D rigid2D;
    float JumpForce = 0.0f;
    
    Animator animator;
    
    //캐릭터 스킬 변수
    public GameObject SkPrefab;

    //캐릭터 데미지 변수
    bool isMonColl = false;
    float collTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;   
        QualitySettings.vSyncCount = 0;

        moveSpeed = 3.5f;
        JumpForce = 75;
        tr = GetComponent<Transform>();
        this.animator = GetComponent<Animator>();
        
        this.rigid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //캐릭터 이동
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) key = 1;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) key = -1;

        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);//좌우반전
        }

        if (h != 0)
        {
            moveDir = new Vector3(h, 0, 0);
            if (1.0f < moveDir.magnitude)
                moveDir.Normalize();
            transform.position += moveDir * moveSpeed * Time.deltaTime;

            this.animator.SetBool("IsRun", true);
        }
        else
            this.animator.SetBool("IsRun", false);
        //캐릭터 이동

        //캐릭터 점프
        if(Input.GetKeyDown(KeyCode.Space) && this.rigid2D.velocity.y == 0)
        {
            this.animator.SetTrigger("JumpTrigger");

            moveDir = new Vector3(key, 1, 0);
            if (1.0f < moveDir.magnitude)
                moveDir.Normalize();
            transform.position += moveDir * JumpForce * Time.deltaTime;
        }
        //캐릭터 점프

        //캐릭터 스킬
        if(Input.GetKeyDown(KeyCode.F))
        {
            GameObject a_SkObj = (GameObject)Instantiate(SkPrefab);
            a_SkObj.transform.position = new Vector3(tr.position.x + key, tr.position.y, tr.position.z);
        }

        if(isMonColl == true)
        {
            collTimer += Time.deltaTime;
            h = 0;
            v = 0;

            moveDir = new Vector3(-key, 0, 0);
            if (1.0f < moveDir.magnitude)
                moveDir.Normalize();
            transform.position += moveDir * (moveSpeed + 6.5f) * Time.deltaTime;

            if (collTimer >= 0.3f)
            {
                isMonColl = false;
                collTimer = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.name.Contains("Coin") == true)
        {
            GameMgr.Inst.AddGold();
            Destroy(coll.gameObject);
        }
        else if(coll.gameObject.name.Contains("Monster")== true)
        {
            //HP를 감소시켜야 함
            isMonColl = true;
        }

    }
}
