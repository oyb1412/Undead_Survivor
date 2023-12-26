using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //스폰 포인트를 랜덤으로 하기 위해 자식 오브젝트로 여러개 설정. 저장하기 위한 배열 변수
    public Transform[] spawnPoint;

    //애너미를 종류별로 저장하기 위한 배열 변수
    public SpawnDate[] spawnDate;

    //애너미의 스폰율을 조정하기 위한 타이머
    float timer;

    //게임의 단계 조정을 위한 변수
    int level;

    void Awake()
    {
        //미리 정해둔 여러개의 자식 오브젝트를 대입하기 위한 GetComponentsInChildren 함수
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;

        //Mathf.Min = 파라미터를 비교해 더 작은 값을 반환하는 함수
        //여기에선 현재 게임의 시간(초) / 10과, 애너미의 종류(2종) - 1 중 작은 값을 반환해 레벨이 무한정 상승하는것을 방지
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / 10f), spawnDate.Length -1);

        //애너미의 종류를 level로 확인 후, level에 맞는 종류에 지정된 스폰타임이 되면 애너미를 스폰
        if(timer >  spawnDate[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
      
    }

    void Spawn()
    {
        //사용중인 프리펩이 1개이므로 1번째 프리펩을 사용
        GameObject enemy = GameManager.Instance.pool.Get(0);

        //스폰된 애너미의 위치는 여러개의 스폰 포인트중 랜덤하게 지정
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

        //현재 레벨에 맞는 애너미의 정보를 토대로 애너미 초기화
        enemy.GetComponent<Enemy>().Init(spawnDate[level]);
    }
}

//별도의 클래스로 애너미의 정보를 저장
[System.Serializable]
public class SpawnDate
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
