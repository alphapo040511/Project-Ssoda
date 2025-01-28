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
            Debug.Log("이미 음료를 흔들고 있습니다!");
            return;
        }

        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        isShaking = true;
        Debug.Log("음료를 흔들었습니다! (공격 강화 시작)");

        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            Debug.Log($"음료 효과 지속 중... {shakeDuration - elapsedTime:F1}초 남음");
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }

        isShaking = false;
        Debug.Log("음료 효과가 종료되었습니다.");
    }
}
