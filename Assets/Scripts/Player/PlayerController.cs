using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public PlayerStatusData playerStatus;
    public PlayerInput playerInput;
    public Rigidbody playerRigidbody;

    private ShakeDrink shakeDrink;
    private PlayerAttack playerAttack;
    private GizmoVisualizer visualizer;
    private RangeVisualizer rangeVisualizer;
    private PlayerReload playerReload;

    private float lastDashTime = -Mathf.Infinity;       // 마지막 대시 시간

    public bool isDashing = false;                      // 대쉬
    public bool isInvincible = false;                   // 무적
    public bool isRangeVisualizerActive = false;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        visualizer = GetComponent<GizmoVisualizer>();
        rangeVisualizer = GetComponent<RangeVisualizer>();
        shakeDrink = GetComponent<ShakeDrink>();
        playerAttack = GetComponent<PlayerAttack>();
        playerReload = GetComponent<PlayerReload>();

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

        // 회전 속도 제어
        playerRigidbody.angularVelocity = Vector3.zero;
    }

    private void Update()
    {
        if (playerStatus == null) return;

        /*if (Input.GetMouseButtonDown(0))
        {
            // 현재 선택된 무기 타입으로 공격
            playerAttack.TryExecuteAttack(playerAttack.GetCurrentWeaponType());
        }*/

        // 무기 타입에 따라 공격 조작이 다른 것 같아서 이렇게 해뒀는데 분사형도 클릭 토글 형식으로 공격할거라면 위로 변경해도됨
        switch (playerAttack.GetCurrentWeaponType())
        {
            case AttackType.NormalAtk:
            case AttackType.MeleeAtk:
                if (Input.GetMouseButtonDown(0))
                    playerAttack.TryExecuteAttack(playerAttack.GetCurrentWeaponType());
                break;

            case AttackType.ThrowingAtk:
            case AttackType.RangedAtk:
                if (Input.GetMouseButtonDown(0))
                    playerAttack.TryExecuteAttack(playerAttack.GetCurrentWeaponType());
                break;

            case AttackType.SprayAtk:
            case AttackType.ContinuousAtk:
                if (Input.GetMouseButton(0))
                    playerAttack.TryExecuteAttack(playerAttack.GetCurrentWeaponType());
                break;
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

        /*if (!isDashing && isRangeVisualizerActive && playerAttack.currentWeaponType == AttackType.MeleeAtk)
        {
            rangeVisualizer.DrawMeleeAttackRange();
        }*/

        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time >= lastDashTime + playerStatus.dashCooldown)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            shakeDrink.Shake();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            playerReload?.Reload();
        }
    }

    public void HandleMoveAndRotate()
    {
        if (playerStatus == null) return;

        Vector3 inputDirection = (Camera.main.transform.right * playerInput.moveX
                                + Camera.main.transform.forward * playerInput.moveY).normalized;
        inputDirection.y = 0;

        if (inputDirection.magnitude > 0.1f)
        {
            // 부드러운 회전
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            playerRigidbody.rotation = Quaternion.Slerp(
                playerRigidbody.rotation,
                targetRotation,
                Time.deltaTime * 10f
            );

            // 이동 속도 적용
            playerRigidbody.velocity = inputDirection * playerStatus.MoveSpeed;

            // 회전 속도 제어
            playerRigidbody.angularVelocity = Vector3.zero;
        }
        else
        {
            // 입력 없을 시 즉시 정지
            playerRigidbody.velocity = Vector3.zero;
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
        lastDashTime = Time.time;

        Vector3 dashDirection = transform.forward;
        float dashSpeed = playerStatus.dashDistance / playerStatus.dashDuration;

        // 초기 속도 적용
        playerRigidbody.velocity = dashDirection * dashSpeed;

        float elapsedTime = 0f;
        while (elapsedTime < playerStatus.dashDuration)
        {
            // 벽 충돌 감지
            if (CheckFrontCollision())
            {
                Debug.Log("벽 충돌로 대시 중단");
                break;
            }

            // 회전 속도 제어
            playerRigidbody.angularVelocity = Vector3.zero;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 대시 종료 후 속도 초기화
        playerRigidbody.velocity = Vector3.zero;
        isDashing = false;

        yield return new WaitForSeconds(0.2f);
        isInvincible = false;
    }

    private bool CheckFrontCollision()
    {
        // 캡슐 캐스트로 전방 충돌 검출
        float radius = 0.5f;
        float distance = 0.1f;
        bool isColliding = Physics.SphereCast(
            transform.position,
            radius,
            transform.forward,
            out RaycastHit hit,
            distance,
            LayerMask.GetMask("Wall")
        );

        if (isColliding)
        {
            // 벽에 부딪혔을 때 회전 속도 초기화
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        return isColliding;
    }
}