using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;

    [Header("���� ���� ���")]
    [SerializeField] private List<BuffData> buffDatas;

    [Header("�߰� �Ǵ� ���� ���")]
    [SerializeField] private AttackType weaponTypes;
}
