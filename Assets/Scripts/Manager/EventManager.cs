using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public event Action<int> Item;       //������ ȹ�� �� �߻��� �̺�Ʈ

    public void ItemInput()
    {
        
    }
}
