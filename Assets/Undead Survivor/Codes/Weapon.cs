using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;
    void Awake()
    {
        player = GameManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        switch (id)
        {
            //0�� �����϶�
            case 0:
                //z���� �������� �ӵ���ŭ �ڵ�ȸ��
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                break;
            case 1:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0;
                    Fire();
                }
                break;
            default:
 
                break;
        }

        //test
        if(Input.GetButtonDown("Jump"))
        {
            LevelUp(5, 1);
        }
    }

    //���� ������ �Լ�
    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        //���� ���ġ
        if(id == 0)
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    //���� �ʱ�ȭ
    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for(int i = 0;i<GameManager.Instance.pool.prefabs.Length;i++)
        {
            if(data.projectile == GameManager.Instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch(id)
        {
            case 0:
                speed = -150;
                Batch();
                break;
            case 1:
                speed = 0.3f;
                break;
            default:
                break;
        }

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        //������ ������ŭ �ݺ�
        for(int index = 0; index < count; index++)
        {
            Transform bullet;
            //�ε����� ���� ������ ������ �ڽĿ�����Ʈ���� �������(�̹� ������ ���Ⱑ �ִ� ���)
            if(index < transform.childCount)
            {
                //�� �ڽ� ������Ʈ�� �״�� ���
                bullet = transform.GetChild(index);
            }
            //���������� ���� ���Ӱ� ��ġ�� ������ ���
            else
            {
                //Ǯ �Ŵ����� ���Ӱ� �ڽĿ�����Ʈ�� ����
                bullet = GameManager.Instance.pool.Get(prefabId).transform;

                //Ǯ �Ŵ����� �ִ� �ڽ� ������Ʈ�� weapon���� �̵�
                bullet.parent = transform;
            }

            //�������� ��ġ 0,0,0���� �ʱ�ȭ
            bullet.localPosition = Vector3.zero;

            //�������� ȸ���� �ʱ�ȭ
            bullet.localRotation = Quaternion.identity;

            //z���� �������� 360 * index * count�� ���� ����
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);

            //���� ���� �� y������ ��ġ�̵�
            bullet.Translate(bullet.up * 1.5f, Space.World);

            //�������� ����� ����
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
        }
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Melee);

    }

    //�Ѿ� �߻� �Լ�
    void Fire()
    {
        //��ĳ�ʿ� �ɸ� �ֳʹ̰� ������ �Լ� ����
        if (!player.scanner.nearestTarget)
            return;

        //��ĳ�ʿ� �ɸ� �ֳʹ� ��ġ 
        Vector3 targetPos = player.scanner.nearestTarget.position;

        //�÷��̾�->�ֳʹ� ���� ������ ����ȭ
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        //Ǯ �Ŵ����� ���Ӱ� �ڽĿ�����Ʈ�� ����
        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;

        bullet.position = transform.position;
        //�ҷ��� ȸ��(a = ȸ����, b=����)
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        //�������� �����, �߻� ���� ����
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);

    }
}
