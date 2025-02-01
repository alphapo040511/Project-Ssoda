using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerReload : MonoBehaviour
{
    public PlayerStatusData playerStatus;       // �÷��̾� ���� ������ ����

    [Header("Ammo")]
    public int currentAmmo;                     // ���� ź��
    public int maxReserves = 2;                 // �ִ� ���� ź�� ��
    public int currentReserves;                 // ���� ���� ź�� ��

    [Header("Charge")]
    public float reserveChargeRate = 0.1f;      // 10%�� ����
    public float chargeInterval = 3f;           // 3�ʸ��� ����
    private float[] reserveCharge;              // ���� �� ���� ���� (0.0 ~ 1.0)

    private void Start()
    {
        // �ʱ�ȭ: ���� ź���� �ִ� ź������ ����, ���� ź���� �ִ�� ����
        currentAmmo = playerStatus.MaxAmmo;
        currentReserves = maxReserves;

        // ���� �� ���� ���� �ʱ�ȭ
        reserveCharge = new float[maxReserves];
        for (int i = 0; i < maxReserves; i++)
        {
            reserveCharge[i] = 0f; // ó������ ��� 0%
        }

        // ���� �ڷ�ƾ ����
        StartCoroutine(ChargeReserves());
    }

    private IEnumerator ChargeReserves()
    {
        while (true)
        {
            yield return new WaitForSeconds(chargeInterval);

            for (int i = 0; i < maxReserves; i++)
            {
                if (reserveCharge[i] < 1f) // ���� ���� ���� ���� ��
                {
                    reserveCharge[i] += reserveChargeRate; // 10% ����
                    Debug.Log($"���� �� {i + 1} ���� ��: {reserveCharge[i] * 100}%"); // ���� ���� ���

                    if (reserveCharge[i] >= 1f) // ���� �� ���
                    {
                        reserveCharge[i] = 1f;
                        Debug.Log($"���� �� {i + 1}�� ���� á���ϴ�.");
                    }
                    break; // �� ���� �ϳ��� ���� ����
                }
            }
        }
    }

    public void Reload()
    {
        // �տ� �� ���� ���ᰡ ������ ���
        if (currentAmmo > 0)
        {
            Debug.Log("�� ��ü �Ұ�: �տ� �� ���� ���ᰡ ���� �ֽ��ϴ�.");
            return;
        }

        // �տ� �� ���� ����� ���
        if (currentAmmo <= 0)
        {
            // ���� �� ���� ���� �ִ��� Ȯ��
            for (int i = 0; i < maxReserves; i++)
            {
                if (reserveCharge[i] >= 1f) // ���� �� ���� �ִ� ���
                {
                    // ���� ź���� �ִ�� ä���, ���� �� ���� ���� �ʱ�ȭ
                    currentAmmo = playerStatus.MaxAmmo;
                    reserveCharge[i] = 0f;
                    Debug.Log($"������ �Ϸ�! ���� ź��: {currentAmmo}, ���� �� {i + 1} ���");
                    return;
                }
            }

            // ���� �� ���� ���� ���
            Debug.Log("������ �Ұ�: ���� �� ���� ���� �����ϴ�.");

            // ��ô���� ���� ��� ���� ���� ���
            if (currentAmmo <= 0 && currentReserves <= 0)
            {
                Debug.Log("�� ���� ���� ���� �տ� ��ϴ�.");
                currentAmmo = 0; // �� ���� ��� ����
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
                Debug.Log($"ź�� ���: {ammoCost}, ���� ź��: {currentAmmo}");
                return true; // ź�� �Ҹ� ����
            }
            else
            {
                Debug.Log("ź�� ����!");
                return false; // ź�� ����
            }
        }

        Debug.LogError($"���� Ÿ�� {attackType}�� ���� �����͸� ã�� �� �����ϴ�.");
        return false;
    }
}