using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour
{
    [Header("음료 정보")]
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;

    [Header("음료 버프 목록")]
    [SerializeField] private List<BuffData> buffDatas;

    [Header("추가 되는 공격 방식")]
    [SerializeField] private AttackType weaponTypes;
}
