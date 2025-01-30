using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Data")]
    public EnemyData enemyData;  // ���� Ÿ�� �� ��ũ���ͺ� ������Ʈ

    private int currentHealth;
    private Animator animator;
    private bool isDead = false;

    // ���� ���� (���, �̵�, ����, �ǰ�, ���)
    protected enum EnemyState
    {
        Idle,
        Moving,
        Attacking,
        Damaged,
        Dead
    }
    protected EnemyState currentState;

    // �ʱ�ȭ �� ������ ����
    protected virtual void Start()
    {
        if (enemyData != null)
        {
            currentHealth = enemyData.maxHealth;
            animator = GetComponent<Animator>();

            if (animator != null && enemyData.animatorController != null)
            {
                animator.runtimeAnimatorController = enemyData.animatorController;
            }
        }
    }

    // ������Ʈ ó��
    protected virtual void Update()
    {
        if (isDead)     // �濡 �������� �ʾ������� �����ϵ��� ���߿� ���� �ʿ� <<<<<<<<<<
            return;

        HandleEnemyBehavior();
    }

    // ������ �ൿ�� ó���ϴ� �⺻���� �Լ�
    protected virtual void HandleEnemyBehavior()
    {
        if (currentState == EnemyState.Idle)
        {
            // �ϴ� ����ΰ�
        }
        else if (currentState == EnemyState.Moving)
        {
            // �÷��̾� ���ݺ���
        }
        else if (currentState == EnemyState.Attacking)
        {
            // ������ ������
        }
        else if (currentState == EnemyState.Damaged)
        {
            // ���� ������ ����
        }
    }

    // ���Ͱ� �������� �޾��� �� ó��
    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        // �ǰ� �ִϸ��̼� Ʈ����
        if (animator != null)
        {
            animator.SetTrigger("TakeDamage");  // �ϴ� Ʈ���ŷ� �صδµ� �� �����ϰ� �� ������ ����� ����� �����ϴ°ɷ�
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // �ǰ� ���·� ����
            currentState = EnemyState.Damaged;
        }
    }

    // ���Ͱ� �׾��� �� ó��
    protected virtual void Die()
    {
        isDead = true;
        currentState = EnemyState.Dead;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }

    // ���Ͱ� ������ ��
    public void Attack()
    {
        if (isDead)
            return;

        currentState = EnemyState.Attacking;

        // ���� ���� �߰��ؾ��� �ϴ� �÷��̾� ���ݺ���
    }
}
