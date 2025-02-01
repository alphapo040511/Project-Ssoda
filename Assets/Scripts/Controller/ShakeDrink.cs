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
            Debug.Log("이미 음료를 흔들고 있습니다!");
            return;
        }

        if (playerReload != null && playerAttack != null && !playerReload.UseAmmo(AttackType.NormalAtk, playerAttack.attackStatusDict))
        {
            Debug.Log("탄약이 없습니다! 재장전하세요.");
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
