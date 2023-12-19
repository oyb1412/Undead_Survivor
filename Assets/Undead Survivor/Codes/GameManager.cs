using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("# Game Control")]
    //현재 게임의 시간
    public float gameTime;

    //게임의 최대시간
    public float maxGameTime = 2 * 10f;

    [Header("# Player Info")]

    //레벨
    public int level;

    //처치 횟수
    public int kill;

    //경험치
    public int exp;

    //레벨업을 위해 필요한 경험치
    public int[] nextExp = {3, 5, 10, 30, 60, 100, 150, 210, 280, 360, 450, 600};

    public int health;
    public int maxHealth = 100;
    [Header("# Game Object")]
    //플레이어
    public Player player;

    //풀매니저
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
