using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StatusType
{
    HealthPoint,        //ü��
    AttackPower,        //���ݷ�
    AttackSpeed,        //����
    CritChance,         //ġ��Ÿ Ȯ��
    CritDamage,         //ġ��Ÿ ���ط�
    DamageBonus,        //���� ���ʽ�
    MoveSpeed,          //�̵� �ӵ�
    ReloadSpeed,        //������ �ӵ�
    GoldBonus,          //ȹ�� ��ȭ�� ����
}

[System.Serializable]
public class BuffData : MonoBehaviour
{
    [Header("���� ����")]
    public StatusType buffType; 

    [Header("���� �� (%)")]
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
