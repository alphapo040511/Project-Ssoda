using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerState ��� �÷��̾� ������ �⺻�� �Ǵ� �߻� Ŭ����
public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;          // ���� �ӽſ� ���� ���� (���� ����)
    protected PlayerController playerController;        // �÷��̾� ��Ʈ�ѷ��� ���� ����
    protected PlayerAnimationManager animationManager;  // �ִϸ��̼� �Ŵ����� �����´�.

    // ������ : ���� �ӽŰ� �÷��̾� ��Ʈ�ѷ� ���� �ʱ�ȭ
    public PlayerState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.playerController = stateMachine.PlayerController;
        this.animationManager = stateMachine.GetComponent<PlayerAnimationManager>();
    }

    // ���� �޼���� : ���� Ŭ�������� �ʿ信 ���� �������̵�
    public virtual void Enter() { }             // ���� ���� �� ȣ��
    public virtual void Exit() { }              // ���� ���� �� ȣ��  
    public virtual void Update() { }            // �� ������ ȣ��
    public virtual void FixedUpdate() { }       // ���� �ð� �������� ȣ�� (���� �����)

    // ���� ��ȣ�� ������ üũ�ϴ� �޼���
    protected void CheckTransitions()
    {
        if (playerController.isGrounded())
        {
            // ���� ���� ���� ���� ��ȯ ����
            if (Input.GetKeyDown(KeyCode.Space))        // �����̽��� ��������
            {
                stateMachine.TransitionToState(new DashingState(stateMachine));
            }
            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)    // �̵�Ű�� ��������
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
        CheckTransitions();                 // �� �����Ӹ��� ���� ��ȯ ���� üũ
    }
}

public class RunningState : PlayerState
{
    public RunningState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        // �̵� �Է� Ȯ�� (WASD �Ǵ� ����Ű)
        bool isRunning = playerController.playerInput.moveX != 0 || playerController.playerInput.moveY != 0;

        if (!isRunning) // �̵� �Է��� ������ Idle ���·� ��ȯ
        {
            stateMachine.TransitionToState(new IdleState(stateMachine));
        }

        CheckTransitions();  // ���� ��ȯ üũ (��: ��� ���� ��)
    }

    public override void FixedUpdate()
    {
        playerController.HandleMoveAndRotate();  // ���� ��� �̵� ó��
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
            stateMachine.TransitionToState(new IdleState(stateMachine)); // ��� ���� �� Idle ���·�
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