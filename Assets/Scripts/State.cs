using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public string name { get; protected set; }                      // �̸�
    public int maxHp { get; protected set; }                        // �ִ� ü��
    public int nowHp { get; set; }                                  // ���� ü��
    public int atkDmg { get; protected set; }                       // ���� ������
    public float moveSpeed { get; protected set; }                  // ������ �ӵ�
    public float atkCooldown { get; protected set; }                // ���� �ӵ�
    public float atkRange { get; protected set; }                   // ���� ����
    public float projectileThickness { get; protected set; }        // �߻�ü �β�

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

    // ���� �ൿ �޼��� (�ʿ信 ���� �߰�)
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
        // �⺻ ��� ó�� ���� (��ӹ޴� Ŭ�������� �������̵� ����)
        Debug.Log($"{name} ���");
    }
}

