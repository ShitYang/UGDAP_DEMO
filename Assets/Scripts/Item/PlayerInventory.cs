using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //ʰȡ��ʹ��Ѫҩ
    public void PickupPotion(GameObject potionObject)
    {
        IItem item = potionObject.GetComponent<IItem>();
        if (item != null)
        {
            // ʹ�õ��ߣ������뵱ǰ��Ҷ���this.gameObject��
            item.Use(this.gameObject);
        }
        else
        {
            Debug.LogWarning("�õ���û��ʵ�� IItem �ӿڣ�");
        }
    }
}
