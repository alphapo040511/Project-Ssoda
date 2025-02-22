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
    public AttackType currentWeaponType; // ���� ���� Ÿ��
    public BattleRoom currentRoom;

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

        // ���� ���� Ÿ�� �ʱ�ȭ
        currentWeaponType = hotbarSlots[currentSlotIndex];
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
        /*foreach (var attackType in attackStatusDict.Keys)
        {
            AttackStateData data = attackStatusDict[attackType];
        }*/

        // ���� foreach�� ��� �� �ؼ� ����ó�� �ص� > ��� ���̴°ǰ���?

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
        UpdateCurrentWeaponType();
        Debug.Log($"���� ���õ� ����: {currentSlotIndex + 1} ({hotbarSlots[currentSlotIndex]})");
    }

    private void SwitchHotbarRight()
    {
        currentSlotIndex++;
        if (currentSlotIndex >= hotbarSlots.Length)
        {
            currentSlotIndex = 0; // �� ������ ��ȯ
        }
        UpdateCurrentWeaponType();
        Debug.Log($"���� ���õ� ����: {currentSlotIndex + 1} ({hotbarSlots[currentSlotIndex]})");
    }

    private void UpdateCurrentWeaponType()
    {
        // ���� ���� Ÿ�� ������Ʈ
        currentWeaponType = hotbarSlots[currentSlotIndex];
        Debug.Log($"���� ���� Ÿ��: {currentWeaponType}");
    }

    // ź��, ��Ÿ�� ��� �� ���� �õ� (���� Ÿ�Կ� ������� ��� ���� �޼���)
    public void TryExecuteAttack(AttackType attackType)
    {
        if (!attackStatusDict.ContainsKey(attackType)) return;

        if (attackType == AttackType.SprayAtk || attackType == AttackType.ContinuousAtk)        // ���������� ���Ḧ �Ҹ��ϴ� ����Ÿ���� ���
        {
            // ź�� Ȯ��
            if (playerReload != null && !playerReload.UseSprayAmmo(attackType, attackStatusDict))
            {
                Debug.Log("ź���� �����ϴ�! �������ϼ���.");
                return;
            }
        }
        else
        {
            // ź�� Ȯ��
            if (playerReload != null && !playerReload.UseAmmo(attackType, attackStatusDict))
            {
                Debug.Log("ź���� �����ϴ�! �������ϼ���.");
                return;
            }
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
            switch (attackType)
            {
                case AttackType.NormalAtk:
                    AttackAtMouseDirection(data);
                    Debug.Log($"Executed {attackType} : Speed={data.projectileSpeed}");
                    break;

                case AttackType.MeleeAtk:
                    MeleeAttack(data);
                    break;

                case AttackType.ThrowingAtk:
                case AttackType.RangedAtk:
                    break;

                case AttackType.SprayAtk:
                case AttackType.ContinuousAtk:
                    break;
            }
        }
    }

    // ĳ���� ���� ���콺 ���� �ܹ� ���� (NormalAtk)
    private void AttackAtMouseDirection(AttackStateData data)
    {
        if (currentWeaponType != AttackType.NormalAtk) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;
            targetPosition.y = transform.position.y;

            Vector3 direction = (targetPosition - projectileSpawnPoint.position).normalized;

            // ������Ÿ�� ����
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.LookRotation(direction));
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            if (projectileRb != null)
            {
                projectileRb.velocity = direction * data.projectileSpeed;
                Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
            }

            // Projectile ��ũ��Ʈ�� ������ ����
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.damage = data.attackPower;
            }

            StartCoroutine(DestroyProjectileAfterRange(projectile, projectileSpawnPoint.position, data.atkRange));
        }
    }

    // ���� ���� (MeleeAtk)
    private void MeleeAttack(AttackStateData data)
    {
        if (currentWeaponType != AttackType.MeleeAtk || currentRoom == null) return;

        Vector3 playerPosition = transform.position;
        Vector3 mousePos = Input.mousePosition;
        float distanceFromCamera = Vector3.Distance(Camera.main.transform.position, transform.position);
        mousePos.z = distanceFromCamera; // ī�޶�� �÷��̾� ������ �Ÿ��� ����

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.y = playerPosition.y;

        Vector3 attackDirection = (worldPos - playerPosition).normalized; // ���콺 �������� ������Ʈ

        for (int i = currentRoom.enemies.Count - 1; i >= 0; i--)
        {
            EnemyBase enemy = currentRoom.enemies[i];
            if (enemy == null) continue;

            float distance = Vector3.Distance(enemy.transform.position, playerPosition);
            if (distance > data.atkRange) continue;

            Vector3 targetDirection = (enemy.transform.position - playerPosition).normalized;
            float angle = Vector3.Angle(attackDirection, targetDirection);

            //Debug.LogWarning($"���� ���� {attackDirection}, ���콺 ���� {mouseWorldPos}, �÷��̾� ��ġ {playerPosition}, �Ÿ�: {distance}, ����: {angle}");

            if (angle <= data.attackAngle / 2)
            {
                Debug.LogError("������ ����!");
                enemy.TakeDamage(data.attackPower);
            }
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