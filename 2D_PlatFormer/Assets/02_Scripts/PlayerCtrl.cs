using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//animation.crossfade 사용해서 매끄럽게 처리하기

//몬스터가 추적 상태일 때 플레이어의 방향에 따라 진행방향을 바꾸는 버그를 없애야함(완료)
//UI 스크립트 연결(완료)
//버그: 플레이어의 바라보는 방향과 몬스터의 바라보느 방향에 따라 튕겨져 나가는 것이 되지 않음.(완료)
//죽는 거 구현(완료)
//플레이어의 HP 구현(완료)

//몬스터 죽고 나서 아이템드롭 구현 -> monState가 Die가 될 때 gameMgr쪽에서 프리팹을 생성하는 것이 어떨까?


//플레이어의 애니메이션 자연스럽게 구현
//몬스터 스폰

//데미지(쉐이더도 바꿔줄 거임), 보상, UI, 사운드
//게임 컨셉 부여하기
//맵 디자인
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
    MonCtrl refMon = null;
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

        //캐릭터 충돌데미지
        if(isMonColl == true)
        {
            collTimer += Time.deltaTime;
            h = 0;
            v = 0;

            moveDir = new Vector3(refMon.a_key, 0, 0);
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
            refMon = coll.gameObject.GetComponent<MonCtrl>();
            GameMgr.Inst.DeHp();
            isMonColl = true;
        }
    }

}
