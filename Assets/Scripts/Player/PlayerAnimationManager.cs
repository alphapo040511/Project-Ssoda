using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public Animator animator;
    public PlayerStateMachine stateMachine;

    // 애니메이션 파라미터 이름들을 상수로 정의
    private const string PARAM_IS_RUNNING = "IsRunning";
    private const string PARAM_IS_DASHING = "IsDashing";

    void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        if (stateMachine.currentState != null)
        {
            ResetAllBoolParameters();

            switch (stateMachine.currentState)
            {
                case IdleState:
                    break;
                case RunningState:
                    animator.SetBool(PARAM_IS_RUNNING, true);
                    break;
                case DashingState:
                    animator.SetBool(PARAM_IS_DASHING, true);
                    break;
            }
        }
    }

    // 모든 bool 파라미터를 초기화 해주는 함수
    private void ResetAllBoolParameters()
    {
        animator.SetBool(PARAM_IS_RUNNING, false);
        animator.SetBool(PARAM_IS_DASHING, false);
    }
}