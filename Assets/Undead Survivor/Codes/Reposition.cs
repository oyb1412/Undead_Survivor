using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    //오브젝트간 충돌에서 벗어날때 호출되는 함수
    void OnTriggerExit2D(Collider2D collision)
    {
        //타일맵과 에리어(플레이어 주변 영역)간의 충돌이 아니면 무시
        if (!collision.CompareTag("Area"))
            return;

        //플레이어 위치 저장
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        //타일맵 위치 저장
        Vector3 TilePos = transform.position;

        //플레이어와 타일간의 거리를 각각 x,y로 절대값으로 저장
        float diffX = Mathf.Abs(playerPos.x - TilePos.x);
        float diffY = Mathf.Abs(playerPos.y - TilePos.y);

        //플레이어 방향 저장
        Vector3 playerDir = GameManager.Instance.player.inputVec;

        //플레이어 방향을 근거로 1,-1로 저장
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        //어떤 태그의 오브젝트를 변경할지 결정
        switch(transform.tag)
        {
            //태그가 그라운드(타일맵)일때
            case "Ground":
                //플레이어가 타일맵의 x축 방향으로 탈출하려 할 때
                if (diffX > diffY)
                {
                    //타일을 x축 방향으로 타일맵크기*2만큼 
                    transform.Translate(dirX * 40, 0, 0);
                }
                else if(diffX < diffY)
                {
                    //타일을 y축 방향으로 타일맵크기*2만큼 이동
                    transform.Translate(0, dirY * 40, 0);
                }
                else
                {
                    //플레이어가 완전히 대각선 방향으로 이동하려고 할때 대각선 방향으로 타일맵크기*2만큼 이동
                    transform.Translate(dirX * 40, dirY * 40, 0);
                }
                break;
                //태그가 애너미일때(플레이어의 이동으로 애너미가 플레이어의 시야에서 벗어났을때)
            case "Enemy":
                if(coll.enabled)
                {
                    //애너미를 (플레이어 방향 * 적당히 멀리 + x,y위치를 랜덤으로 지정) 한 곳으로 이동
                    transform.Translate(playerDir * 30 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                }
                break;
        }
    }
}
