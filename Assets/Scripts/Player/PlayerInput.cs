using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string horizontalAxisName = "Horizontal";
    public string verticalAxisName = "Vertical";

    public float moveX { get; private set; }
    public float moveY { get; private set; }

    private void Update()
    {
        moveX = Input.GetAxisRaw(horizontalAxisName);
        moveY = Input.GetAxisRaw(verticalAxisName);
    }
}
