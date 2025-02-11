using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class PowerLand : SkillData
{
    private bool isHealingActive = false;

    // ��������ϻ�ȡ UPlayerController �������������ѪЭ��
    public override void Activate(GameObject player)
    {
        UPlayerController controller = player.GetComponent<UPlayerController>();
        
        if(controller!=null)
        {
            controller.StartCoroutine(HealOverTime(controller));
        }
    }

    IEnumerator HealOverTime(UPlayerController playerController)
    {
        // ����ѭ����ÿ1��ָ�5��Ѫ��
        while (true)
        {
            HealthComponent health = playerController.GetHealthComponent();

            if(health.GetHealthValRate()<1.0f)
            {
                health.IncreaseHealthVal(5f);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}