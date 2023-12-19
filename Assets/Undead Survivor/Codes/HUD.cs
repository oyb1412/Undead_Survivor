using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //����ġ���� ����ϱ� ���� ������ ���
    public enum InfoType 
    {
        Exp,
        Level,
        Kill,
        Time,
        Health
    }

    public InfoType type;

    Text myText;
    Slider mySlider;

    private void Awake()
    {
        mySlider = GetComponent<Slider>();
        myText = GetComponent<Text>();
    }

    private void LateUpdate()
    {
        switch(type)
        {
            case InfoType.Exp:
                float curExp = GameManager.Instance.exp;
                float maxExp = GameManager.Instance.nextExp[GameManager.Instance.level];
                mySlider.value = curExp / maxExp;
                break;
            //���� TextUI�϶�
            case InfoType.Level:
                //{}�� ù���� �Ķ���� 0 �� �Ҽ����� ������ ǥ���ϱ� ���� F0�� �Է�, ���ڿ��� �ޱ� ������ string.Format�� ���
                myText.text = string.Format("Lv.{0:F0}", GameManager.Instance.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.Instance.kill);
                break;
            case InfoType.Time:
                float remainTime = GameManager.Instance.maxGameTime - GameManager.Instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                //������ �ڸ����� 2�ڸ� �̻� ǥ���ϱ� ���� D2�� �Է�
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                float curHealth = GameManager.Instance.health;
                float maxHealth = GameManager.Instance.maxHealth;
                mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}
