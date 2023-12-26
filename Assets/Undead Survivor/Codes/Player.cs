using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    public Scanner scanner;

    //�÷��̾� ����
    public Vector2 inputVec;

    //�÷��̾� �ӵ�
    public float speed;
    Rigidbody2D rigid;

    //�÷��̾� ������
    SpriteRenderer spriter;

    //�÷��̾� �ִϸ��̼�
    Animator anim;

    float deadTimer;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    void Update()
    {
        {
            //GetAxis�� �̲������µ��� ����
            //inputVec.x = Input.GetAxis("Horizontal");
            //inputVec.y = Input.GetAxis("Vertical");

            //GetAxisRaw�� ��Ȯ�� ��Ʈ�� ����
            //inputVec.x = Input.GetAxisRaw("Horizontal");
            //inputVec.y = Input.GetAxisRaw("Vertical");
        }//�� �ٸ� ��ǲ�ý���
    }

    //���������� FixedUpdate����
    private void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        {
            // 1. ���� ����
            //rigid.AddForce(inputVec);

            // 2. �ӵ� ����
            //rigid.velocity = inputVec;

            // 3. ��ġ �̵�
            //rigid.MovePosition(rigid.position + inputVec);
        }//�� �ٸ� �̵� ���

        //�����Ӻ� �ӵ��� ���߱� ���� �۾�
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;

        //�÷��̾� ��ġ + (�÷��̾� ���� * �ӵ� * ��ŸŸ��)
        rigid.MovePosition(rigid.position + nextVec);
    }

    //��ǲ�ý����� �̿��� �̵� ���
    void OnMove(InputValue value)
    {
        //�÷��̾� ���� ����
        inputVec = value.Get<Vector2>();
    }

    //�������� ����Ǳ� �� ����Ǵ� �Լ�
    private void LateUpdate()
    {
        //������ ũ�Ⱚ�� ����Ͽ� 0.01 �̻��̸� Speed ����
        anim.SetFloat("Speed", inputVec.magnitude);

        if(inputVec.x != 0)
        {
            //�÷��̾��� ������ ���� ����
            spriter.flipX = inputVec.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.Instance.isLive)
            return;

        GameManager.Instance.health -= Time.deltaTime * 10;

        if(GameManager.Instance.health < 0)
        {
            for(int i = 2; i< transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            deadTimer += Time.deltaTime;
            anim.SetTrigger("Dead");
            if(deadTimer > 1.0f)
            {
                GameManager.Instance.GameOver();
                deadTimer = 0.0f;
            }
        }
    }


}
