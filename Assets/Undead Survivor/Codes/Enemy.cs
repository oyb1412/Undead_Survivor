using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //�ֳʹ��� �ӵ�
    public float speed;

    //�ֳʹ��� ü��
    public float health;

    //�ֳʹ��� �ִ�ü��
    public float maxHealth;

    //�ֳʹ��� ��������
    bool isLive;

    //��ȯ�� �ֳʹ��� ������ ���� �ٸ� �ִϸ��̼��� ����ϱ� ���� ����
    public RuntimeAnimatorController[] animCon;

    //Ÿ��(�÷��̾�)
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
        //�ֳʹ̰� false���¸� �Լ� ����
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        //�÷��̾�� �ֳʹ� ������ ����
        Vector2 dirVec = target_Player.position - rigid.position;

        //�ֳʹ��� ���� ���� * �ӵ��� ����ȭ�� ���ͷ� ����
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        //�ֳʹ��� ��ġ + ������� * �ӵ��� �ֳʹ̸� �̵�
        rigid.MovePosition(rigid.position + nextVec);

        //�ֳʹ̰� �÷��̾�� �з����� �ʰ� ���ν�Ƽ ���η� ����
        rigid.velocity = Vector2.zero;
    }

    //��� ������Ʈ �Լ��� ȣ��� �� �������� ȣ��
    private void LateUpdate()
    {
        //�ֳʹ̰� false���¸� �Լ� ����
        if (!isLive)
            return;

        //�ֳʹ��� ������ ���� ���� ����
        spriter.flipX = target_Player.position.x < rigid.position.x;
    }

    //������Ʈ�� ���ǵ��� �����Ǹ� ȣ��
    private void OnEnable()
    {
        //�÷��̾� ���� ����
        target_Player = GameManager.Instance.player.GetComponent<Rigidbody2D>();

        //�ֳʹ� Ȱ��ȭ
        isLive = true;

        //�ֳʹ� ü�� ����
        health = maxHealth;

        isLive = true;
        col.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
    }

    public void Init(SpawnDate data)
    {
        //������ �ֳʹ��� ������ ���� �ִϸ��̼� ���
        anim.runtimeAnimatorController = animCon[data.spriteType];

        //������ �ֳʹ��� ������ ���� �ӵ�,ü��,�ִ�ü�� ����
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    //�ֳʹ� �浹 �Լ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�ҷ��� �浹���� ���� ��Ȳ�̸� �Լ� ����
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        //�ֳʹ��� ü���� �ҷ��� ��������ŭ ����
        health -= collision.GetComponent<Bullet>().damage;

        //�˹� �ڷ�ƾ ����
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

    //�ڷ�ƾ �Լ�
    IEnumerator KnockBack()
    {
        //1������ ����
        //yield return null;
        //2�� ����
        //yield return new WaitForSeconds(2f);

        //���� �ϳ��� ���� ������ ���� ���
        yield return wait;

        //�÷��̾� ������
        Vector3 playerPos = GameManager.Instance.player.transform.position;

        //�ֳʹ��� �ݴ� ���� ����
        Vector3 dirVec = transform.position - playerPos;

        //�����忡 ���� ����(����ȭ ��) ForceMode2D.Impulse�� Ÿ��,����ó�� �������� ���� ��Ÿ���� ���
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    //����Ƽ �ִϸ��̼� �̺�Ʈ���� ����
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
