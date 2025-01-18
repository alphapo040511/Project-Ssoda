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
        // 입력 방향 벡터 계산
        Vector3 inputDirection = new Vector3(playerInput.moveX, 0, playerInput.moveY).normalized;

        if (inputDirection.magnitude > 0.1f)
        {
            // 플레이어가 입력 방향을 바라보도록 회전
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, targetRotation, Time.deltaTime * 10f);

            // 입력 방향으로 이동
            Vector3 moveDistance = inputDirection * moveSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
        }
    }
}
