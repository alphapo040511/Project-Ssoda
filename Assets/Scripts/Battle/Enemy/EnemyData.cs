using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Object/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int maxHealth;
    public int damage;
    public float attackSpeed;
    public float moveSpeed;
    public RuntimeAnimatorController animatorController; // ���͸��� �ִϸ��̼� Ŭ���� �ٸ��⿡
}
