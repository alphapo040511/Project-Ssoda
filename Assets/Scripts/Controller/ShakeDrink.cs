using System.Collections;
using UnityEngine;

public class ShakeDrink : MonoBehaviour
{
    private PlayerReload playerReload;
    private PlayerAttack playerAttack;

    private bool isShaking = false;
    private float shakeDuration = 10f;

    private void Start()
    {
        playerReload = GetComponent<PlayerReload>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    public void Shake()
    {
        if (isShaking)
        {
            Debug.Log("�̹� ���Ḧ ���� �ֽ��ϴ�!");
            return;
        }

        if (playerReload != null && playerAttack != null && !playerReload.UseAmmo(AttackType.NormalAtk, playerAttack.attackStatusDict))
        {
            Debug.Log("ź���� �����ϴ�! �������ϼ���.");
            return;
        }

        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        isShaking = true;
        Debug.Log("���Ḧ �������ϴ�! (���� ��ȭ ����)");

        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            Debug.Log($"���� ȿ�� ���� ��... {shakeDuration - elapsedTime:F1}�� ����");
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }

        isShaking = false;
        Debug.Log("���� ȿ���� ����Ǿ����ϴ�.");
    }
}
