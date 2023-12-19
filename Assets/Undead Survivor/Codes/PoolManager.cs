using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //프리펩 보관용 변수
    public GameObject[] prefabs;

    //풀 담당 리스트
    List<GameObject>[] pools;

    void Awake()
    {
        //크기값을 프리펩의 크기값으로 지정
        pools = new List<GameObject>[prefabs.Length];

        //지정된 크기값만큼 배열 초기화
        for(int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 선택한 풀의 비활성화된 게임오브젝트 서치
        // 발견하면 select 변수에 할당
        foreach(GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 발견하지 못하면 새롭게 생성해서 select 변수에 할당
        if(!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
