using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerController playerController;
    protected PlayerAnimationManager animationManager;

    public PlayerState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.playerController = stateMachine.PlayerController;
        this.animationManager = stateMachine.GetComponent<PlayerAnimationManager>();
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }

    protected void CheckTransitions()
    {
        if (playerController.isGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                stateMachine.TransitionToState(new DashingState(stateMachine));
            }
            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
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
        CheckTransitions();
    }
}

public class RunningState : PlayerState
{
    public RunningState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        bool isRunning = playerController.playerInput.moveX != 0 || playerController.playerInput.moveY != 0;

        if (!isRunning)
        {
            stateMachine.TransitionToState(new IdleState(stateMachine));
        }

        CheckTransitions();
    }

    public override void FixedUpdate()
    {
        playerController.HandleMoveAndRotate();
    }
}

public class DashingState : PlayerState
{
    public DashingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void FixedUpdate()
    {
        if (!playerController.isDashing)
        {
            stateMachine.TransitionToState(new IdleState(stateMachine));
        }
    }
}