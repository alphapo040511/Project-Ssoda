using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public string name { get; protected set; }                      // 이름
    public int maxHp { get; protected set; }                        // 최대 체력
    public int nowHp { get; set; }                                  // 현재 체력
    public int atkDmg { get; protected set; }                       // 공격 데미지
    public float moveSpeed { get; protected set; }                  // 움직임 속도
    public float atkCooldown { get; protected set; }                // 공격 속도
    public float atkRange { get; protected set; }                   // 공격 범위
    public float projectileThickness { get; protected set; }        // 발사체 두께

    public State(string name, int maxHp, int atkDmg, float moveSpeed, float attackCooldown, float atkRange, float projectileThickness)
    {
        this.name = name;
        this.maxHp = maxHp;
        this.nowHp = maxHp;
        this.atkDmg = atkDmg;
        this.moveSpeed = moveSpeed;
        this.atkCooldown = attackCooldown;
        this.atkRange = atkRange;
        this.projectileThickness = projectileThickness;
    }

    // 공통 행동 메서드 (필요에 따라 추가)
    public virtual void TakeDamage(int damage)
    {
        nowHp -= damage;
        if (nowHp <= 0)
        {
            nowHp = 0;
            Die();
        }
    }

    protected virtual void Die()
    {
        // 기본 사망 처리 로직 (상속받는 클래스에서 오버라이드 가능)
        Debug.Log($"{name} 사망");
    }
}

