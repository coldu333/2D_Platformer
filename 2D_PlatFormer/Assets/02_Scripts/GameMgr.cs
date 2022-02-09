using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI와 스크립트 연결
//데미지
//몬스터
//타일맵
//게임오버씬
//게임 컨셉 디벨롭
//로비 및 메뉴씬 구현

public class GameMgr : MonoBehaviour
{
    [HideInInspector] 
    public int Gold = 0;

    public static GameMgr Inst;

    void Awake()
    {
        Inst = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void AddGold(int a_Gold = 10)
    {
        Gold += a_Gold;
        Debug.Log("MyGold" + Gold);
    }
}
