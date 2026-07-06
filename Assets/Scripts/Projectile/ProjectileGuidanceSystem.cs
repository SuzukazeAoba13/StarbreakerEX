using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private float _minBallistAngle = 50f;
    [SerializeField] private float _maxBallistAngle = 75f;
    private Vector3 _targetDirection;
    private float _ballisticAngle;
    
    public IEnumerator HomingCoroutine(GameObject target)
    {
        _ballisticAngle = Random.Range(_minBallistAngle, _maxBallistAngle);
        while (gameObject.activeSelf)
        {
            if (target.activeSelf)
            {
                _targetDirection = target.transform.position - transform.position;
                float angle = Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation *= Quaternion.Euler(0f, 0f, _ballisticAngle);
                _projectile.Move();
            }
            else
            {
                _projectile.Move();
            }
            yield return null;
        }
    }
}
