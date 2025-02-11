using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPotion:MonoBehaviour,IItem
{
    public float healAoumnt = 50f;

    public void Use(GameObject user)
    {
        HealthComponent health = user.GetComponentInChildren<HealthComponent>();
        if (health != null)
        {
            if (health.currentHealthVal < health.GetMaxHealthVal())
            {
                health.IncreaseHealthVal(healAoumnt);
                Debug.Log("ʹ��Ѫҩ�ɹ���������" + healAoumnt + "����ֵ");
            }
            else
            {
                Debug.Log("����ֵ����������ʹ��Ѫҩ");
            }
        }
        else
        {
            Debug.LogWarning("ʹ������û���ҵ�HealthComponent���޷�ʹ��Ѫҩ");
        }
    }
}
