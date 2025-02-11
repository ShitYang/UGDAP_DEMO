using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SkillData : ScriptableObject
{
    public int skillID;//����ID
    public string skillName;//������
    public Sprite skillSprite;//����ͼ��
    [TextArea] public string description;//��������

    public bool isUnlocked;//�ж��Ƿ��������

    public abstract void Activate(GameObject player);
    public virtual void Deactivate(GameObject player) { }
}
