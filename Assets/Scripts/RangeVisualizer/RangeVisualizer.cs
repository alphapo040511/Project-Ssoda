using UnityEngine;

public class RangeVisualizer : MonoBehaviour
{
    private LineRenderer rangeLineRenderer;
    private PlayerController controller;
    public AttackType visualizerAttackType = AttackType.NormalAtk;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();

        if (controller == null)
        {
            controller = GetComponentInParent<PlayerController>();
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
        if (controller.attackStatusDict != null && controller.attackStatusDict.ContainsKey(visualizerAttackType))
        {
            AttackStateData attackData = controller.attackStatusDict[visualizerAttackType];

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
        if (rangeLineRenderer != null && controller.projectileSpawnPoint != null)
        {
            // ������ ���� Ÿ���� ������ ��������
            if (controller.attackStatusDict != null && controller.attackStatusDict.ContainsKey(visualizerAttackType))
            {
                AttackStateData attackData = controller.attackStatusDict[visualizerAttackType];

                Vector3 start = controller.projectileSpawnPoint.position;
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
