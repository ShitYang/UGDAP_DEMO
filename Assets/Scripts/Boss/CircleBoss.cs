using UnityEngine;
using UnityEngine.UI;


public class CircleBoss : BaseBoss {

    [SerializeField] private ItemDialogue ItemDialogue;

    private int phase = 1;
    private float timer = 0f;
    public Transform firePoint;
    public float fireSpeed = 1f;


    private void Update()
    {
        if (player == null)
        {
            return;
        }


        if (healthComponent.GetHealthValRate() <= 0.3f) {
            phase = 3;
        }
        else if (healthComponent.GetHealthValRate() <= .7f) {
            phase = 2;
        }
        else {
            phase = 1;
        }

        timer += Time.deltaTime;
        if (timer >= fireSpeed)
        {
            AttackPattern();
            timer = 0f;
        }
        RotateTowardsPlayer();
    }

    public override void ActiveMonster(UPlayerController _player) {
        base.ActiveMonster(_player);
        ItemDialogue.EnterDialogue();
        _bossHealthBar.SetActive(true);
    }
    
    public override void DeActiveMonster()
    {
        base.DeActiveMonster();
        _bossHealthBar.SetActive(false);
    }


    void AttackPattern()
    {
        switch (phase)
        {
            case 1:
                FireCircularBullets(12);
                break;
            case 2:
                FireStraightAtPlayer();
                break;
            case 3:
                FireRandomBullets(8);
                break;
        }
    }
    void FireCircularBullets(int bulletCount)
    {
        float angleStep = 360f / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
            FireBullet(direction);
        }
    }
    void FireStraightAtPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        FireBullet(direction);
    }
    void FireRandomBullets(int bulletCount)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
            FireBullet(randomDirection);
        }
    }
    void FireBullet(Vector2 direction)
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position + (Vector3)direction * 2f,
            Quaternion.identity
        );
                
        UProjectile uProjectile = projectile.GetComponent<UProjectile>();
        uProjectile.from = gameObject;
        uProjectile.damage = attackVal;
                
        projectile.transform.up = direction;
    }
    
    void RotateTowardsPlayer()
    {
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

}

