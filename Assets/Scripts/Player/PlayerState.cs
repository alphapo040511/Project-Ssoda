using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerState 모든 플레이어 상태의 기본이 되는 추상 클래스
public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;          // 상태 머신에 대한 참조 (이후 구현)
    protected PlayerController playerController;        // 플레이어 컨트롤러에 대한 참조
    protected PlayerAnimationManager animationManager;  // 애니메이션 매니저를 가져온다.

    // 생성자 : 상태 머신과 플레이어 컨트롤러 참조 초기화
    public PlayerState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.playerController = stateMachine.PlayerController;
        this.animationManager = stateMachine.GetComponent<PlayerAnimationManager>();
    }

    // 가상 메서드들 : 하위 클래스에서 필요에 따라 오버라이드
    public virtual void Enter() { }             // 상태 진입 시 호출
    public virtual void Exit() { }              // 상태 종료 시 호출  
    public virtual void Update() { }            // 매 프레임 호출
    public virtual void FixedUpdate() { }       // 고정 시간 간격으로 호출 (물리 연산용)

    // 상태 전호나 조건을 체크하는 메서드
    protected void CheckTransitions()
    {
        if (playerController.isGrounded())
        {
            // 지상에 있을 때의 상태 전환 로직
            if (Input.GetKeyDown(KeyCode.Space))        // 스페이스를 눌렀을때
            {
                stateMachine.TransitionToState(new DashingState(stateMachine));
            }
            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)    // 이동키가 눌렸을때
            {
                stateMachine.TransitionToState(new RunningState(stateMachine));
            }
            else
            {
                stateMachine.TransitionToState(new IdleState(stateMachine));
            }
        }
    }
}

public class IdleState : PlayerState
{
    public IdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        CheckTransitions();                 // 매 프레임마다 상태 전환 조건 체크
    }
}

public class RunningState : PlayerState
{
    public RunningState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        // 이동 입력 확인 (WASD 또는 방향키)
        bool isRunning = playerController.playerInput.moveX != 0 || playerController.playerInput.moveY != 0;

        if (!isRunning) // 이동 입력이 없으면 Idle 상태로 전환
        {
            stateMachine.TransitionToState(new IdleState(stateMachine));
        }

        CheckTransitions();  // 상태 전환 체크 (예: 대시 여부 등)
    }

    public override void FixedUpdate()
    {
        playerController.HandleMoveAndRotate();  // 물리 기반 이동 처리
    }
}

public class DashingState : PlayerState
{
    private bool hasDashed = false;

    public DashingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        animationManager.animator.SetBool("IsDashing", true);
        hasDashed = false;
        playerController.StartDash();
    }

    public override void Update()
    {
        if (!playerController.isDashing && hasDashed)
        {
            stateMachine.TransitionToState(new IdleState(stateMachine)); // 대시 종료 후 Idle 상태로
        }
    }

    public override void FixedUpdate()
    {
        if (!hasDashed)
        {
            hasDashed = true;
        }
    }

    public override void Exit()
    {
        animationManager.animator.SetBool("IsRolling", false);
    }
}