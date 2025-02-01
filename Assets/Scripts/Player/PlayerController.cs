using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public PlayerStatusData playerStatus;
    public PlayerInput playerInput;

    private Rigidbody playerRigidbody;
    private ShakeDrink shakeDrink;
    private PlayerAttack playerAttack;
    private GizmoVisualizer visualizer;
    private RangeVisualizer rangeVisualizer;
    private PlayerReload playerReload;

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
    }

    private void Update()
    {
        if (playerStatus == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            // 현재 선택된 무기 타입으로 공격
            playerAttack.TryExecuteAttack(playerAttack.GetCurrentWeaponType());
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            playerReload?.Reload();
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
}