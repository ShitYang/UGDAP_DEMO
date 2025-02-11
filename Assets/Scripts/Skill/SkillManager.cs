using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public SkillData activeSkill;

    public GameObject skillPopup;

    public GameObject player;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void GetSkillButton()
    {
        if (activeSkill != null)
        {
            // 标记技能为解锁状态
            activeSkill.isUnlocked = true;

            activeSkill.Activate(player);

            CloseSkillPopup();

        }
    }

    public void CloseSkillPopup()
    {
        skillPopup.SetActive(false);
    }

    [Header("UI")]
    public Image skillImage;
    public Text skillNameText, skillDesText;

    public void DisplaySkillInfo()
    {
        skillImage.sprite = activeSkill.skillSprite;
        skillNameText.text = activeSkill.skillName;
        skillDesText.text = "描述：\n" + activeSkill.description;
    }

    public void OpenSkillPanel()
    {
        skillPopup.SetActive(true);
    }
}
