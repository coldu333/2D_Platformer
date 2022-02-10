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

    Transform monTr;
    Transform playerTr;

    float moveSpeed = 0.0f;
    int a_key = 1;

    float Dist = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        monTr = this.GetComponent<Transform>();
        playerTr = GameObject.Find("Player").GetComponent<Transform>();

        moveSpeed = 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        MonUpdate();
    }

    void MonUpdate()
    {
        if(monState == MonState.Idle) // 좌우로 왔다갔다 하다가 멈추고 반복
        {
            monTr.Translate(new Vector3(a_key, 0, 0) * moveSpeed * Time.deltaTime);
            monTr.localScale = new Vector3(-1 * a_key, 1, 1);

            if (monTr.position.x >= 3)
                a_key = -1;
            else if (monTr.position.x <= -3)
                a_key = 1;

            Dist = Vector3.Distance(monTr.position, playerTr.position);
            if (Dist < 2)
                monState = MonState.Trace;
        }
        else if(monState == MonState.Trace) // 플레이어가 추적거리 안으로 들어오면 추격
        {
            if (Dist > 2)
                monState = MonState.Idle;
        }
        else if(monState == MonState.Attack) // 플레이어를 공격
        {

        }
        else if(monState == MonState.Die) // 몬스터 사망
        {

        }
    }
}
