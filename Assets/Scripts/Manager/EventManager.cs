using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public event Action<bool> OnSceneChanger;       //�� �ѱ� �� ����� �̺�Ʈ
    public event Action<bool> OnNextStage;       //���� ���������� �Ѿ �� ����� �̺�Ʈ

    public void SceneChange()       //���� ����, ����, ����, ����â �� �� ��
    {
        
    }

    public void NextStage()         //���� ���������� �Ѿ �� ( ���� ���� �� �÷��̾� �̵� )
    {

    }
}
