using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector3 _offset = new Vector3(6, 9, -6);
    [SerializeField] private float _followSpeed = 5f;

    private void LateUpdate()
    {
        //// ��ǥ ��ġ ���
        //Vector3 targetPosition = _targetTransform.position + _offset;

        //// ī�޶� ��ġ�� ��ǥ ��ġ�� �ε巴�� �̵�
        //transform.position = Vector3.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);

        //transform.LookAt(_targetTransform);

        transform.position = _targetTransform.position + _offset;
    }
}
