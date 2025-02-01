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

    // 핫바 슬롯
    private AttackType[] hotbarSlots = new AttackType[6];
    private int currentSlotIndex = 0; // 현재 선택된 슬롯 인덱스

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

        playerReload = GetComponent<PlayerReload>();

        // 핫바 슬롯 초기화
        InitializeHotbar();
    }

    private void InitializeHotbar()
    {
        // 기본 공격 슬롯
        hotbarSlots[0] = AttackType.NormalAtk;
        hotbarSlots[1] = AttackType.MeleeAtk;
        hotbarSlots[2] = AttackType.ThrowingAtk;

        // 추가 공격 슬롯 (나중에 음료 조합 생기면 하드코딩 부분 지우고 조합에 따른 방식을 넣는 형식으로 수정 예정)
        hotbarSlots[3] = AttackType.SprayAtk;
        hotbarSlots[4] = AttackType.ContinuousAtk;
        hotbarSlots[5] = AttackType.RangedAtk;
    }

    private void Update()
    {
        // 공격 속도 업데이트
        foreach (var attackType in attackStatusDict.Keys)
        {
            AttackStateData data = attackStatusDict[attackType];
            data.atkCooldown = 1f / playerStatus.AttackSpeed;
        }

        // 무기 변경 입력 처리
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
            SwitchHotbarLeft(); // 휠 올림: 왼쪽으로 이동
        }
        else if (scroll < 0)
        {
            SwitchHotbarRight(); // 휠 내림: 오른쪽으로 이동
        }
    }

    private void SwitchHotbarLeft()
    {
        currentSlotIndex--;
        if (currentSlotIndex < 0)
        {
            currentSlotIndex = hotbarSlots.Length - 1; // 맨 끝으로 순환
        }
        Debug.Log($"현재 선택된 슬롯: {currentSlotIndex + 1} ({hotbarSlots[currentSlotIndex]})");
    }

    private void SwitchHotbarRight()
    {
        currentSlotIndex++;
        if (currentSlotIndex >= hotbarSlots.Length)
        {
            currentSlotIndex = 0; // 맨 앞으로 순환
        }
        Debug.Log($"현재 선택된 슬롯: {currentSlotIndex + 1} ({hotbarSlots[currentSlotIndex]})");
    }

    public void TryExecuteAttack(AttackType attackType)
    {
        if (!attackStatusDict.ContainsKey(attackType)) return;

        // 탄약 확인
        if (playerReload != null && !playerReload.UseAmmo(attackType, attackStatusDict))
        {
            Debug.Log("탄약이 없습니다! 재장전하세요.");
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