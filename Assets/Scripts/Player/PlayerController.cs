using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashDistance = 3f;         // 대쉬 거리
    public float dashDuration = 0.2f;       // 무적 시간

    public PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    public bool isDashing = false;         // 대쉬
    public bool isInvincible = false;      // 무적

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
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
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            StartCoroutine(Dash());
        }
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

        PlayerAnimationManager animationManager = GetComponent<PlayerAnimationManager>();
        if (animationManager != null)
        {
            animationManager.animator.SetBool("IsDashing", true);
        }

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

        if (animationManager != null)
        {
            animationManager.animator.SetBool("IsDashing", false);
        }

        yield return new WaitForSeconds(0.2f); // 0.2초 무적 유지
        isInvincible = false;
    }

}
