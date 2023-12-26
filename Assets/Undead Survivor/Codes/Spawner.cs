using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //���� ����Ʈ�� �������� �ϱ� ���� �ڽ� ������Ʈ�� ������ ����. �����ϱ� ���� �迭 ����
    public Transform[] spawnPoint;

    //�ֳʹ̸� �������� �����ϱ� ���� �迭 ����
    public SpawnDate[] spawnDate;

    //�ֳʹ��� �������� �����ϱ� ���� Ÿ�̸�
    float timer;

    //������ �ܰ� ������ ���� ����
    int level;

    void Awake()
    {
        //�̸� ���ص� �������� �ڽ� ������Ʈ�� �����ϱ� ���� GetComponentsInChildren �Լ�
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;

        //Mathf.Min = �Ķ���͸� ���� �� ���� ���� ��ȯ�ϴ� �Լ�
        //���⿡�� ���� ������ �ð�(��) / 10��, �ֳʹ��� ����(2��) - 1 �� ���� ���� ��ȯ�� ������ ������ ����ϴ°��� ����
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / 10f), spawnDate.Length -1);

        //�ֳʹ��� ������ level�� Ȯ�� ��, level�� �´� ������ ������ ����Ÿ���� �Ǹ� �ֳʹ̸� ����
        if(timer >  spawnDate[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
      
    }

    void Spawn()
    {
        //������� �������� 1���̹Ƿ� 1��° �������� ���
        GameObject enemy = GameManager.Instance.pool.Get(0);

        //������ �ֳʹ��� ��ġ�� �������� ���� ����Ʈ�� �����ϰ� ����
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

        //���� ������ �´� �ֳʹ��� ������ ���� �ֳʹ� �ʱ�ȭ
        enemy.GetComponent<Enemy>().Init(spawnDate[level]);
    }
}

//������ Ŭ������ �ֳʹ��� ������ ����
[System.Serializable]
public class SpawnDate
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
