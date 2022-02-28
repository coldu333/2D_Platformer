using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonGenerator : MonoBehaviour
{
    public Transform[] points;
    public GameObject monsterPrefab;
    public List<GameObject> monsterPool = new List<GameObject>();

    public float createTime = 5.0f;
    public int maxMonster = 3; //초기 몬스터 수

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<maxMonster; i++) //몬스터 생성 후 오브젝트 풀에 등록
        {
            GameObject monster = (GameObject)Instantiate(monsterPrefab);
            monster.name = "Monster_" + i.ToString();
            monster.SetActive(false);
            monsterPool.Add(monster);
        }

        points = GameObject.Find("SpawnGroup").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
            StartCoroutine(this.CreateMonster());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator CreateMonster()
    {
        while(GameMgr.Inst.gameState == GameState.GameIng)
        {
            yield return new WaitForSeconds(createTime);


            if (GameMgr.Inst.gameState == GameState.GameEnd)
                yield break;

            foreach(GameObject monster in monsterPool)
            {
                if(!monster.activeSelf)
                {
                    int idx = Random.Range(1, points.Length);

                    monster.transform.position = points[idx].position;
                    monster.SetActive(true);
                    break;
                }
            }
        }
    }
}
