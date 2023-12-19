using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //������ ������ ����
    public GameObject[] prefabs;

    //Ǯ ��� ����Ʈ
    List<GameObject>[] pools;

    void Awake()
    {
        //ũ�Ⱚ�� �������� ũ�Ⱚ���� ����
        pools = new List<GameObject>[prefabs.Length];

        //������ ũ�Ⱚ��ŭ �迭 �ʱ�ȭ
        for(int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ������ Ǯ�� ��Ȱ��ȭ�� ���ӿ�����Ʈ ��ġ
        // �߰��ϸ� select ������ �Ҵ�
        foreach(GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // �߰����� ���ϸ� ���Ӱ� �����ؼ� select ������ �Ҵ�
        if(!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
