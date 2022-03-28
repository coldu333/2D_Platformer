using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라 위치 예외처리 할 것(현재는 x축만 제한함)


public class FollowCam : MonoBehaviour
{
    Transform playerTr = null;
    Transform CamTr = null;

    Vector3 NewPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        playerTr = GameObject.Find("Player").GetComponent<Transform>();
        CamTr = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //NewPos = CamTr.position;
        NewPos = new Vector3(playerTr.position.x, playerTr.position.y + 1.5f, -10);

        if (NewPos.x < -0.45f)
            NewPos.x = -0.45f;

        //if (NewPos.x > 18.45f)
        //    NewPos.x = 18.45f;

        if (NewPos.y > 3.45f)
            NewPos.y = 3.45f;

        CamTr.position = NewPos;
    }
}
