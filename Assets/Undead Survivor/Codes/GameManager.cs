using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("# Game Control")]
    public bool isLive;
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

    public float health;
    public float maxHealth = 100;
    [Header("# Game Object")]
    //플레이어
    public Player player;

    //풀매니저
    public PoolManager pool;

    public LevelUp uiLevelUp;

    public Result uiResult;

    public GameObject enemyCleaner;
    private void Awake()
    {
        Instance = this;
    }

    public void GameStart()
    {
        health = maxHealth;
        isLive = true;
        //임시
        uiLevelUp.Select(0);
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }


    public void GameOver()
    {
        isLive = false;
        //StartCoroutine(GameOverRoutine());
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);

    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);

    }

    //IEnumerator GameOverRoutine()
    //{
    //    yield return new WaitForSeconds(2.0f);
    //}

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        if (!isLive)
            return;

        exp++;

        if(exp >= nextExp[Mathf.Min(level, nextExp.Length -1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive= true;
        Time.timeScale = 1;

    }
}
