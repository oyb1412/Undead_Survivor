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

    //������Ʈ�� �浹���� ����� ȣ��Ǵ� �Լ�
    void OnTriggerExit2D(Collider2D collision)
    {
        //Ÿ�ϸʰ� ������(�÷��̾� �ֺ� ����)���� �浹�� �ƴϸ� ����
        if (!collision.CompareTag("Area"))
            return;

        //�÷��̾� ��ġ ����
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        //Ÿ�ϸ� ��ġ ����
        Vector3 TilePos = transform.position;

        //�÷��̾�� Ÿ�ϰ��� �Ÿ��� ���� x,y�� ���밪���� ����
        float diffX = Mathf.Abs(playerPos.x - TilePos.x);
        float diffY = Mathf.Abs(playerPos.y - TilePos.y);

        //�÷��̾� ���� ����
        Vector3 playerDir = GameManager.Instance.player.inputVec;

        //�÷��̾� ������ �ٰŷ� 1,-1�� ����
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        //� �±��� ������Ʈ�� �������� ����
        switch(transform.tag)
        {
            //�±װ� �׶���(Ÿ�ϸ�)�϶�
            case "Ground":
                //�÷��̾ Ÿ�ϸ��� x�� �������� Ż���Ϸ� �� ��
                if (diffX > diffY)
                {
                    //Ÿ���� x�� �������� Ÿ�ϸ�ũ��*2��ŭ 
                    transform.Translate(dirX * 40, 0, 0);
                }
                else if(diffX < diffY)
                {
                    //Ÿ���� y�� �������� Ÿ�ϸ�ũ��*2��ŭ �̵�
                    transform.Translate(0, dirY * 40, 0);
                }
                else
                {
                    //�÷��̾ ������ �밢�� �������� �̵��Ϸ��� �Ҷ� �밢�� �������� Ÿ�ϸ�ũ��*2��ŭ �̵�
                    transform.Translate(dirX * 40, dirY * 40, 0);
                }
                break;
                //�±װ� �ֳʹ��϶�(�÷��̾��� �̵����� �ֳʹ̰� �÷��̾��� �þ߿��� �������)
            case "Enemy":
                if(coll.enabled)
                {
                    //�ֳʹ̸� (�÷��̾� ���� * ������ �ָ� + x,y��ġ�� �������� ����) �� ������ �̵�
                    transform.Translate(playerDir * 30 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                }
                break;
        }
    }
}
