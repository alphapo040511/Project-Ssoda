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
        // �Է� ���� ���� ���
        Vector3 inputDirection = new Vector3(playerInput.moveX, 0, playerInput.moveY).normalized;

        if (inputDirection.magnitude > 0.1f)
        {
            // �÷��̾ �Է� ������ �ٶ󺸵��� ȸ��
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, targetRotation, Time.deltaTime * 10f);

            // �Է� �������� �̵�
            Vector3 moveDistance = inputDirection * moveSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
        }
    }
}
