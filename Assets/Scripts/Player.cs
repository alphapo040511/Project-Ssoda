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
        // ī�޶��� forward�� right ���͸� �̿��Ͽ� �Է� ���� ��ȯ
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        // y�� ���� ���� (��� �̵�)
        camForward.y = 0;
        camRight.y = 0;

        // ���� ����ȭ
        camForward.Normalize();
        camRight.Normalize();

        // �Է� ���� ���͸� ī�޶� �������� ��ȯ
        Vector3 inputDirection = (camRight * playerInput.moveX + camForward * playerInput.moveY).normalized;

        if (inputDirection.magnitude > 0.1f)
        {
            // �Է� ������ �ٶ󺸵��� ȸ��
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, targetRotation, Time.deltaTime * 10f);

            // �Է� �������� �̵�
            Vector3 moveDistance = inputDirection * moveSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
        }
    }
}
