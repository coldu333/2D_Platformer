using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformCtrl : MonoBehaviour
{
    SpriteRenderer spriteRenderer = null;
    BoxCollider2D coll = null;

    // Start is called before the first frame update
    void Start()
    {
        //콜리더의 넓이를 스프라이트의 넓이와 동일하게 설정
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        coll = this.GetComponent<BoxCollider2D>();

        coll.size = new Vector2(spriteRenderer.size.x, spriteRenderer.size.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
