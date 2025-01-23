using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackStatusData", menuName = "Scriptable Object/AttackStatusData", order = int.MaxValue)]
public class AttackStateData : ScriptableObject
{
    public float atkCooldown = 0.5f;            // ���� �ӵ� (�ʴ�)
    public float atkRange = 10f;                // ��Ÿ�
    public float projectileThickness = 0.8f;    // �߻�ü �β�
    public float lastAttackTime = 0f;
    public float projectileSpeed = 10f;         // �߻�ü �ӵ�
    public float projectileLifetime = 5f;       // �߻�ü ���� �ð�
}
