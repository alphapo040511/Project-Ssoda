using UnityEngine;

public class GizmoVisualizer : MonoBehaviour
{
    private PlayerController controller;

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

        if (controller.projectileSpawnPoint == null || controller.attackStatus == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controller.projectileSpawnPoint.position, controller.attackStatus.atkRange);

        Gizmos.color = Color.blue;
        Vector3 start = controller.projectileSpawnPoint.position;
        Vector3 end = start + transform.forward * controller.attackStatus.atkRange;

        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(start, controller.attackStatus.projectileThickness / 2);
        Gizmos.DrawWireSphere(end, controller.attackStatus.projectileThickness / 2);
    }

}
