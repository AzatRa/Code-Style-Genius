using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _delay;
    [SerializeField] private Bullet _prefab;
    [SerializeField] private Transform _target;
    [SerializeField] private int _poolCapasity = 10;
    [SerializeField] private int _poolMaxSize = 20;

    private ObjectPool<Bullet> _pool;
    private Coroutine _coroutine;
    private bool _isRunning = true;

    private void Awake()
    {
        _pool = new ObjectPool<Bullet>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapasity,
            maxSize: _poolMaxSize);
    }

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

    private Bullet Spawn()
    {
        Bullet bullet = Get();
        bullet.transform.position = transform.position;
        bullet.SetSpeed(_speed);
        bullet.gameObject.SetActive(true);
        bullet.CollidedWithTarget += OnBulletCollision;

        return bullet;
    }

    private void OnBulletCollision(Bullet enemy)
    {
        enemy.CollidedWithTarget -= OnBulletCollision;
        Release(enemy);
    }

    private Bullet Get()
    {
        return _pool.Get();
    }

    private void Release(Bullet obj)
    {
        _pool.Release(obj);
    }

    private IEnumerator Coroutine()
    {
        while (_isRunning)
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            Bullet bullet = Spawn();

            bullet.Move(direction);

            yield return new WaitForSecondsRealtime(_delay);
        }
    }
}