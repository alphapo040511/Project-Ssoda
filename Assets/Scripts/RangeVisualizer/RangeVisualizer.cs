using System.Collections.Generic;
using UnityEngine;

public class RangeVisualizer : MonoBehaviour
{
    private LineRenderer rangeLineRenderer;
    private PlayerController controller;
    private PlayerAttack attack;
    private Vector3 throwRangePosition;     // ��ô ���� ���� ���� ��ǥ

    public GameObject throwRangeIndicator; // ���� ǥ�ÿ� �� (������ �Ǵ� ��������Ʈ)
    public List<AttackStateData> attackState = new List<AttackStateData>();

    private void Awake()
    {
        controller = GetComponent<PlayerController>();

        if (controller == null)
        {
            controller = GetComponentInParent<PlayerController>();
        }

        attack = GetComponent<PlayerAttack>();

        if (controller == null)
        {
            attack = GetComponentInParent<PlayerAttack>();
        }

        CreateRangeVisualizer();
    }

    private void OnDrawGizmos()
    {
        DrawMeleeAttackRange();
    }

    public void CreateRangeVisualizer()
    {
        rangeLineRenderer = GetComponent<LineRenderer>();
        if (rangeLineRenderer == null)
        {
            rangeLineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // ������ ���� Ÿ���� ������ ��������
        if (attack.attackStatusDict != null && attack.attackStatusDict.ContainsKey(attack.GetCurrentWeaponType()))
        {
            AttackStateData attackData = attack.attackStatusDict[attack.GetCurrentWeaponType()];

            rangeLineRenderer.startWidth = attackData.projectileThickness;
            rangeLineRenderer.endWidth = attackData.projectileThickness;
            rangeLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            rangeLineRenderer.startColor = Color.blue;
            rangeLineRenderer.endColor = Color.blue;

            rangeLineRenderer.positionCount = 2;
            rangeLineRenderer.useWorldSpace = true;

            UpdateRangeVisualizer();
        }
    }

    public void UpdateRangeVisualizer()
    {
        if (rangeLineRenderer != null && attack.projectileSpawnPoint != null)
        {
            // ������ ���� Ÿ���� ������ ��������
            if (attack.attackStatusDict != null && attack.attackStatusDict.ContainsKey(attack.GetCurrentWeaponType()))
            {
                AttackStateData attackData = attack.attackStatusDict[attack.GetCurrentWeaponType()];

                Vector3 start = attack.projectileSpawnPoint.position;
                Vector3 end = start + controller.transform.forward * attackData.atkRange;

                rangeLineRenderer.SetPosition(0, start);
                rangeLineRenderer.SetPosition(1, end);
            }
        }
    }

    private void DrawMeleeAttackRange()
    {
        AttackStateData data = attackState[2];

        Gizmos.color = new Color(1, 0, 0, 1f);

        Vector3 playerPosition = gameObject.transform.position;

        Vector3 mousePos = Input.mousePosition;
        float distanceFromCamera = Vector3.Distance(Camera.main.transform.position, transform.position);
        mousePos.z = distanceFromCamera; // ī�޶�� �÷��̾� ������ �Ÿ��� ����
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        mouseWorldPos.y = playerPosition.y;

        Vector3 attackDirection = (mouseWorldPos - playerPosition).normalized;

        // ��ä�� ������
        float halfAngle = data.attackAngle / 2;
        Quaternion leftRotation = Quaternion.Euler(0, -halfAngle, 0);
        Quaternion rightRotation = Quaternion.Euler(0, halfAngle, 0);

        Vector3 leftBoundary = leftRotation * attackDirection * data.atkRange;
        Vector3 rightBoundary = rightRotation * attackDirection * data.atkRange;

        // ������ �� �׸���
        Gizmos.DrawLine(playerPosition, playerPosition + leftBoundary);
        Gizmos.DrawLine(playerPosition, playerPosition + rightBoundary);

        // ��� ����� ��
        int segmentCount = 10;
        Vector3 prevPoint = playerPosition + leftBoundary;

        for (int i = 1; i <= segmentCount; i++)
        {
            float t = (float)i / segmentCount;
            Quaternion segmentRotation = Quaternion.Euler(0, -halfAngle + (t * data.attackAngle), 0);
            Vector3 segmentPoint = segmentRotation * attackDirection * data.atkRange + playerPosition;

            // ������ �̾ �ó�� ���̵��� ��
            Gizmos.DrawLine(prevPoint, segmentPoint);
            prevPoint = segmentPoint;
        }

        // ������ ���� ������ ��踦 ����
        Gizmos.DrawLine(prevPoint, playerPosition + rightBoundary);
    }


    public void ShowThrowRange()
    {
        if (!throwRangeIndicator.activeSelf)
        {
            throwRangeIndicator.SetActive(true);
        }

        Vector3 playerPosition = transform.position;
        Vector3 mousePos = Input.mousePosition;
        float distanceFromCamera = Vector3.Distance(Camera.main.transform.position, transform.position);
        mousePos.z = distanceFromCamera;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.y = playerPosition.y;

        AttackStateData data = attack.attackStatusDict[attack.currentWeaponType];

        // ���� �߽��� ��� (�÷��̾� ����)
        throwRangePosition = playerPosition + (worldPos - playerPosition).normalized * data.atkRange;

        // throwRangeIndicator�� ��ġ�� ���� ��ǥ �������� ����
        throwRangeIndicator.transform.localPosition = transform.InverseTransformPoint(throwRangePosition);

        // ���� ũ�⸦ areaWidth�� �°� ���� (SpriteRenderer �Ǵ� LineRenderer)
        float areaWidth = data.areaWidth;
        throwRangeIndicator.transform.localScale = new Vector3(areaWidth * 2, 0.001f, areaWidth * 2);
    }


    public void ToggleRangeVisualizer(bool isActive)
    {
        if (rangeLineRenderer != null)
        {
            rangeLineRenderer.enabled = isActive;
        }
        controller.isRangeVisualizerActive = isActive;
    }
}
