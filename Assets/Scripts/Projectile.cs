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
        Destroy(gameObject);        // �߻�ü �����ð��� �����ϰ� ���� �ε�ġ�� �ı�

        if (collision.gameObject.CompareTag("Enemy"))
        {
            attackedEnemy = collision.gameObject.GetComponent<EnemyBase>();
            attackedEnemy.TakeDamage(damage);
        }
    }
}