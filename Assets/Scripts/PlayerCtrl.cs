using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tip 플랫폼의 높이를 점프높이의 80%정도로 할 것.
//기본기능
//달리기, 점프 (완료)
//데미지, 보상, UI
//게임 컨셉 부여하기
//맵 디자인, 게임오버 화면

public class PlayerCtrl : MonoBehaviour
{
    //캐릭터 이동 변수
    Transform tr;
    float h = 0.0f; //수평(x축)
    float v = 0.0f; //수직(y축)
    Vector3 moveDir = Vector3.zero;
    float moveSpeed = 0.0f;

    Rigidbody2D rigid2D;
    float JumpForce = 0.0f;
    
    Animator animator;
    //캐릭터 이동 변수

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;   
        QualitySettings.vSyncCount = 0;

        moveSpeed = 5.0f;
        JumpForce = 85;
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

        int key = 0;
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

            moveDir = new Vector3(key, 1.5f, 0);
            if (1.0f < moveDir.magnitude)
                moveDir.Normalize();
            transform.position += moveDir * JumpForce * Time.deltaTime;
        }
        //캐릭터 점프
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.name.Contains("Coin") == true)
        {
            GameMgr.Inst.AddGold();
            Destroy(coll.gameObject);
        }
    }
}
