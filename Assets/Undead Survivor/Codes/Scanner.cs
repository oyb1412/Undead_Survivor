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
        //������ ĳ��Ʈ�� ��� ��� ����� ��ȯ�ϴ� �Լ� (1.ĳ���� ���� ��ġ 2.���� ������ 3.ĳ���� ���� 4.��� ������ ���� 5.��� ���̾� )
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            //�÷��̾� ������
            Vector3 myPos = transform.position;

            //�ֳʹ��� ������
            Vector3 targetPos = target.transform.position;

            //���� a,b�� �Ÿ��� ��ȯ�ϴ� �Լ�
            float curDiff = Vector3.Distance(myPos, targetPos);

            //������ �����ȿ� �ֳʹ̰� �����ҽ�
            if(curDiff < diff)
            {
                //�� �ֳʹ̿��� �Ÿ��� ���Ӱ� ����
                diff = curDiff;

                //�� �ֳʹ̸� ���� ����� Ÿ������ ����
                result = target.transform;
            }
        }


        return result;
    }
}
