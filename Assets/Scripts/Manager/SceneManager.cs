using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    //스테이지 끝난거 판단해서 가져오기
    public bool isDone;     //게임이 끝났는지
    public bool isRoom;     //방에 들어감 , 방에 들어갔을 때 = true

    void Start()
    {
        isDone = false;
        isRoom = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
