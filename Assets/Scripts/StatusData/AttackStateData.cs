using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    NormalAtk,          // 기본 공격
    ThrowingAtk,        // 투척 공격
    MeleeAtk,           // 근접 공격
    SprayAtk,           // 분사형 공격
    ContinuousAtk,      // 연사 공격
    RangedAtk           // 원거리 광역 공격
}

[CreateAssetMenu(fileName = "AttackStatusData", menuName = "Scriptable Object/AttackStatusData", order = int.MaxValue)]
public class AttackStateData : ScriptableObject
{
    public AttackType attackType;           // 공격 타입
    public int ammoCost;                    // 탄약 소모량
    public float attackPower;               // 공격력
    public float atkCooldown;               // 공격 속도 (초당)
    public float atkRange;                  // 사거리
    public float projectileThickness;       // 발사체 두께
    public float projectileSpeed;           // 발사체 속도
    public float projectileLifetime;        // 발사체 생존 시간
}