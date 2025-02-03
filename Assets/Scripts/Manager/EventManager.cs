using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public event Action<bool> OnSceneChanger;       //씬 넘길 때 사용할 이벤트
    public event Action<bool> OnNextStage;       //다음 스테이지로 넘어갈 때 사용할 이벤트

    public void SceneChange()       //게임 시작, 종료, 상점, 설정창 등 들어갈 때
    {
        
    }

    public void NextStage()         //다음 스테이지로 넘어갈 때 ( 몬스터 생성 및 플레이어 이동 )
    {

    }
}
