using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public EnemyBase attackedEnemy;

    private void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);        // 발사체 생존시간을 삭제하고 어디든 부딪치면 파괴

        if (collision.gameObject.CompareTag("Enemy"))
        {
            attackedEnemy = collision.gameObject.GetComponent<EnemyBase>();
            attackedEnemy.TakeDamage(damage);
        }
    }
}