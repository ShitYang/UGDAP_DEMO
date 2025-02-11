
using UnityEngine;

[CreateAssetMenu]
public class PowerWind : SkillData
{
    private float originalSpeed;

    public override void Activate(GameObject player)
    {
        UPlayerController playerController = player.GetComponent<UPlayerController>();
        if (playerController != null)
        {
            // ?›¥?????
            originalSpeed = playerController.GetMoveSpeed();

            // ??????????????
            playerController.SetUpMoveSpeed(originalSpeed * 2f);
        }
    }

    public override void Deactivate(GameObject player)
    {
        UPlayerController playerController = player.GetComponent<UPlayerController>();
        if (playerController != null)
        {
            // ????????
            playerController.SetUpMoveSpeed(originalSpeed);
        }
    }
}