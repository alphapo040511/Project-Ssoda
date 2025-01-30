using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;  // 싱글톤 패턴 적용

    [Header("모든 방 리스트")]
    public List<BattleRoom> battleRooms = new List<BattleRoom>(); // 모든 방 관리

    private BattleRoom currentRoom;  // 플레이어가 현재 위치한 방

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // 플레이어가 새로운 방에 입장할 때 호출
    public void EnterRoom(BattleRoom room)
    {
        currentRoom = room;
        currentRoom.EnterRoom(); // 새 방 활성화
    }

    // 특정 방이 클리어되었을 때 호출
    public void RoomCleared(BattleRoom room)
    {
        if (battleRooms.Contains(room))
        {
            Debug.Log($"방 {battleRooms.IndexOf(room)} 클리어!");
            room.isCleared = true;  // 해당 방 클리어 상태 변경
        }

        // 모든 방이 클리어되었는지 체크
        CheckAllRoomsCleared();
    }

    // 모든 방이 클리어되었는지 확인
    private void CheckAllRoomsCleared()
    {
        foreach (var room in battleRooms)
        {
            if (!room.isCleared)
                return;
        }

        Debug.Log("모든 방이 클리어되었습니다!");
        // 예: 보스 방 활성화, 맵 해금 등 추가 처리 가능
    }
}
