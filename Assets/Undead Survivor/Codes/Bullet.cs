using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //�ҷ� ������
    public float damage;

    //�ҷ� ����
    public int penetrate;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int penetrate, Vector3 dir)
    {
        this.damage = damage;
        this.penetrate = penetrate;

        //�ҷ��� �������Ⱑ �ƴ� ��쿡��
        if(penetrate > -1)
        {
            //�߻����, �߻�� ����
            rigid.velocity = dir * 15f;
        }
    }

    //�߻��� �ҷ��� �浹����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�浹 ����� ���� �ƴϰų� �ҷ��� �߻����� �ƴ� ��쿣 �Լ� ����
        if (!collision.CompareTag("Enemy") || penetrate == -1)
            return;

        //�ҷ� ����� 1�� ����
        penetrate--;

        //�ҷ� ������� 0���� ����������
        if(penetrate == -1)
        {
            //�ҷ� ������ �ʱ�ȭ
            rigid.velocity = Vector2.zero;

            //�ҷ� ������Ʈ ��Ȱ��ȭ
            gameObject.SetActive(false);
        }
    }
}
