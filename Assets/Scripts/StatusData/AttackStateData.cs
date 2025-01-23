using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackStatusData", menuName = "Scriptable Object/AttackStatusData", order = int.MaxValue)]
public class AttackStateData : ScriptableObject
{
    public float atkCooldown = 0.5f;            // 공격 속도 (초당)
    public float atkRange = 10f;                // 사거리
    public float projectileThickness = 0.8f;    // 발사체 두께
    public float lastAttackTime = 0f;
    public float projectileSpeed = 10f;         // 발사체 속도
    public float projectileLifetime = 5f;       // 발사체 생존 시간
}
