using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackStatusData", menuName = "Scriptable Object/AttackStatusData", order = int.MaxValue)]
public class AttackStateData : ScriptableObject
{
    public float atkCooldown;               // 공격 속도 (초당)
    public float atkRange;                  // 사거리
    public float projectileThickness;       // 발사체 두께
    public float lastAttackTime;
    public float projectileSpeed;           // 발사체 속도
    public float projectileLifetime;        // 발사체 생존 시간
}
