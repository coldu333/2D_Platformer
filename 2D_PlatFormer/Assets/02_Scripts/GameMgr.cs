using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
