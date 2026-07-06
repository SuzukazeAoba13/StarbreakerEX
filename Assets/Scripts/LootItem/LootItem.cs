using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LootItem : MonoBehaviour
{
    [SerializeField] private float _minSpeed = 5f;
    [SerializeField] private float _maxSpeed = 15f;
    [SerializeField] protected AudioData _defaultPickUpSFX;
    protected AudioData _pickUpSFX;
    private Animator _animator;
    protected Player _player;
    protected Text _lootMessage;
    
    private int _pickUpStateID = Animator.StringToHash("PickUp");

    private void Awake()
    {
        _animator =  GetComponent<Animator>();
        _player = FindObjectOfType<Player>();
        _lootMessage = GetComponentInChildren<Text>(true);
        _pickUpSFX = _defaultPickUpSFX;
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(MoveCoroutine));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PickUp();
    }

    protected virtual void PickUp()
    {
        StopCoroutine(nameof(MoveCoroutine));
        _animator.Play(_pickUpStateID);
        AudioManager.Instance.PlayRandomSFX(_pickUpSFX);
    }
    
    private IEnumerator MoveCoroutine()
    {
        float speed = Random.Range(_minSpeed, _maxSpeed);
        Vector3 direction = Vector3.left;
        while (true)
        {
            if (_player.isActiveAndEnabled)
            {
                direction = (_player.transform.position-transform.position).normalized;
                transform.Translate(direction*speed*Time.deltaTime);
            }
            yield return null;
        }
    }
}
