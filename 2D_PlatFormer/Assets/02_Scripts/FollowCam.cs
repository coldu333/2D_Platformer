using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 위치 제한 설정을 변경할 것(완)
//카메라 위치 예외처리 할 것

public class FollowCam : MonoBehaviour
{
    Transform playerTr = null;
    Transform CamTr = null;

    // Start is called before the first frame update
    void Start()
    {
        playerTr = GameObject.Find("Player").GetComponent<Transform>();
        CamTr = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        CamTr.position = new Vector3(playerTr.position.x, CamTr.position.y, CamTr.position.z);
    }
}
