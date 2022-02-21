using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//몬스터 죽고 나서 아이템드롭 구현 -> monState가 Die가 될 때 gameMgr쪽에서 프리팹을 생성하는 것이 어떨까?(완)
//점프를 전부 리지드바디로 바꾸는 건 어떨까?(완)

//버그 : 몬스터가 반대 쪽으로 달아나는 현상
//플레이어의 애니메이션 자연스럽게 구현
//버그 : 점프를 할 때 Run animation이 플레이됨.
//몬스터 스폰

//데미지 시 투명화도(완)
//데미지(쉐이더도 바꿔줄 거임) => 1초로 늘리자.
//사운드
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
        //JumpForce = 75;
        JumpForce = 290;
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
            
            //moveDir = new Vector3(key, 1, 0);
            //if (1.0f < moveDir.magnitude)
            //    moveDir.Normalize();
            //transform.position += moveDir * JumpForce * Time.deltaTime;
            this.rigid2D.AddForce(transform.up * JumpForce);
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

            //충돌 연출
            this.GetComponent<SpriteRenderer>().color = new Color32(255, 120, 120, 255);

            if ((int)(collTimer*10)%3 == 0)
                this.GetComponent<SpriteRenderer>().color = new Color32(255, 120, 120, 120);
            else if((int)(collTimer * 10) % 3 == 1)
                this.GetComponent<SpriteRenderer>().color = new Color32(255, 120, 120, 200);

            if (collTimer >= 0.3f)
            {
                this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                isMonColl = false;
                collTimer = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name.Contains("Coin") == true)
        {
            GameMgr.Inst.AddGold();
            Destroy(coll.gameObject);
        }
        else if (coll.gameObject.name.Contains("Monster") == true)
        {
            refMon = coll.gameObject.GetComponent<MonCtrl>();
            GameMgr.Inst.DeHp();
            isMonColl = true;
        }
    }

}
