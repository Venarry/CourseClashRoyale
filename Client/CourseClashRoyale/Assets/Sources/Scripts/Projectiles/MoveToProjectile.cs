using UnityEngine;

public class MoveToProjectile : MonoBehaviour
{
    private ITarget _target;
    private Transform _transform;
    private int _damage;
    private bool _isInitialized;
    private Vector3 _lastTargetPosition;
    private float _targetRadius;

    public void Init(ITarget target, int damage)
    {
        _target = target;
        _transform = transform;
        _damage = damage;
        _targetRadius = target.Radius;
        _isInitialized = true;

        _target.Destroyed += OnTargetDestroy;
    }

    private void OnTargetDestroy(ITarget target)
    {
        target.Destroyed -= OnTargetDestroy;
        _target = null;
    }

    private void Update()
    {
        if(_isInitialized == false)
        {
            return;
        }

        if (_target != null)
        {
            _lastTargetPosition = _target.Transform.position;
        }

        float speed = 10f;

        _transform.rotation = Quaternion.LookRotation(
            _lastTargetPosition - _transform.position);

        _transform.position = Vector3.MoveTowards(
            _transform.position, _lastTargetPosition, speed * Time.deltaTime);

        if(Vector3.Distance(_transform.position, _lastTargetPosition) <= _targetRadius)
        {
            _target?.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
