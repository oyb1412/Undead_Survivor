using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    void FixedUpdate()
    {
        //원형의 캐스트를 쏘고 모든 결과를 반환하는 함수 (1.캐스팅 시작 위치 2.원의 반지름 3.캐스팅 방향 4.쏘는 방향의 길이 5.대상 레이어 )
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            //플레이어 포지션
            Vector3 myPos = transform.position;

            //애너미의 포지션
            Vector3 targetPos = target.transform.position;

            //벡터 a,b의 거리를 반환하는 함수
            float curDiff = Vector3.Distance(myPos, targetPos);

            //지정된 범위안에 애너미가 존재할시
            if(curDiff < diff)
            {
                //그 애너미와의 거리를 새롭게 지정
                diff = curDiff;

                //그 애너미를 가장 가까운 타겟으로 지정
                result = target.transform;
            }
        }


        return result;
    }
}
