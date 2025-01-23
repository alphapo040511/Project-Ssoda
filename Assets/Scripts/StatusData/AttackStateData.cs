using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackStatusData", menuName = "Scriptable Object/AttackStatusData", order = int.MaxValue)]
public class AttackStateData : ScriptableObject
{
    public float atkCooldown;               // ���� �ӵ� (�ʴ�)
    public float atkRange;                  // ��Ÿ�
    public float projectileThickness;       // �߻�ü �β�
    public float lastAttackTime;
    public float projectileSpeed;           // �߻�ü �ӵ�
    public float projectileLifetime;        // �߻�ü ���� �ð�
}
