using UnityEngine;

public class RangeVisualizer : MonoBehaviour
{
    private LineRenderer rangeLineRenderer;
    private PlayerController controller;
    private PlayerAttack attack;

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


    public void ToggleRangeVisualizer(bool isActive)
    {
        if (rangeLineRenderer != null)
        {
            rangeLineRenderer.enabled = isActive;
        }
        controller.isRangeVisualizerActive = isActive;
    }
}
