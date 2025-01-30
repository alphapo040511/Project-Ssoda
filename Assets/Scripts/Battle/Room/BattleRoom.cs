using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : MonoBehaviour
{
    [Header("방에 속한 몬스터 리스트")]
    public List<EnemyBase> enemies = new List<EnemyBase>(); // 방에 있는 몬스터들
    public bool isCleared = false;  // 방 클리어 여부

    [Header("방의 입출구")]
    public GameObject entryDoor;  // 입구 문
    public GameObject exitDoor;   // 출구 문

    // 플레이어가 방에 들어올 때 호출
    public void EnterRoom()
    {
        if (isCleared) return;  // 이미 클리어된 방이면 그냥 통과

        foreach (var enemy in enemies)
        {
            if (enemy != null)
                enemy.ActivateEnemy();
        }

        // 입장하면 몬스터를 다 잡을때까지 못나가도록 물리적으로 막음
        entryDoor.transform.position = new Vector3(entryDoor.transform.position.x, entryDoor.transform.position.y + 5f, entryDoor.transform.position.z);
        exitDoor.transform.position = new Vector3(exitDoor.transform.position.x, exitDoor.transform.position.y + 5f, exitDoor.transform.position.z);
    }

    // 몬스터가 죽을 때 체크하여 클리어 여부 판단
    public void EnemyDefeated(EnemyBase enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
        {
            isCleared = true;
            RoomManager.Instance.RoomCleared(this);

            entryDoor.transform.position = new Vector3(entryDoor.transform.position.x, entryDoor.transform.position.y - 5f, entryDoor.transform.position.z);
            exitDoor.transform.position = new Vector3(exitDoor.transform.position.x, exitDoor.transform.position.y - 5f, exitDoor.transform.position.z);
        }
    }
}
