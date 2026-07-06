using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    #region Fields
    [SerializeField] private StatsBarHUD _statsBarHUD;
    [SerializeField] private bool _regenerateHealth = true;
    [SerializeField] private float _healthRegenerateTime = 3f;
    [SerializeField,Range(0,1)] private float _healthRegeneratePercent = 0.05f;
    
    [Header("---- Input ----")]
    [SerializeField] private PlayerInput _input;
    
    [Header("---- Move ----")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _moveRotationAngle = 15f;
    [SerializeField] private float _accelerationTime = 3f;  // 加速时间
    [SerializeField] private float _decelerationTime = 3f;  // 减速时间
    private float _paddingX;
    private float _paddingY;
    private Vector2 _moveDirection;
    private Vector2 _previousVelocity;
    private Quaternion _previousRotation;
    
    [Header("---- Fire ----")]
    [SerializeField] private GameObject _projectile01;
    [SerializeField] private GameObject _projectile02;
    [SerializeField] private GameObject _projectile03;
    [SerializeField] private GameObject _projectileOverdrive;
    [SerializeField] private ParticleSystem _muzzleVFX;
    [SerializeField,Range(0,2)] private int _weaponPower = 0;
    [SerializeField] private Transform _muzzleTop;
    [SerializeField] private Transform _muzzleMiddle;
    [SerializeField] private Transform _muzzleBottom;
    [SerializeField] private AudioData _projectileLaunchSFX;
    [SerializeField] private float _fireInverval = 0.2f;

    [Header("---- Dodge ----")] 
    [SerializeField] private AudioData _dodgeSFX;
    [SerializeField,Range(0,100)] private int _dodgeEnergyCost = 25;
    [SerializeField] private float _maxRoll = 720f;
    [SerializeField] private float _rollSpeed = 360f;
    [SerializeField] private Vector3 _dodgeScale = new Vector3(0.5f,0.5f,0.5f);

    [Header("---- Overdrive ----")] 
    [SerializeField] private int _overdriveDodgeFactor = 2;
    [SerializeField] private float _overdriveSpeedFactor = 1.2f;
    [SerializeField] private float _overdirveFireFactor = 1.2f;

    private readonly float _slowMotionDuration = 0.5f;
    private readonly float _invincibleTime = 1f;
    private float _currentRoll;
    private float _dodgeDuration;
    private bool _isDodging = false;
    private bool _isOverdriving = false;
    
    private WaitForSeconds _waitForFireInterval;
    private WaitForSeconds _waitForOverdriveInterval;
    private WaitForSeconds _waitHealthRegenerateTime;
    private WaitForSeconds _waitDecelerationTime;
    private WaitForSeconds _waitInvincibleTime;
    private WaitForFixedUpdate _waitForFixedUpdate;
    private Coroutine _moveCoroutine;
    private Coroutine _healthRegenerateCoroutine;
    private float _t;
    
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private MissileSystem _missile;
    #endregion

    #region Properties
    public bool IsFullHealth => _health == _maxHealth;
    public bool IsFullPower => _weaponPower == 2;
    #endregion

    #region Unity Event Functions
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _missile = GetComponent<MissileSystem>();

        Vector3 size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        _paddingX = size.x * 0.5f;
        _paddingY = size.y * 0.5f;
        
        _dodgeDuration = _maxRoll / _rollSpeed;
        _rigidbody.gravityScale = 0;
        _waitForFireInterval = new WaitForSeconds(_fireInverval);;
        _waitForOverdriveInterval = new WaitForSeconds(_fireInverval / _overdirveFireFactor);
        _waitHealthRegenerateTime= new WaitForSeconds(_healthRegenerateTime);
        _waitForFixedUpdate = new WaitForFixedUpdate();
        _waitDecelerationTime = new WaitForSeconds(_decelerationTime);
        _waitInvincibleTime = new WaitForSeconds(_invincibleTime);
    }

    private void Start()
    {
        _statsBarHUD.Initialize(_health,_maxHealth);
        _input.EnableGameplayInput();
    }

    protected virtual void OnEnable()
    {
        base.OnEnable();
        _input.onMove += Move;
        _input.onStopMove += StopMove;
        _input.onFire += Fire;
        _input.onStopFire += StopFire;
        _input.onDodge += Dodge;
        _input.onOverdrive += Overdrive;
        _input.onLaunchMissile += LaunchMissile;

        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;
    }

    private void OnDisable()
    {
        _input.onMove -= Move;
        _input.onStopMove -= StopMove;
        _input.onFire -= Fire;
        _input.onStopFire -= StopFire;
        _input.onDodge -= Dodge;
        _input.onOverdrive -= Overdrive;
        _input.onLaunchMissile -= LaunchMissile;
        
        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
    }
    #endregion

    #region Health
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        PowerDown();
        _statsBarHUD.UpdateStats(_health,_maxHealth);
        TimeController.Instance.BulletTime(0.1f);
        if (gameObject.activeSelf)
        {
            Move(_moveDirection);
            StartCoroutine(nameof(InvincibleCoroutine));
            if (_regenerateHealth)
            {
                if (_healthRegenerateCoroutine != null)
                {
                    StopCoroutine(_healthRegenerateCoroutine);
                }
                _healthRegenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(_waitHealthRegenerateTime, _healthRegeneratePercent));
            }
        }
    }
    
    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        _statsBarHUD.UpdateStats(_health,_maxHealth);
    }

    public override void Die()
    {
        GameManager.onGameOver?.Invoke();
        GameManager.GameState = GameState.GameOver;
        _statsBarHUD.UpdateStats(0,_maxHealth);
        base.Die();
    }

    private IEnumerator InvincibleCoroutine()
    {
        _collider.isTrigger = true;
        yield return _waitInvincibleTime;
        _collider.isTrigger = false;
    }
    #endregion
    
    #region Move
    private void Move(Vector2 moveInput)
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }
        // Quaternion moveRotation = Quaternion.AngleAxis(_moveRotationAngle * moveInput.y, Vector3.right);
        // _moveCoroutine = StartCoroutine(MoveCoroutine(_accelerationTime,moveInput.normalized * _moveSpeed,moveRotation));
        _moveDirection = moveInput.normalized;
        _moveCoroutine = StartCoroutine(MoveCoroutine(_accelerationTime,moveInput.normalized * _moveSpeed,Quaternion.AngleAxis(_moveRotationAngle * moveInput.y, Vector3.right)));
        StopCoroutine(nameof(DecelerationCoroutine));
        StartCoroutine(nameof(MoveRangeLimitationCoroutine));
    }

    private void StopMove()
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }
        
        _moveDirection = Vector2.zero;
        _moveCoroutine = StartCoroutine(MoveCoroutine(_decelerationTime,Vector2.zero,Quaternion.identity));
        StartCoroutine(nameof(DecelerationCoroutine));
    }

    private IEnumerator MoveCoroutine(float time,Vector2 moveVelocity,Quaternion moveRotation)
    {
        _t = 0f;
        _previousVelocity = _rigidbody.velocity;
        _previousRotation = transform.rotation;
        while (_t < 1f)
        {
            _t += Time.fixedDeltaTime / time;
            _rigidbody.velocity = Vector2.Lerp(_previousVelocity, moveVelocity, _t);
            transform.rotation = Quaternion.Lerp(_previousRotation,moveRotation,_t);
            yield return _waitForFixedUpdate;
        }
    }
    
    private IEnumerator MoveRangeLimitationCoroutine()
    {
        while (true)
        {
            transform.position = ViewPort.Instance.PlayerMoveablePosition(transform.position, _paddingX, _paddingY);
            yield return null;
        }
    }

    private IEnumerator DecelerationCoroutine()
    {
        yield return _waitDecelerationTime;
        StopCoroutine(nameof(MoveRangeLimitationCoroutine));
    }
    #endregion

    #region Fire

    private void Fire()
    {
        _muzzleVFX.Play();
        StartCoroutine(nameof(FireCoroutine));
    }

    private void StopFire()
    {
        _muzzleVFX.Stop();
        StopCoroutine(nameof(FireCoroutine));
    }

    private IEnumerator FireCoroutine()
    {
        while (true)
        {
            switch (_weaponPower)
            {
                case 0:
                    PoolManager.Release(_isOverdriving?_projectileOverdrive:_projectile01,_muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(_isOverdriving?_projectileOverdrive:_projectile01,_muzzleTop.position);
                    PoolManager.Release(_isOverdriving?_projectileOverdrive:_projectile01,_muzzleBottom.position);
                    break;
                case 2:
                    PoolManager.Release(_isOverdriving?_projectileOverdrive:_projectile02,_muzzleTop.position);
                    PoolManager.Release(_isOverdriving?_projectileOverdrive:_projectile01,_muzzleMiddle.position);
                    PoolManager.Release(_isOverdriving?_projectileOverdrive:_projectile03,_muzzleBottom.position);
                    break;
                default:
                    break;
            }
            
            AudioManager.Instance.PlayRandomSFX(_projectileLaunchSFX);
            yield return _isOverdriving ? _waitForOverdriveInterval : _waitForFireInterval;
        }
    }
    
    #endregion

    #region Dodge

    private void Dodge()
    {
        if (_isDodging || !PlayerEnergy.Instance.IsEnough(_dodgeEnergyCost))
            return;
        TimeController.Instance.BulletTime(0.2f,0.2f);
        StartCoroutine(nameof(DodgeCoroutine));
    }

    private IEnumerator DodgeCoroutine()
    {
        _isDodging = true;
        AudioManager.Instance.PlayRandomSFX(_dodgeSFX);
        PlayerEnergy.Instance.Use(_dodgeEnergyCost);
        _collider.isTrigger = true;
        _currentRoll = 0f;
        
        //方法1
        //Vector3 scale = transform.localScale;
        // while (_currentRoll < _maxRoll)
        // {
        //     _currentRoll += _rollSpeed * Time.deltaTime;
        //     transform.rotation = Quaternion.AngleAxis(_currentRoll, Vector3.right);
        //     if (_currentRoll < _maxRoll * 0.5f)
        //     {
        //         scale.x = Mathf.Clamp(scale.x - Time.deltaTime/_dodgeDuration, _dodgeScale.x,1f);
        //         scale.y = Mathf.Clamp(scale.y - Time.deltaTime/_dodgeDuration, _dodgeScale.y,1f);
        //         scale.z = Mathf.Clamp(scale.z - Time.deltaTime/_dodgeDuration, _dodgeScale.z,1f);
        //     }
        //     else
        //     {
        //         scale.x = Mathf.Clamp(scale.x + Time.deltaTime/_dodgeDuration, _dodgeScale.x,1f);
        //         scale.y = Mathf.Clamp(scale.y + Time.deltaTime/_dodgeDuration, _dodgeScale.y,1f);
        //         scale.z = Mathf.Clamp(scale.z + Time.deltaTime/_dodgeDuration, _dodgeScale.z,1f);
        //     }
        //     transform.localScale = scale;
        //     yield return null;
        // }
        
        //方法2
        // float t1 = 0f;
        // float t2 = 0f;
        // while (_currentRoll < _maxRoll)
        // {
        //     _currentRoll += _rollSpeed * Time.deltaTime;
        //     transform.rotation = Quaternion.AngleAxis(_currentRoll, Vector3.right);
        //     if (_currentRoll < _maxRoll * 0.5f)
        //     {
        //         t1 += Time.deltaTime / _dodgeDuration;
        //         transform.localScale = Vector3.Lerp(transform.localScale,_dodgeScale,t1);
        //     }
        //     else
        //     {
        //         t2 += Time.deltaTime / _dodgeDuration;
        //         transform.localScale = Vector3.Lerp(transform.localScale,Vector3.one,t2);
        //     }
        //     yield return null;
        // }
        
        while (_currentRoll < _maxRoll)
        {
            _currentRoll += _rollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(_currentRoll, Vector3.right);
            transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, _dodgeScale, _currentRoll / _maxRoll);
            yield return null;
        }
        _collider.isTrigger = false;
        _isDodging = false;
    }
    
    #endregion

    #region Overdrive

    private void Overdrive()
    {
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX))
            return;
        PlayerOverdrive.on.Invoke();
    }

    private void OverdriveOn()
    {
        _isOverdriving = true;
        _dodgeEnergyCost *= _overdriveDodgeFactor;
        _moveSpeed *= _overdriveSpeedFactor;
        TimeController.Instance.BulletTime(_slowMotionDuration,_slowMotionDuration);
    }

    private void OverdriveOff()
    {
        _isOverdriving = false;
        _dodgeEnergyCost /= _overdriveDodgeFactor;
        _moveSpeed /= _overdriveSpeedFactor;
    }
    
    #endregion

    #region Missile
    private void LaunchMissile()
    {
        _missile.Launch(_muzzleMiddle);
    }

    public void PickUpMissile()
    {
        _missile.PickUp();
    }
    #endregion

    #region Weapon Power

    public void PowerUp()
    {
        _weaponPower = Mathf.Clamp(_weaponPower+1, 0,2);
    }
    
    public void PowerDown()
    {
        _weaponPower = Mathf.Clamp(_weaponPower-1, 0,2);
    }
    #endregion
}
