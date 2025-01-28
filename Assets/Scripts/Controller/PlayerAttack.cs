using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerAttack : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    [Header("Status")]
    public PlayerStatusData playerStatus;
    public List<AttackStateContainer> attackStates = new List<AttackStateContainer>();
    public Dictionary<AttackType, AttackStateData> attackStatusDict;
    private Dictionary<AttackType, float> lastAttackTimeDict = new Dictionary<AttackType, float>();

    private void Start()
    {
        // 딕셔너리 초기화
        attackStatusDict = attackStates.ToDictionary(
            item => item.type,
            item => item.data
        );

        // 공격 타입별 마지막 공격 시간 초기화
        foreach (var attackType in attackStatusDict.Keys)
        {
            lastAttackTimeDict[attackType] = 0f;
        }
    }

    private void Update()
    {
        // 모든 공격 타입의 쿨다운을 업데이트
        foreach (var attackType in attackStatusDict.Keys)
        {
            AttackStateData data = attackStatusDict[attackType];
            data.atkCooldown = 1f / playerStatus.AttackSpeed;       // 플레이어 스탯 반영
        }
    }

    public void Initialize(List<AttackStateContainer> attackStates)
    {
        attackStatusDict = attackStates.ToDictionary(item => item.type, item => item.data);
        foreach (var attackType in attackStatusDict.Keys)
        {
            lastAttackTimeDict[attackType] = 0f;
        }
    }

    public void TryExecuteAttack(AttackType attackType)
    {
        if (!attackStatusDict.ContainsKey(attackType)) return;

        AttackStateData data = attackStatusDict[attackType];
        float lastAttackTime = lastAttackTimeDict.ContainsKey(attackType) ? lastAttackTimeDict[attackType] : 0f;

        if (Time.time >= lastAttackTime + data.atkCooldown)
        {
            ExecuteAttack(attackType);
            lastAttackTimeDict[attackType] = Time.time;
        }
    }

    private void ExecuteAttack(AttackType attackType)
    {
        if (attackStatusDict.TryGetValue(attackType, out AttackStateData data))
        {
            AttackAtMouseDirection(data);
            Debug.Log($"Executed {attackType} : Speed={data.projectileSpeed}");
        }
    }

    private void AttackAtMouseDirection(AttackStateData data)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;
            targetPosition.y = transform.position.y;

            Vector3 direction = (targetPosition - projectileSpawnPoint.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.LookRotation(direction));
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            if (projectileRb != null)
            {
                projectileRb.velocity = direction * data.projectileSpeed;
                Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
            }

            StartCoroutine(DestroyProjectileAfterRange(projectile, projectileSpawnPoint.position, data.atkRange));
        }
    }

    private IEnumerator DestroyProjectileAfterRange(GameObject projectile, Vector3 startPosition, float range)
    {
        float distanceTraveled = 0f;
        while (distanceTraveled < range)
        {
            if (projectile == null) yield break;
            distanceTraveled = Vector3.Distance(startPosition, projectile.transform.position);
            yield return null;
        }
        Destroy(projectile);
    }
}