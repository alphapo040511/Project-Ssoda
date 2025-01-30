using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Data")]
    public EnemyData enemyData;  // 몬스터 타입 별 스크립터블 오브젝트

    private int currentHealth;
    private Animator animator;
    private bool isDead = false;

    // 몬스터 상태 (대기, 이동, 공격, 피격, 사망)
    protected enum EnemyState
    {
        Idle,
        Moving,
        Attacking,
        Damaged,
        Dead
    }
    protected EnemyState currentState;

    // 초기화 및 데이터 설정
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

    // 업데이트 처리
    protected virtual void Update()
    {
        if (isDead)     // 방에 입장하지 않았을때도 리턴하도록 나중에 수정 필요 <<<<<<<<<<
            return;

        HandleEnemyBehavior();
    }

    // 몬스터의 행동을 처리하는 기본적인 함수
    protected virtual void HandleEnemyBehavior()
    {
        if (currentState == EnemyState.Idle)
        {
            // 일단 비워두고
        }
        else if (currentState == EnemyState.Moving)
        {
            // 플레이어 공격부터
        }
        else if (currentState == EnemyState.Attacking)
        {
            // 구현한 다음에
        }
        else if (currentState == EnemyState.Damaged)
        {
            // 몬스터 구현할 예정
        }
    }

    // 몬스터가 데미지를 받았을 때 처리
    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        // 피격 애니메이션 트리거
        if (animator != null)
        {
            animator.SetTrigger("TakeDamage");  // 일단 트리거로 해두는데 더 공부하고 더 적합한 방법이 생기면 수정하는걸로
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // 피격 상태로 변경
            currentState = EnemyState.Damaged;
        }
    }

    // 몬스터가 죽었을 때 처리
    protected virtual void Die()
    {
        isDead = true;
        currentState = EnemyState.Dead;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }

    // 몬스터가 공격할 때
    public void Attack()
    {
        if (isDead)
            return;

        currentState = EnemyState.Attacking;

        // 공격 로직 추가해야함 일단 플레이어 공격부터
    }
}
