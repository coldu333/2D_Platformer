using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkCtrl : MonoBehaviour
{
    float SkSpeed = 0; //공격 속도
    float SkLife = 0.0f; //총알 생명주기
    Vector3 m_DirTgVec; //방향벡터
    Vector3 a_StartPos = Vector3.zero; //시작 백터

    PlayerCtrl playerCtrl = null;

    // Start is called before the first frame update
    void Start()
    {
        SkSpeed = 5;
        m_DirTgVec = new Vector3(1, 0, 0); //임시코드 캐릭터가 바라보는 방향으로 스킬발사하게 변경할 것
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += m_DirTgVec * Time.deltaTime * SkSpeed;
        SkLife += Time.deltaTime;

        if (this.transform.position.x > 9.0f)
            Destroy(this.gameObject);
    }
}

