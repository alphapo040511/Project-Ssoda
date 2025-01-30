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
    public RuntimeAnimatorController animatorController; // 몬스터마다 애니메이션 클립이 다르기에
}
