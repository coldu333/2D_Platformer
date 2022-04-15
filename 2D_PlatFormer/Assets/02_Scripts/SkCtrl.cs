using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkCtrl : MonoBehaviour
{
    float SkSpeed = 0; //공격 속도
    float SkLife = 0.0f; //총알 생명주기
    Vector3 m_DirTgVec; //방향벡터
    Vector3 a_StartPos = Vector3.zero; //시작 백터

    Transform playerTr;
    Transform skTr;

    MonCtrl refMon;

    // Start is called before the first frame update
    void Start()
    {
        SkSpeed = 5;
        playerTr = GameObject.Find("Player").GetComponent<Transform>();
        skTr = this.GetComponent<Transform>();
        SkLife = 1f;

        float a_Key;
        a_Key = playerTr.localScale.x;
        m_DirTgVec = new Vector3(a_Key, 0, 0);
        skTr.localScale = new Vector3(a_Key*skTr.localScale.x, skTr.localScale.y, skTr.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += m_DirTgVec * Time.deltaTime * SkSpeed;
        SkLife -= Time.deltaTime;

        if (SkLife < 0)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.name.Contains( "Monster" ))
        {
            refMon = coll.gameObject.GetComponent<MonCtrl>();
            GameMgr.Inst.SpawnItem( refMon.transform.position );
            Destroy( this.gameObject );
            GameMgr.Inst.AddScore();
        }
        if (coll.gameObject.name.Contains("Ground"))
            Destroy(this.gameObject);
    }
}

