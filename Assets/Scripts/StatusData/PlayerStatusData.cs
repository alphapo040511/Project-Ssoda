using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusData", menuName = "Scriptable Object/StatusData", order = int.MaxValue)]
public class PlayerStatusData : ScriptableObject
{
    public float HealthPoint;       //체력
    public float AttackPower;       //공격력
    public float AttackSpeed;       //공격 속도
    public float CritChance;        //치명타 확률
    public float CritDamage;        //치명타 피해량
    public float MoveSpeed;         //이동 속도
    public float dashDistance;      // 대쉬 거리
    public float dashDuration;      // 무적 시간
}