using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _delay;
    [SerializeField] private Bullet _prefab;
    [SerializeField] private Transform _target;

    private Coroutine _coroutine;
    private bool _isRunning = true;

    private void Start()
    {
        Restart();
    }

    public void Stop()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _isRunning = false;
    }

    public void Restart()
    {
        Stop();
        _isRunning = true;
        _coroutine = StartCoroutine(Coroutine());
    }

    private IEnumerator Coroutine()
    {
        WaitForSecondsRealtime delay = new WaitForSecondsRealtime(_delay);

        while (_isRunning)
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            Bullet bullet = Instantiate(_prefab, transform.position, Quaternion.identity);
            bullet.SetSpeed(_speed);
            bullet.Move(direction);

            yield return delay;
        }
    }
}
