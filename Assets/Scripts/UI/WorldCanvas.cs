using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvas : MonoBehaviour
{
    private Transform camTransform;

    void Start()
    {
        camTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // UI가 항상 카메라를 향하도록 회전
        transform.rotation = Quaternion.LookRotation(camTransform.forward);
    }
}
