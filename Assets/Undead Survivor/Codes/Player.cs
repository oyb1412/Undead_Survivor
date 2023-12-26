using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    public Scanner scanner;

    //플레이어 방향
    public Vector2 inputVec;

    //플레이어 속도
    public float speed;
    Rigidbody2D rigid;

    //플레이어 반전용
    SpriteRenderer spriter;

    //플레이어 애니메이션
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
            //GetAxis는 미끄러지는듯한 연출
            //inputVec.x = Input.GetAxis("Horizontal");
            //inputVec.y = Input.GetAxis("Vertical");

            //GetAxisRaw는 명확한 컨트롤 가능
            //inputVec.x = Input.GetAxisRaw("Horizontal");
            //inputVec.y = Input.GetAxisRaw("Vertical");
        }//또 다른 인풋시스템
    }

    //물리연산은 FixedUpdate에서
    private void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        {
            // 1. 힘을 가함
            //rigid.AddForce(inputVec);

            // 2. 속도 제어
            //rigid.velocity = inputVec;

            // 3. 위치 이동
            //rigid.MovePosition(rigid.position + inputVec);
        }//또 다른 이동 방식

        //프레임별 속도를 맞추기 위한 작업
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;

        //플레이어 위치 + (플레이어 방향 * 속도 * 델타타임)
        rigid.MovePosition(rigid.position + nextVec);
    }

    //인풋시스템을 이용한 이동 방식
    void OnMove(InputValue value)
    {
        //플레이어 방향 저장
        inputVec = value.Get<Vector2>();
    }

    //프레임이 종료되기 전 실행되는 함수
    private void LateUpdate()
    {
        //벡터의 크기값만 계산하여 0.01 이상이면 Speed 실행
        anim.SetFloat("Speed", inputVec.magnitude);

        if(inputVec.x != 0)
        {
            //플레이어의 방향을 토대로 반전
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
