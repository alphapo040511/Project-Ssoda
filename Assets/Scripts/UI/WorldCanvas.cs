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
        // UI�� �׻� ī�޶� ���ϵ��� ȸ��
        transform.rotation = Quaternion.LookRotation(camTransform.forward);
    }
}
