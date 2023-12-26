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
            //0번 무기일때
            case 0:
                //z축을 기준으로 속도만큼 자동회전
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

    //무기 레벨업 함수
    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        //무기 재배치
        if(id == 0)
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    //무기 초기화
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
        //무기의 갯수만큼 반복
        for(int index = 0; index < count; index++)
        {
            Transform bullet;
            //인덱스가 현재 생성된 웨폰의 자식오브젝트보다 작을경우(이미 생성된 무기가 있는 경우)
            if(index < transform.childCount)
            {
                //그 자식 오브젝트를 그대로 사용
                bullet = transform.GetChild(index);
            }
            //레벨업으로 인해 새롭게 배치를 진행할 경우
            else
            {
                //풀 매니저에 새롭게 자식오브젝트로 생성
                bullet = GameManager.Instance.pool.Get(prefabId).transform;

                //풀 매니저에 있는 자식 오브젝트를 weapon으로 이동
                bullet.parent = transform;
            }

            //생성직후 위치 0,0,0으로 초기화
            bullet.localPosition = Vector3.zero;

            //생성직후 회전량 초기화
            bullet.localRotation = Quaternion.identity;

            //z축을 기준으로 360 * index * count로 각도 조정
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);

            //각도 조정 후 y축으로 위치이동
            bullet.Translate(bullet.up * 1.5f, Space.World);

            //데미지와 관통력 지정
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
        }
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Melee);

    }

    //총알 발사 함수
    void Fire()
    {
        //스캐너에 걸린 애너미가 없으면 함수 종료
        if (!player.scanner.nearestTarget)
            return;

        //스캐너에 걸린 애너미 위치 
        Vector3 targetPos = player.scanner.nearestTarget.position;

        //플레이어->애너미 벡터 저장후 정규화
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        //풀 매니저에 새롭게 자식오브젝트로 생성
        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;

        bullet.position = transform.position;
        //불렛의 회전(a = 회전축, b=벡터)
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        //데미지와 관통력, 발사 방향 지정
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);

    }
}
