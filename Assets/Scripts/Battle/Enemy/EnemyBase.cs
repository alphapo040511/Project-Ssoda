using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Data")]
    public EnemyData enemyData;  // 몬스터 타입 별 스크립터블 오브젝트

    public float currentHealth;
    public BattleRoom room;            // 몬스터가 속한 방의 트리거
    private Animator animator;
    private bool isDead = false;
    private bool isActive = false;  // 플레이어가 방에 입장 했는지/안 했는지에 따른 몬스터 활성화 여부

    [Header("UI")]
    public Slider healthBar;

    // 몬스터 상태 (대기, 이동, 공격, 피격, 사망)
    protected enum EnemyState
    {
        Idle,
        Moving,
        Attacking,
        Damaged,
        Dead
    }
    protected EnemyState currentState = EnemyState.Idle;

    // 초기화 및 데이터 설정
    protected virtual void Start()
    {
        if (enemyData != null)
        {
            currentHealth = enemyData.maxHealth;
            healthBar.maxValue = currentHealth;
            healthBar.value = currentHealth;

            animator = GetComponent<Animator>();

            if (animator != null && enemyData.animatorController != null)
            {
                animator.runtimeAnimatorController = enemyData.animatorController;
            }
        }

        // 처음엔 비활성화 상태 (AI 작동 X, 이동 X)
        gameObject.SetActive(false);
    }

    // 업데이트 처리
    protected virtual void Update()
    {
        if (!isActive || isDead)
            return;

        // 테스트용 코드
        if (isActive)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.red; // 빨간색으로 변경
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        // 여기까지 테스트

        HandleEnemyBehavior();
    }

    // 전투 시작 시 활성화
    public void ActivateEnemy()
    {
        gameObject.SetActive(true); // 몬스터 활성화
        isActive = true;
        currentState = EnemyState.Idle;
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
    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        healthBar.value = currentHealth;

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

        // 현재 몬스터가 속한 방에 사망 알림
        if (room != null)
        {
            room.EnemyDefeated(this);
        }

        Destroy(gameObject);    // 임시로 파괴
    }

    // 몬스터가 공격할 때
    public void Attack()
    {
        if (isDead)
            return;

        currentState = EnemyState.Attacking;

        // 공격 로직 추가해야함 일단 플레이어 공격부터
    }

    public void SetRoom(BattleRoom battleRoom)
    {
        room = battleRoom;
    }
}
