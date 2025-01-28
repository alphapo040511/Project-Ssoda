using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public PlayerStatusData playerStatus;
    public PlayerInput playerInput;

    public List<AttackStateContainer> attackStates = new List<AttackStateContainer>();
    public Dictionary<AttackType, AttackStateData> attackStatusDict;
    private Dictionary<AttackType, float> lastAttackTimeDict = new Dictionary<AttackType, float>();

    private Rigidbody playerRigidbody;
    private GizmoVisualizer visualizer;
    private RangeVisualizer rangeVisualizer;
    private ShakeDrink shakeDrink;

    public bool isDashing = false;                      // 대쉬
    public bool isInvincible = false;                   // 무적
    public bool isRangeVisualizerActive = false;

    [Header("Player Bullet")]
    public GameObject projectilePrefab;                 // 발사체 프리팹
    public Transform projectileSpawnPoint;              // 발사체 생성 위치

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        visualizer = GetComponent<GizmoVisualizer>();
        rangeVisualizer = GetComponent<RangeVisualizer>();
        shakeDrink = GetComponent<ShakeDrink>();

        // LineRenderer 초기화
        rangeVisualizer.CreateRangeVisualizer();
        rangeVisualizer.ToggleRangeVisualizer(false);

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

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            HandleMoveAndRotate();
        }
    }

    private void Update()
    {
        if (playerStatus == null) return;

        // 모든 공격 타입의 쿨다운을 업데이트
        foreach (var attackType in attackStatusDict.Keys)
        {
            AttackStateData data = attackStatusDict[attackType];
            data.atkCooldown = 1f / playerStatus.AttackSpeed;       // 플레이어 스탯 반영
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryExecuteAttack(AttackType.NormalAtk);
        }

        // CapsLock 키 입력 감지로 토글
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            rangeVisualizer.ToggleRangeVisualizer(!isRangeVisualizerActive);
        }

        if (!isDashing && isRangeVisualizerActive)
        {
            rangeVisualizer.UpdateRangeVisualizer();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            shakeDrink.Shake();
        }
    }

    public void HandleMoveAndRotate()
    {
        if (playerStatus == null) return;

        // 카메라의 forward와 right 벡터를 이용하여 입력 방향 변환
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        // y축 영향 제거 (평면 이동)
        camForward.y = 0;
        camRight.y = 0;

        // 방향 정규화
        camForward.Normalize();
        camRight.Normalize();

        // 입력 방향 벡터를 카메라 기준으로 변환
        Vector3 inputDirection = (camRight * playerInput.moveX + camForward * playerInput.moveY).normalized;

        if (inputDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, targetRotation, Time.deltaTime * 10f);

            // StatusData에서 moveSpeed 값 가져오기
            Vector3 moveDistance = inputDirection * playerStatus.MoveSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
        }
    }

    public bool isGrounded()    // 땅 체크 확인
    {
        return Physics.Raycast(transform.position, Vector3.down, 2.0f);
    }

    public void StartDash()
    {
        if (!isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        isInvincible = true;

        Vector3 dashDirection = transform.forward;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + dashDirection * playerStatus.dashDistance;

        float elapsedTime = 0f;

        while (elapsedTime < playerStatus.dashDuration)
        {
            playerRigidbody.MovePosition(Vector3.Lerp(startPosition, targetPosition, elapsedTime / playerStatus.dashDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerRigidbody.MovePosition(targetPosition);

        isDashing = false;

        yield return new WaitForSeconds(0.2f); // 0.2초 무적 유지
        isInvincible = false;
    }

    private void TryExecuteAttack(AttackType attackType)
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

    public void ExecuteAttack(AttackType attackType)
    {
        if (attackStatusDict.TryGetValue(attackType, out AttackStateData data))
        {
            // 실제 공격 로직 (발사체 생성 등)
            AttackAtMouseDirection(data); // 데이터 전달
            Debug.Log($"Executed {attackType} : Speed={data.projectileSpeed}");
        }
        else
        {
            Debug.LogError($"Attack {attackType} not found!");
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

            // 발사체 생성
            GameObject projectile = Instantiate(
                projectilePrefab,
                projectileSpawnPoint.position,
                Quaternion.LookRotation(direction)
            );

            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            if (projectileRb != null)
            {
                // ScriptableObject의 데이터 사용
                projectileRb.velocity = direction * data.projectileSpeed;
                Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
            }

            // 발사체 제거 코루틴 (데이터 전달)
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