using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //불렛 데미지
    public float damage;

    //불렛 관통
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

        //불렛이 근접무기가 아닌 경우에만
        if(penetrate > -1)
        {
            //발사방향, 발사력 지정
            rigid.velocity = dir * 15f;
        }
    }

    //발사형 불렛의 충돌판정
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //충돌 대상이 적이 아니거나 불렛이 발사형이 아닌 경우엔 함수 종료
        if (!collision.CompareTag("Enemy") || penetrate == -1)
            return;

        //불렛 관통력 1씩 감소
        penetrate--;

        //불렛 관통력이 0보다 낮아졌을때
        if(penetrate == -1)
        {
            //불렛 물리력 초기화
            rigid.velocity = Vector2.zero;

            //불렛 오브젝트 비활성화
            gameObject.SetActive(false);
        }
    }
}
