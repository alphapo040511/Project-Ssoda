using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerReload : MonoBehaviour
{
    public PlayerStatusData playerStatus;       // 플레이어 스탯 데이터 참조

    [Header("Ammo")]
    public int currentAmmo;                     // 현재 탄약
    public int maxReserves = 2;                 // 최대 예비 탄약 수
    public int currentReserves;                 // 현재 예비 탄약 수

    [Header("Charge")]
    public float reserveChargeRate = 0.1f;      // 10%씩 충전
    public float chargeInterval = 3f;           // 3초마다 충전
    private float[] reserveCharge;              // 예비 병 충전 상태 (0.0 ~ 1.0)

    private void Start()
    {
        // 초기화: 현재 탄약을 최대 탄약으로 설정, 예비 탄약을 최대로 설정
        currentAmmo = playerStatus.MaxAmmo;
        currentReserves = maxReserves;

        // 예비 병 충전 상태 초기화
        reserveCharge = new float[maxReserves];
        for (int i = 0; i < maxReserves; i++)
        {
            reserveCharge[i] = 0f; // 처음에는 모두 0%
        }

        // 충전 코루틴 시작
        StartCoroutine(ChargeReserves());
    }

    private IEnumerator ChargeReserves()
    {
        while (true)
        {
            yield return new WaitForSeconds(chargeInterval);

            for (int i = 0; i < maxReserves; i++)
            {
                if (reserveCharge[i] < 1f) // 아직 가득 차지 않은 병
                {
                    reserveCharge[i] += reserveChargeRate; // 10% 충전
                    Debug.Log($"예비 병 {i + 1} 충전 중: {reserveCharge[i] * 100}%"); // 충전 상태 출력

                    if (reserveCharge[i] >= 1f) // 가득 찬 경우
                    {
                        reserveCharge[i] = 1f;
                        Debug.Log($"예비 병 {i + 1}이 가득 찼습니다.");
                    }
                    break; // 한 번에 하나의 병만 충전
                }
            }
        }
    }

    public void Reload()
    {
        // 손에 든 병에 음료가 남았을 경우
        if (currentAmmo > 0)
        {
            Debug.Log("병 교체 불가: 손에 든 병에 음료가 남아 있습니다.");
            return;
        }

        // 손에 든 병이 비었을 경우
        if (currentAmmo <= 0)
        {
            // 가득 찬 예비 병이 있는지 확인
            for (int i = 0; i < maxReserves; i++)
            {
                if (reserveCharge[i] >= 1f) // 가득 찬 병이 있는 경우
                {
                    // 현재 탄약을 최대로 채우고, 예비 병 충전 상태 초기화
                    currentAmmo = playerStatus.MaxAmmo;
                    reserveCharge[i] = 0f;
                    Debug.Log($"재장전 완료! 현재 탄약: {currentAmmo}, 예비 병 {i + 1} 사용");
                    return;
                }
            }

            // 가득 찬 병이 없을 경우
            Debug.Log("재장전 불가: 가득 찬 예비 병이 없습니다.");

            // 투척으로 병을 들고 있지 않을 경우
            if (currentAmmo <= 0 && currentReserves <= 0)
            {
                Debug.Log("빈 병을 새로 꺼내 손에 듭니다.");
                currentAmmo = 0; // 빈 병을 들고 있음
            }
        }
    }

    public bool UseAmmo(AttackType attackType, Dictionary<AttackType, AttackStateData> attackStatusDict)
    {
        if (attackStatusDict.TryGetValue(attackType, out AttackStateData data))
        {
            int ammoCost = data.ammoCost;

            if (currentAmmo >= ammoCost)
            {
                currentAmmo -= ammoCost;
                Debug.Log($"탄약 사용: {ammoCost}, 남은 탄약: {currentAmmo}");
                return true; // 탄약 소모 성공
            }
            else
            {
                Debug.Log("탄약 부족!");
                return false; // 탄약 부족
            }
        }

        Debug.LogError($"공격 타입 {attackType}에 대한 데이터를 찾을 수 없습니다.");
        return false;
    }
}