using UnityEngine;

[CreateAssetMenu]
public class PowerFire : SkillData {

    [SerializeField] private GameObject powerFire;
    public override void Activate(GameObject player) {
        GameObject power = Instantiate(powerFire, player.transform.position, Quaternion.identity, player.transform);
        player.GetComponent<UPlayerController>().ChangeWeapon(power);
    }
}