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

    private PlayerReload playerReload;

    // �ֹ� ����
    private AttackType[] hotbarSlots = new AttackType[6];
    private int currentSlotIndex = 0; // ���� ���õ� ���� �ε���

    private void Start()
    {
        // ��ųʸ� �ʱ�ȭ
        attackStatusDict = attackStates.ToDictionary(
            item => item.type,
            item => item.data
        );

        // ���� Ÿ�Ժ� ������ ���� �ð� �ʱ�ȭ
        foreach (var attackType in attackStatusDict.Keys)
        {
            lastAttackTimeDict[attackType] = 0f;
        }

        playerReload = GetComponent<PlayerReload>();

        // �ֹ� ���� �ʱ�ȭ
        InitializeHotbar();
    }

    private void InitializeHotbar()
    {
        // �⺻ ���� ����
        hotbarSlots[0] = AttackType.NormalAtk;
        hotbarSlots[1] = AttackType.MeleeAtk;
        hotbarSlots[2] = AttackType.ThrowingAtk;

        // �߰� ���� ���� (���߿� ���� ���� ����� �ϵ��ڵ� �κ� ����� ���տ� ���� ����� �ִ� �������� ���� ����)
        hotbarSlots[3] = AttackType.SprayAtk;
        hotbarSlots[4] = AttackType.ContinuousAtk;
        hotbarSlots[5] = AttackType.RangedAtk;
    }

    private void Update()
    {
        // ���� �ӵ� ������Ʈ
        foreach (var attackType in attackStatusDict.Keys)
        {
            AttackStateData data = attackStatusDict[attackType];
            data.atkCooldown = 1f / playerStatus.AttackSpeed;
        }

        // ���� ���� �Է� ó��
        HandleWeaponSwitch();
    }

    public AttackType GetCurrentWeaponType()
    {
        return hotbarSlots[currentSlotIndex];
    }

    private void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchHotbarLeft();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchHotbarRight();
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0)
        {
            SwitchHotbarLeft(); // �� �ø�: �������� �̵�
        }
        else if (scroll < 0)
        {
            SwitchHotbarRight(); // �� ����: ���������� �̵�
        }
    }

    private void SwitchHotbarLeft()
    {
        currentSlotIndex--;
        if (currentSlotIndex < 0)
        {
            currentSlotIndex = hotbarSlots.Length - 1; // �� ������ ��ȯ
        }
        Debug.Log($"���� ���õ� ����: {currentSlotIndex + 1} ({hotbarSlots[currentSlotIndex]})");
    }

    private void SwitchHotbarRight()
    {
        currentSlotIndex++;
        if (currentSlotIndex >= hotbarSlots.Length)
        {
            currentSlotIndex = 0; // �� ������ ��ȯ
        }
        Debug.Log($"���� ���õ� ����: {currentSlotIndex + 1} ({hotbarSlots[currentSlotIndex]})");
    }

    public void TryExecuteAttack(AttackType attackType)
    {
        if (!attackStatusDict.ContainsKey(attackType)) return;

        // ź�� Ȯ��
        if (playerReload != null && !playerReload.UseAmmo(attackType, attackStatusDict))
        {
            Debug.Log("ź���� �����ϴ�! �������ϼ���.");
            return;
        }

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