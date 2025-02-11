using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //拾取并使用血药
    public void PickupPotion(GameObject potionObject)
    {
        IItem item = potionObject.GetComponent<IItem>();
        if (item != null)
        {
            // 使用道具，并传入当前玩家对象（this.gameObject）
            item.Use(this.gameObject);
        }
        else
        {
            Debug.LogWarning("该道具没有实现 IItem 接口！");
        }
    }
}
