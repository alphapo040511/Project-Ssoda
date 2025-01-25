using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StatusType
{
    HealthPoint,        //체력
    AttackPower,        //공격력
    AttackSpeed,        //공속
    CritChance,         //치명타 확률
    CritDamage,         //치명타 피해량
    DamageBonus,        //피해 보너스
    MoveSpeed,          //이동 속도
    ReloadSpeed,        //재장전 속도
    GoldBonus,          //획득 재화량 증가
}

[System.Serializable]
public class BuffData : MonoBehaviour
{
    [Header("버프 종류")]
    public StatusType buffType; 

    [Header("버프 량 (%)")]
    public float buffAmount;
    public float healthPoint;
    public float attackPower;
    public float attackSpeed;
    public float critChance;
    public float critDamage;
    public float damageBonus;
    public float moveSpeed;
    public float reloadSpeed;
    public float goldBonus; 
}
