using UnityEngine;

public class GizmoVisualizer : MonoBehaviour
{
    private PlayerController controller;
    public AttackType gizmoAttackType = AttackType.NormalAtk; // Gizmo로 표시할 공격 타입

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void OnDrawGizmos()
    {
        if (controller == null)
        {
            controller = GetComponent<PlayerController>();
        }

        if (controller.attackStatusDict == null) return;

        foreach (var attackType in controller.attackStatusDict.Keys)
        {
            AttackStateData attackData = controller.attackStatusDict[attackType];

            if (controller.projectileSpawnPoint == null || attackData == null) continue;

            // Gizmo 색상 구분
            Gizmos.color = attackType == AttackType.NormalAtk ? Color.red : Color.green;

            // Gizmo 그리기
            Gizmos.DrawWireSphere(controller.projectileSpawnPoint.position, attackData.atkRange);

            Vector3 start = controller.projectileSpawnPoint.position;
            Vector3 end = start + transform.forward * attackData.atkRange;

            Gizmos.DrawLine(start, end);
            Gizmos.DrawWireSphere(start, attackData.projectileThickness / 2);
            Gizmos.DrawWireSphere(end, attackData.projectileThickness / 2);
        }
    }
}