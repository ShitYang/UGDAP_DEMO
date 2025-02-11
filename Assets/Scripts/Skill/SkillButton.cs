using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerClickHandler
{
    public SkillData skillData;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("=========<<<<<<<<<<<<<<<=========");
        SkillManager instance = SkillManager.instance;
        instance.activeSkill = skillData;
        instance.DisplaySkillInfo();
        
    }
}
