using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusData", menuName = "Scriptable Object/StatusData", order = int.MaxValue)]
public class PlayerStatusData : ScriptableObject
{
    public int MaxAmmo;             // �ѿ뷮
    public float HealthPoint;       // ü��
    public float CritChance;        // ġ��Ÿ Ȯ��
    public float CritDamage;        // ġ��Ÿ ���ط�
    public float MoveSpeed;         // �̵� �ӵ�
    public float dashDistance;      // �뽬 �Ÿ�
    public float dashDuration;      // ���� �ð�
    public float dashCooldown;      // ��� ��ٿ� (��)
}