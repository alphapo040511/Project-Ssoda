using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;

    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleMoveAndRotate();
    }

    private void HandleMoveAndRotate()
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
}
