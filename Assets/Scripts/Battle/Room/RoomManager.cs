using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;  // �̱��� ���� ����

    [Header("��� �� ����Ʈ")]
    public List<BattleRoom> battleRooms = new List<BattleRoom>(); // ��� �� ����

    private BattleRoom currentRoom;  // �÷��̾ ���� ��ġ�� ��

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // �÷��̾ ���ο� �濡 ������ �� ȣ��
    public void EnterRoom(BattleRoom room)
    {
        currentRoom = room;
        currentRoom.EnterRoom(); // �� �� Ȱ��ȭ
    }

    // Ư�� ���� Ŭ����Ǿ��� �� ȣ��
    public void RoomCleared(BattleRoom room)
    {
        if (battleRooms.Contains(room))
        {
            Debug.Log($"�� {battleRooms.IndexOf(room)} Ŭ����!");
            room.isCleared = true;  // �ش� �� Ŭ���� ���� ����
        }

        // ��� ���� Ŭ����Ǿ����� üũ
        CheckAllRoomsCleared();
    }

    // ��� ���� Ŭ����Ǿ����� Ȯ��
    private void CheckAllRoomsCleared()
    {
        foreach (var room in battleRooms)
        {
            if (!room.isCleared)
                return;
        }

        Debug.Log("��� ���� Ŭ����Ǿ����ϴ�!");
        // ��: ���� �� Ȱ��ȭ, �� �ر� �� �߰� ó�� ����
    }
}
