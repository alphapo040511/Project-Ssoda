using UnityEngine;

public class GizmoVisualizer : MonoBehaviour
{
    private PlayerAttack controller;

    private void Awake()
    {
        controller = GetComponent<PlayerAttack>();
    }

    private void OnDrawGizmos()
    {
        if (controller == null)
        {
            controller = GetComponent<PlayerAttack>();
        }

        if (controller.attackStatusDict == null) return;

        foreach (var attackType in controller.attackStatusDict.Keys)
        {
            AttackStateData attackData = controller.attackStatusDict[attackType];

            if (controller.projectileSpawnPoint == null || attackData == null) continue;

            // Gizmo ���� ����
            Gizmos.color = attackType == controller.GetCurrentWeaponType() ? Color.red : Color.green;

            // Gizmo �׸���
            Gizmos.DrawWireSphere(controller.projectileSpawnPoint.position, attackData.atkRange);

            Vector3 start = controller.projectileSpawnPoint.position;
            Vector3 end = start + transform.forward * attackData.atkRange;

            Gizmos.DrawLine(start, end);
            Gizmos.DrawWireSphere(start, attackData.projectileThickness / 2);
            Gizmos.DrawWireSphere(end, attackData.projectileThickness / 2);
        }
    }
}