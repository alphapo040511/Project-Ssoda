using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public event Action<int> Item;       //아이템 획득 시 발생할 이벤트

    public void ItemInput()
    {
        
    }
}
