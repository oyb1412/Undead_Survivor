using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("# Game Control")]
    //���� ������ �ð�
    public float gameTime;

    //������ �ִ�ð�
    public float maxGameTime = 2 * 10f;

    [Header("# Player Info")]

    //����
    public int level;

    //óġ Ƚ��
    public int kill;

    //����ġ
    public int exp;

    //�������� ���� �ʿ��� ����ġ
    public int[] nextExp = {3, 5, 10, 30, 60, 100, 150, 210, 280, 360, 450, 600};

    public int health;
    public int maxHealth = 100;
    [Header("# Game Object")]
    //�÷��̾�
    public Player player;

    //Ǯ�Ŵ���
    public PoolManager pool;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void GetExp()
    {
        exp++;

        if(exp >= nextExp[level] )
        {
            level++;
            exp = 0;
        }
    }
}
