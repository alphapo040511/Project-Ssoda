using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    NormalAtk,          // �⺻ ����
    ThrowingAtk,        // ��ô ����
    MeleeAtk,           // ���� ����
    SprayAtk,           // �л��� ����
    ContinuousAtk,      // ���� ����
    RangedAtk           // ���Ÿ� ���� ����
}

[CreateAssetMenu(fileName = "AttackStatusData", menuName = "Scriptable Object/AttackStatusData", order = int.MaxValue)]
public class AttackStateData : ScriptableObject
{
    public AttackType attackType;           // ���� Ÿ��
    public int ammoCost;                    // ź�� �Ҹ�
    public float attackPower;               // ���ݷ�
    public float atkCooldown;               // ���� �ӵ� (�ʴ�)
    public float atkRange;                  // ��Ÿ�
    public float projectileThickness;       // �߻�ü �β�
    public float projectileSpeed;           // �߻�ü �ӵ�
    public float projectileLifetime;        // �߻�ü ���� �ð�
}