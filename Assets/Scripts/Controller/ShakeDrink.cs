using UnityEngine;
using System.Collections;

public class ShakeDrink : MonoBehaviour
{
    private PlayerAttack playerAttack;

    private bool isShaking = false;         // ���� ������ ����
    private float shakeDuration = 10f;      // ��ȭ ���� �ð� (10��)

    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }

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
        Debug.Log("���Ḧ ���� ���� ����� ��ȭ�մϴ�!");

        // ���� ��� �ִ� ������ ���� Ÿ�Ը� ��ȭ
        EnhanceAttackStats();

        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            Debug.Log($"���� ȿ�� ���� ��... {shakeDuration - elapsedTime:F1}�� ����");
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }

        // ��ȭ ȿ�� ����
        ResetAttackStats();

        isShaking = false;

        Debug.Log("���� ȿ���� ����Ǿ����ϴ�.");
    }

    private void EnhanceAttackStats()
    {
        if (playerAttack == null || playerAttack.attackStatusDict == null) return;

        // ���� ��� �ִ� ������ ���� Ÿ�Ը� ��ȭ
        AttackType currentWeaponType = playerAttack.currentWeaponType;
        if (playerAttack.attackStatusDict.ContainsKey(currentWeaponType))
        {
            AttackStateData data = playerAttack.attackStatusDict[currentWeaponType];

            switch (currentWeaponType)
            {
                case AttackType.NormalAtk:
                    data.ammoCost = 55;                                         // ������ �� ���� �Һ�: 55ml
                    data.attackPower *= 1.1667f;                                // ������ �� ������ ���: 175%
                    break;

                case AttackType.ThrowingAtk:
                    data.attackPower = 100f + (data.attackPower * 0.03f);       // ������ �� ���� 100 + ���� ���� 1ml �� 3%
                    data.projectileThickness = 4f;                              // ������ �� ���� (������): 4
                    break;

                case AttackType.SprayAtk:
                    data.ammoCost = 25;                                         // ������ �� ���� �Һ�: 25ml (ƽ ��)
                    data.attackPower *= 1.25f;                                  // ������ �� ������ ���: 50% (ƽ ��)
                    break;

                case AttackType.ContinuousAtk:
                    data.ammoCost = 25;                                         // ������ �� ���� �Һ�: 25ml (ƽ ��)
                    data.attackPower *= 1.25f;                                  // ������ �� ������ ���: 50% (ƽ ��)
                    break;

                case AttackType.RangedAtk:
                    data.ammoCost = 110;                                        // ������ �� ���� �Һ�: 110ml (�� ��)
                    data.attackPower *= 1.1667f;                                // ������ �� ������ ���: 350% (�� ��)
                    break;
            }

            Debug.Log($"{currentWeaponType} ��ȭ �Ϸ�: ź�� �Һ�={data.ammoCost}, ������={data.attackPower}");
        }
    }

    private void ResetAttackStats()
    {
        if (playerAttack == null || playerAttack.attackStatusDict == null) return;

        // ���� ��� �ִ� ������ ���� Ÿ�Ը� ����
        AttackType currentWeaponType = playerAttack.currentWeaponType;
        if (playerAttack.attackStatusDict.ContainsKey(currentWeaponType))
        {
            AttackStateData data = playerAttack.attackStatusDict[currentWeaponType];

            switch (currentWeaponType)
            {
                case AttackType.NormalAtk:
                    data.ammoCost = 50;                                         // ���� ���� �Һ�: 50ml
                    data.attackPower /= 1.1667f;                                // ���� ������ ���: 150%
                    break;

                case AttackType.ThrowingAtk:
                    data.attackPower = (data.attackPower - 100f) / 0.03f;       // ���� ���� 100 + ���� ���� 1ml �� 2.5%
                    data.projectileThickness = 3f;                              // ���� ���� (������): 3
                    break;  

                case AttackType.SprayAtk:
                    data.ammoCost = 20;                                         // ���� ���� �Һ�: 20ml (ƽ ��)
                    data.attackPower /= 1.25f;                                  // ���� ������ ���: 40% (ƽ ��)
                    break;

                case AttackType.ContinuousAtk:
                    data.ammoCost = 20;                                         // ���� ���� �Һ�: 20ml (ƽ ��)
                    data.attackPower /= 1.25f;                                  // ���� ������ ���: 40% (ƽ ��)
                    break;

                case AttackType.RangedAtk:
                    data.ammoCost = 100;                                        // ���� ���� �Һ�: 100ml (�� ��)
                    data.attackPower /= 1.1667f;                                // ���� ������ ���: 300% (�� ��)
                    break;
            }

            Debug.Log($"{currentWeaponType} ���� ����: ź�� �Һ�={data.ammoCost}, ������={data.attackPower}");
        }
    }
}