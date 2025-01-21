using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Player State")]
    public int maxHP = 1000;

    [Header("Player Movement")]
    public float moveSpeed = 5f;

    [Header("Player Dash")]
    public float dashDistance = 3f;             // 대쉬 거리
    public float dashDuration = 0.2f;           // 무적 시간

    public PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private LineRenderer rangeLineRenderer;

    public bool isDashing = false;              // 대쉬
    public bool isInvincible = false;           // 무적

    private bool isRangeVisualizerActive = false;

    [Header("Player Bullet")]
    public GameObject projectilePrefab;         // 발사체 프리팹
    public Transform projectileSpawnPoint;      // 발사체 생성 위치
    public float projectileSpeed = 10f;         // 발사체 속도
    public float projectileLifetime = 5f;       // 발사체 생존 시간

    [Header("Attack")]
    public float atkCooldown = 0.5f;         // 공격 속도 (초당)
    public float atkRange = 10f;             // 사거리
    public float projectileThickness = 0.8f;    // 발사체 두께

    private float lastAttackTime = 0f;

    private State playerState;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();

        // LineRenderer 초기화
        CreateRangeVisualizer();

        ToggleRangeVisualizer(false);

        playerState = new State("Player", maxHP, 1, moveSpeed, atkCooldown, atkRange, projectileThickness);
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
        // CapsLock 키 입력 감지로 토글
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            ToggleRangeVisualizer(!isRangeVisualizerActive); // 현재 상태 반대로 설정
        }

        if (!isDashing && isRangeVisualizerActive)
        {
            UpdateRangeVisualizer(); // 범위 시각화 업데이트
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time >= lastAttackTime + atkCooldown)
            {
                AttackAtMouseDirection();
                lastAttackTime = Time.time;
            }
        }

        // 테스트용
        //TakeDamage(10);
    }

    public void HandleMoveAndRotate()
    {
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
            // 입력 방향을 바라보도록 회전
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, targetRotation, Time.deltaTime * 10f);

            // 입력 방향으로 이동
            Vector3 moveDistance = inputDirection * moveSpeed * Time.deltaTime;
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
        Vector3 targetPosition = startPosition + dashDirection * dashDistance;

        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            playerRigidbody.MovePosition(Vector3.Lerp(startPosition, targetPosition, elapsedTime / dashDuration));
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
            targetPosition.y = transform.position.y; // 높이는 캐릭터와 동일하게 유지

            // 클릭한 방향으로 발사체 생성
            Vector3 direction = (targetPosition - projectileSpawnPoint.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.LookRotation(direction));

            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            if (projectileRb != null)
            {
                projectileRb.velocity = direction * projectileSpeed;

                // 발사체와 캐릭터의 충돌 방지
                Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
            }

            // 발사체의 사거리 제한
            StartCoroutine(DestroyProjectileAfterRange(projectile, projectileSpawnPoint.position));

            // 발사체 두께 시각화
            LineRenderer lineRenderer = projectile.GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                lineRenderer.startWidth = projectileThickness;
                lineRenderer.endWidth = projectileThickness;
                lineRenderer.SetPosition(0, projectileSpawnPoint.position);
                lineRenderer.SetPosition(1, projectileSpawnPoint.position + direction * atkRange);
            }
        }
    }

    private IEnumerator DestroyProjectileAfterRange(GameObject projectile, Vector3 startPosition)
    {
        float distanceTraveled = 0f;

        while (distanceTraveled < atkRange)
        {
            if (projectile == null)
                yield break;

            distanceTraveled = Vector3.Distance(startPosition, projectile.transform.position);
            yield return null;
        }

        Destroy(projectile);
    }

    private void OnDrawGizmos()
    {
        if (projectileSpawnPoint != null)
        {
            // 사거리 기즈모 (빨간색)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(projectileSpawnPoint.position, atkRange);

            // 투사체 범위(두께) 기즈모 (파란색)
            Gizmos.color = Color.blue;
            Vector3 start = projectileSpawnPoint.position;
            Vector3 end = start + transform.forward * atkRange;

            // 투사체 두께 반영
            Gizmos.DrawLine(start, end);
            Gizmos.DrawWireSphere(start, projectileThickness / 2);
            Gizmos.DrawWireSphere(end, projectileThickness / 2);
        }
    }

    private void CreateRangeVisualizer()
    {
        // LineRenderer 컴포넌트 추가
        rangeLineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer 설정
        rangeLineRenderer.startWidth = projectileThickness;
        rangeLineRenderer.endWidth = projectileThickness;
        rangeLineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 기본 쉐이더
        rangeLineRenderer.startColor = Color.blue;
        rangeLineRenderer.endColor = Color.blue;

        rangeLineRenderer.positionCount = 2; // 시작점과 끝점
        rangeLineRenderer.useWorldSpace = true;

        UpdateRangeVisualizer();
    }

    private void UpdateRangeVisualizer()
    {
        if (rangeLineRenderer != null && projectileSpawnPoint != null)
        {
            Vector3 start = projectileSpawnPoint.position;
            Vector3 end = start + transform.forward * atkRange;

            rangeLineRenderer.SetPosition(0, start); // 시작 위치
            rangeLineRenderer.SetPosition(1, end);   // 끝 위치
        }
    }

    private void ToggleRangeVisualizer(bool isActive)
    {
        if (rangeLineRenderer != null)
        {
            rangeLineRenderer.enabled = isActive; // 활성화 여부 토글
        }
        isRangeVisualizerActive = isActive;
    }

    public void TakeDamage(int damage)
    {
        playerState.TakeDamage(damage);
        Debug.Log($"플레이어 현재 체력: {playerState.nowHp}");
    }
}