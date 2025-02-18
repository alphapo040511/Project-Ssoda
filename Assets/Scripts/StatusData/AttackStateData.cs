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

    [Header("발사체 공격")]
    public bool useProjectile;              // 발사체 사용 여부
    public float projectileThickness;       // 발사체 두께
    public float projectileSpeed;           // 발사체 속도

    [Header("범위 공격")]
    public bool isAreaAttack;               // 범위 공격 여부
    public float areaWidth;                 // 공격 너비
    public float attackAngle;               // 공격 각도

    [Header("지속 피해 옵션")]
    public bool isContinuousAttack;         // 지속 공격 여부
    public float damageInterval;            // 몇 초마다 데미지를 줄 것인지

    [Header("투척 공격 옵션")]
    public bool isThrowingAttack;           // 투척 공격 여부
    public float explosionDelay;            // 투척 공격이 터지기까지 걸리는 시간
}