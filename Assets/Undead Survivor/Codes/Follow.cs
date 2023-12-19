using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;
    // Start is called before the first frame update
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        //�ｺ���� ��ġ�� �÷��̾��� ��ġ�� ����(WorldToScreenPoint�Լ��� rect transform�� transform�� ����
        rect.position = Camera.main.WorldToScreenPoint(GameManager.Instance.player.transform.position);
    }
}
