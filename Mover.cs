using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private List<Transform> _points = new();
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _rotationSpeed = 20f;

    private Transform _nextPoint = null;
    private Queue<Transform> _pointsQueue = new();
    private float _minDistance = 0.1f;

    private void Start()
    {
        foreach (Transform point in _points)
        {
            _pointsQueue.Enqueue(point);
        }

        _nextPoint = _pointsQueue.Dequeue();
    }

    private void Update()
    {
        
        if (Vector3Extensions.IsEnoughClose(transform.position, _nextPoint.position, _minDistance))
        {
            _pointsQueue.Enqueue(_nextPoint);
            _nextPoint = _pointsQueue.Dequeue();

        }

        Move(_nextPoint);
    }

    private void Move(Transform point)
    {
        Vector3 direction = (point.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        transform.Translate(direction * _speed * Time.deltaTime, Space.World);
    }
}

public static class Vector3Extensions
{
    public static float SqrDistance(this Vector3 start, Vector3 end)
    {
        return (end - start).sqrMagnitude;
    }

    public static bool IsEnoughClose(this Vector3 start, Vector3 end, float distance)
    {
        return start.SqrDistance(end) <= distance * distance;
    }
}