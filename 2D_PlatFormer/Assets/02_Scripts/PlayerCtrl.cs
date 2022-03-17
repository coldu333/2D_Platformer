using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//버그 : 몬스터가 반대 쪽으로 달아나는 현상/ 추적상태와 벼랑체크 코드가 서로 간섭해서 일어나는 듯

//2단 뛰기 , 슬라이드 구현하기
//게임 컨셉 부여하기
//맵 디자인
//공격(중간 보스부터 구현해줘야겠다.)

public class PlayerCtrl : MonoBehaviour
{
    //캐릭터 이동 변수
    Rigidbody2D rigid2D;

    public float key = 0; //캐릭터가 바라보는 방향

    Transform tr;
    float h = 0.0f; //수평(x축)
    float v = 0.0f; //수직(y축)
    Vector3 moveDir = Vector3.zero;
    float moveSpeed = 0.0f;

    //캐릭터 점프
    float JumpForce = 0.0f;
    int JumpCount = 0;

    //슬라이드 변수
    float height = 0;
    bool isSlide = false;

    //캐릭터 애니메이션
    Animator animator;
    RaycastHit2D CheckJumpRay;
    
    //캐릭터 스킬 변수
    public GameObject SkPrefab;

    //캐릭터 데미지 변수
    MonCtrl refMon = null;
    bool isMonColl = false;
    float collTimer = 0.0f;

    bool isDamage = false;
    float dmgTimer = 0.0f;

    int playerLayer, monLayer;

    //캐릭터 음향
    public AudioClip JumpSfx = null;
    public AudioClip GetSfx = null;
    public AudioClip FireSfx = null;
    public AudioClip CollSfx = null;
    AudioSource sfx = null;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;   
        QualitySettings.vSyncCount = 0;

        this.rigid2D = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        this.animator = GetComponent<Animator>();
        this.sfx = GetComponent<AudioSource>();

        key = 1;
        height = 1;

        moveSpeed = 3.5f;
        //JumpForce = 75;
        JumpForce = 290;

        playerLayer = LayerMask.NameToLayer("PLAYER");
        monLayer = LayerMask.NameToLayer("MONSTER");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();

        //캐릭터 스킬
        if(Input.GetKeyDown(KeyCode.F) && isMonColl == false)
        {
            sfx.PlayOneShot(FireSfx, 0.2f);
            GameObject a_SkObj = (GameObject)Instantiate(SkPrefab);
            a_SkObj.transform.position = new Vector3(tr.position.x + key, tr.position.y, tr.position.z);
        }

        MonColl();
    }

    void PlayerMove()
    {
        transform.localScale = new Vector3(key, height, 1);//좌우반전

        //캐릭터 이동
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) key = 1;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) key = -1;

        //캐릭터 슬라이드
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            isSlide = true;
            height = 0.8f;
        }
        else
        {
            isSlide = false;
            height = 1;
        }

        if (h != 0)
        {
            moveDir = new Vector3(h, 0, 0);
            if (1.0f < moveDir.magnitude)
                moveDir.Normalize();
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        //화면 밖으로 이동 제한
        if (tr.position.x <= -8.5f)
            tr.position = new Vector3(-8.5f, tr.position.y, 0);
        //else if (tr.position.x >= 8.5f)
        //    tr.position = new Vector3(8.5f, tr.position.y, 0);

        //캐릭터 이동

        //캐릭터 애니메이션
        CheckJumpRay = Physics2D.Raycast(new Vector3(tr.position.x, tr.position.y - 1, 0), Vector3.down, 1);
        if (CheckJumpRay.collider != null && h != 0)
        {
            this.animator.SetBool("IsRun", true);
            this.animator.SetBool("IsIdle", false);
        }
        else
        {
            this.animator.SetBool("IsRun", false);
            this.animator.SetBool("IsIdle", true);
        }
        //캐릭터 애니메이션

        //캐릭터 점프
        if (Input.GetKeyDown(KeyCode.Space) && isSlide == false)//this.rigid2D.velocity.y == 0)
        {
            if (JumpCount > 1)
                return;

            if (JumpCount == 1)
                JumpForce = 200; //더블점프
            else
                JumpForce = 290;

            sfx.PlayOneShot(JumpSfx, 0.2f); //효과음
            this.animator.SetTrigger("JumpTrigger");
            this.rigid2D.AddForce(transform.up * JumpForce);
            JumpCount++;
        }
        //캐릭터 점프

    }

    void MonColl()
    {
        if (isMonColl == true)
        {
            collTimer += Time.deltaTime;
            h = 0;
            v = 0;

            moveDir = new Vector3(refMon.a_key, 0, 0);
            if (1.0f < moveDir.magnitude)
                moveDir.Normalize();
            transform.position += moveDir * (moveSpeed + 6.5f) * Time.deltaTime;

            isDamage = true;

            //충돌 타이머
            if (collTimer >= 0.3f)
            {
                isMonColl = false;
                collTimer = 0;
            }
        }

        //충돌 연출
        if (isDamage == true)
        {
            dmgTimer += Time.deltaTime;

            Physics2D.IgnoreLayerCollision(playerLayer, monLayer, true);

            //충돌 연출
            this.GetComponent<SpriteRenderer>().color = new Color32(255, 120, 120, 255);

            if ((int)(dmgTimer * 10) % 2 == 0)
                this.GetComponent<SpriteRenderer>().color = new Color32(255, 120, 120, 120);
            else if ((int)(dmgTimer * 10) % 2 == 1)
                this.GetComponent<SpriteRenderer>().color = new Color32(255, 120, 120, 200);

            if (dmgTimer > 1)
            {
                Physics2D.IgnoreLayerCollision(playerLayer, monLayer, false);
                this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                isDamage = false;
                dmgTimer = 0;
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name.Contains("Coin") == true)
        {
            sfx.PlayOneShot(GetSfx, 0.2f);
            GameMgr.Inst.AddGold();
            Destroy(coll.gameObject);
        }
        else if (coll.gameObject.name.Contains("Monster") == true)
        {
            if (isDamage == true)
                return;

            sfx.PlayOneShot(CollSfx, 0.1f);

            refMon = coll.gameObject.GetComponent<MonCtrl>();
            GameMgr.Inst.DeHp();
            isMonColl = true;
        }
        else if(coll.gameObject.name.Contains("Ground")==true)
        {
            JumpCount = 0;
        }    
    }

}
