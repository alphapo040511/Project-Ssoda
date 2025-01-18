using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector3 _offset = new Vector3(6, 9, -6);
    [SerializeField] private float _followSpeed = 5f;

    private void LateUpdate()
    {
        //// 목표 위치 계산
        //Vector3 targetPosition = _targetTransform.position + _offset;

        //// 카메라 위치를 목표 위치로 부드럽게 이동
        //transform.position = Vector3.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);

        //transform.LookAt(_targetTransform);

        transform.position = _targetTransform.position + _offset;
    }
}
