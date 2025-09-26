using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private float _speed;
    private Rigidbody _rigidbody;

    public event Action<Bullet> CollidedWithTarget;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        transform.up = direction;
        _rigidbody.velocity = direction * _speed;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Target>(out _))
        {
            CollidedWithTarget?.Invoke(this);
        }
    }
}
