using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //애너미의 속도
    public float speed;

    //애너미의 체력
    public float health;

    //애너미의 최대체력
    public float maxHealth;

    //애너미의 생존여부
    bool isLive;

    //소환한 애너미의 종류에 따라 다른 애니메이션을 재생하기 위한 변수
    public RuntimeAnimatorController[] animCon;

    //타겟(플레이어)
    Rigidbody2D target_Player;
    SpriteRenderer spriter;
    Rigidbody2D rigid;
    Collider2D col;
    Animator anim;
    WaitForFixedUpdate wait;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //애너미가 false상태면 함수 종료
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        //플레이어와 애너미 사이의 벡터
        Vector2 dirVec = target_Player.position - rigid.position;

        //애너미의 진행 방향 * 속도를 정규화해 벡터로 저장
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        //애너미의 위치 + 진행방향 * 속도로 애너미를 이동
        rigid.MovePosition(rigid.position + nextVec);

        //애너미가 플레이어에게 밀려나지 않게 벨로시티 제로로 설정
        rigid.velocity = Vector2.zero;
    }

    //모든 업데이트 함수가 호출된 후 마지막에 호출
    private void LateUpdate()
    {
        //애너미가 false상태면 함수 종료
        if (!isLive)
            return;

        //애너미의 방향을 토대로 반전 설정
        spriter.flipX = target_Player.position.x < rigid.position.x;
    }

    //오브젝트가 사용되도록 설정되면 호출
    private void OnEnable()
    {
        //플레이어 정보 저장
        target_Player = GameManager.Instance.player.GetComponent<Rigidbody2D>();

        //애너미 활성화
        isLive = true;

        //애너미 체력 설정
        health = maxHealth;

        isLive = true;
        col.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
    }

    public void Init(SpawnDate data)
    {
        //스폰된 애너미의 정보를 토대로 애니메이션 재생
        anim.runtimeAnimatorController = animCon[data.spriteType];

        //스폰된 애너미의 정보를 토대로 속도,체력,최대체력 설정
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    //애너미 충돌 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //불렛과 충돌하지 않은 상황이면 함수 종료
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        //애너미의 체력을 불렛의 데미지만큼 감소
        health -= collision.GetComponent<Bullet>().damage;

        //넉백 코루틴 실행
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            isLive = false;
            col.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.Instance.kill++;
            GameManager.Instance.GetExp();
        }
    }

    //코루틴 함수
    IEnumerator KnockBack()
    {
        //1프레임 쉬기
        //yield return null;
        //2초 쉬기
        //yield return new WaitForSeconds(2f);

        //다음 하나의 물리 프레임 까지 대기
        yield return wait;

        //플레이어 포지션
        Vector3 playerPos = GameManager.Instance.player.transform.position;

        //애너미의 반대 방향 벡터
        Vector3 dirVec = transform.position - playerPos;

        //리지드에 힘을 가함(정규화 후) ForceMode2D.Impulse는 타격,폭발처럼 순간적인 힘을 나타낼때 사용
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    //유니티 애니메이션 이벤트에서 실행
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
