using System.Collections;
using UnityEngine;

public class ShakeDrink : MonoBehaviour
{
    private bool isShaking = false;
    private float shakeDuration = 10f;

    public void Shake()
    {
        if (isShaking)
        {
            Debug.Log("�̹� ���Ḧ ���� �ֽ��ϴ�!");
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
