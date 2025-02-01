using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : MonoBehaviour
{
    [Header("�濡 ���� ���� ����Ʈ")]
    public List<EnemyBase> enemies = new List<EnemyBase>(); // �濡 �ִ� ���͵�
    public bool isCleared = false;  // �� Ŭ���� ����
    public bool isFighting = false;  // �� ������ ����

    [Header("���� ���ⱸ")]
    public GameObject entryDoor;  // �Ա� ��
    public GameObject exitDoor;   // �ⱸ ��

    void Start()
    {
        foreach (var EnemyBase in enemies)
        {
            EnemyBase.SetRoom(this);        // ����Ʈ�� ��ϵ� ���͵鿡�� � �濡 ���ϴ��� ����
        }
    }

    // �÷��̾ �濡 �����ߴ���
    private void OnTriggerEnter(Collider other)
    {
        if (isCleared || isFighting) return; // �̹� Ŭ����Ǿ��ų� ���� ���̸� ���� X

        if (other.CompareTag("Player")) // �÷��̾ ������ ��
        {
            EnterRoom();
        }
    }

    // �÷��̾ �濡 ���� �� ȣ��
    public void EnterRoom()
    {
        if (isCleared) return;  // �̹� Ŭ����� ���̸� �׳� ���

        foreach (var enemy in enemies)
        {
            if (enemy != null)
                enemy.ActivateEnemy();
        }

        isFighting = true;
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
            isFighting = false;
            //RoomManager.Instance.RoomCleared(this);

            exitDoor.transform.position = new Vector3(exitDoor.transform.position.x, exitDoor.transform.position.y - 5f, exitDoor.transform.position.z);
        }
    }
}
