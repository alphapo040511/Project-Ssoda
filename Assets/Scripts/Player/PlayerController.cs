using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public PlayerStatusData playerStatus;
    public AttackStateData attackStatus;
    public PlayerInput playerInput;

    private Rigidbody playerRigidbody;
    private GizmoVisualizer visualizer;
    private RangeVisualizer rangeVisualizer;

    public bool isDashing = false;              // 대쉬
    public bool isInvincible = false;           // 무적
    public bool isRangeVisualizerActive = false;

    [Header("Player Bullet")]
    public GameObject projectilePrefab;         // 발사체 프리팹
    public Transform projectileSpawnPoint;      // 발사체 생성 위치

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        visualizer = GetComponent<GizmoVisualizer>();
        rangeVisualizer = GetComponent<RangeVisualizer>();

        // LineRenderer 초기화
        rangeVisualizer.CreateRangeVisualizer();
        rangeVisualizer.ToggleRangeVisualizer(false);
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
        if (playerStatus == null || attackStatus == null) return;

        // 공격 속도 적용
        attackStatus.atkCooldown = 1f / playerStatus.AttackSpeed;

        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time >= attackStatus.lastAttackTime + attackStatus.atkCooldown)
            {
                AttackAtMouseDirection();
                attackStatus.lastAttackTime = Time.time;
            }
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

    private void AttackAtMouseDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;
            targetPosition.y = transform.position.y;

            Vector3 direction = (targetPosition - projectileSpawnPoint.position).normalized;

            // 공격 방향으로 범위 시각화 업데이트
            rangeVisualizer.UpdateRangeVisualizer();

            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.LookRotation(direction));

            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            if (projectileRb != null)
            {
                projectileRb.velocity = direction * attackStatus.projectileSpeed;

                // 발사체와 캐릭터의 충돌 방지
                Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
            }

            StartCoroutine(DestroyProjectileAfterRange(projectile, projectileSpawnPoint.position));
        }
    }

    private IEnumerator DestroyProjectileAfterRange(GameObject projectile, Vector3 startPosition)
    {
        float distanceTraveled = 0f;

        while (distanceTraveled < attackStatus.atkRange)
        {
            if (projectile == null)
                yield break;

            distanceTraveled = Vector3.Distance(startPosition, projectile.transform.position);
            yield return null;
        }

        Destroy(projectile);
    }
}