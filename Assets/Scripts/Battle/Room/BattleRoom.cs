using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : MonoBehaviour
{
    [Header("�濡 ���� ���� ����Ʈ")]
    public List<EnemyBase> enemies = new List<EnemyBase>(); // �濡 �ִ� ���͵�
    public bool isCleared = false;  // �� Ŭ���� ����

    [Header("���� ���ⱸ")]
    public GameObject entryDoor;  // �Ա� ��
    public GameObject exitDoor;   // �ⱸ ��

    // �÷��̾ �濡 ���� �� ȣ��
    public void EnterRoom()
    {
        if (isCleared) return;  // �̹� Ŭ����� ���̸� �׳� ���

        foreach (var enemy in enemies)
        {
            if (enemy != null)
                enemy.ActivateEnemy();
        }

        // �����ϸ� ���͸� �� ���������� ���������� ���������� ����
        entryDoor.transform.position = new Vector3(entryDoor.transform.position.x, entryDoor.transform.position.y + 5f, entryDoor.transform.position.z);
        exitDoor.transform.position = new Vector3(exitDoor.transform.position.x, exitDoor.transform.position.y + 5f, exitDoor.transform.position.z);
    }

    // ���Ͱ� ���� �� üũ�Ͽ� Ŭ���� ���� �Ǵ�
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
