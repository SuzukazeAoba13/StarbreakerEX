using UnityEngine;

public class EnemyBeam : MonoBehaviour
{
    [SerializeField] private float _damage = 50f;
    [SerializeField] private GameObject _hitVFX;
    
    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(_damage);
            PoolManager.Release(_hitVFX,collision.GetContact(0).point,Quaternion.LookRotation(collision.GetContact(0).normal));
        }
    }
}
