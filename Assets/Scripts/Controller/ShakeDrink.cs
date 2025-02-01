using UnityEngine;
using System.Collections;

public class ShakeDrink : MonoBehaviour
{
    private PlayerAttack playerAttack;

    private bool isShaking = false;         // 흔들기 중인지 여부
    private float shakeDuration = 10f;      // 강화 지속 시간 (10초)

    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }

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
        Debug.Log("음료를 흔들어 공격 방식을 강화합니다!");

        // 현재 들고 있는 음료의 공격 타입에 따라 스탯 강화
        EnhanceAttackStats();

        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            Debug.Log($"음료 효과 지속 중... {shakeDuration - elapsedTime:F1}초 남음");
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }

        // 강화 효과 종료
        ResetAttackStats();

        isShaking = false;

        Debug.Log("음료 효과가 종료되었습니다.");
    }

    private void EnhanceAttackStats()
    {
        if (playerAttack == null || playerAttack.attackStatusDict == null) return;

        // 현재 들고 있는 음료의 공격 타입에 따라 스탯 강화
        foreach (var attackType in playerAttack.attackStatusDict.Keys)
        {
            AttackStateData data = playerAttack.attackStatusDict[attackType];

            switch (attackType)
            {
                case AttackType.NormalAtk:
                    data.ammoCost = 55;                                     // 흔들었을 때 음료 소비량: 55ml
                    data.attackPower *= 1.75f;                              // 흔들었을 때 데미지 계수: 175%
                    break;

                case AttackType.ThrowingAtk:
                    data.attackPower = 100f + (data.attackPower * 0.03f);   // 흔들었을 때 고정 100 + 남은 음료 1ml 당 3%
                    data.atkRange = 4f;                                     // 흔들었을 때 범위 (반지름): 4
                    break;

                case AttackType.SprayAtk:
                    data.ammoCost = 25;                                     // 흔들었을 때 음료 소비량: 25ml (틱 당)
                    data.attackPower *= 0.5f;                               // 흔들었을 때 데미지 계수: 50% (틱 당)
                    break;

                case AttackType.ContinuousAtk:
                    data.ammoCost = 25;                                     // 흔들었을 때 음료 소비량: 25ml (틱 당)
                    data.attackPower *= 0.5f;                               // 흔들었을 때 데미지 계수: 50% (틱 당)
                    break;

                case AttackType.RangedAtk:
                    data.ammoCost = 110;                                    // 흔들었을 때 음료 소비량: 110ml (발 당)
                    data.attackPower *= 3.5f;                               // 흔들었을 때 데미지 계수: 350% (발 당)
                    break;
            }

            Debug.Log($"{attackType} 강화 완료: 탄약 소비량={data.ammoCost}, 데미지={data.attackPower}");
        }
    }

    private void ResetAttackStats()
    {
        if (playerAttack == null || playerAttack.attackStatusDict == null) return;

        // 강화된 스탯을 원래 값으로 복원
        foreach (var attackType in playerAttack.attackStatusDict.Keys)
        {
            AttackStateData data = playerAttack.attackStatusDict[attackType];

            switch (attackType)
            {
                case AttackType.NormalAtk:
                    data.ammoCost = 50;                                     // 원래 음료 소비량: 50ml
                    data.attackPower /= 1.75f;                              // 원래 데미지 계수: 150%
                    break;

                case AttackType.ThrowingAtk:
                    data.attackPower = (data.attackPower - 100f) / 0.03f;   // 원래 고정 100 + 남은 음료 1ml 당 2.5%
                    data.atkRange = 3f;                                     // 원래 범위 (반지름): 3
                    break;

                case AttackType.SprayAtk:
                    data.ammoCost = 20;                                     // 원래 음료 소비량: 20ml (틱 당)
                    data.attackPower /= 0.5f;                               // 원래 데미지 계수: 40% (틱 당)
                    break;

                case AttackType.ContinuousAtk:
                    data.ammoCost = 20;                                     // 원래 음료 소비량: 20ml (틱 당)
                    data.attackPower /= 0.5f;                               // 원래 데미지 계수: 40% (틱 당)
                    break;

                case AttackType.RangedAtk:
                    data.ammoCost = 100;                                    // 원래 음료 소비량: 100ml (발 당)
                    data.attackPower /= 3.5f;                               // 원래 데미지 계수: 300% (발 당)
                    break;
            }

            Debug.Log($"{attackType} 스탯 복원: 탄약 소비량={data.ammoCost}, 데미지={data.attackPower}");
        }
    }
}