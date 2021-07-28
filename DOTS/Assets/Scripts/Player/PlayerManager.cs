using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Fields

    public static PlayerManager Instance;



    #region ScriptableObjects Data
    [SerializeField] PlayerSettings _startingData;
    #endregion

    #region Events

    [SerializeField] EventOfFloat _verticalMovementEvent;
    [SerializeField] EventOfFloat _horizontalMovementEvent;
    [SerializeField] EventOfInt _weaponSelectionEvent;
    [SerializeField] EventDispatcher _shootEvent;

    #endregion

    #region References
    PlayerShootings _playerShootings;


    PlayerMovement _playerMovement;
    public PlayerMovement PlayerMovement => _playerMovement;
    #endregion


    #region Data
    Weapon.WeaponTypeEnum weapon;

    private static int _score;
    public static Transform Body;
    #endregion

    #endregion
    private void Awake()
    {
        Instance = this;

        _playerShootings = new PlayerShootings(this.transform, _startingData.StartingWeapon);
        _shootEvent.OnEventRaised += _playerShootings.Shoot;

        Body = this.transform;

        _playerMovement = new PlayerMovement(this.transform, _startingData, ref _xMin, ref _xMax, ref _zMin, ref _zMax, ref _offset);
        _verticalMovementEvent.OnEventRaised += _playerMovement.VerticalMovement;
        _horizontalMovementEvent.OnEventRaised += _playerMovement.HorizontalMovement;

        weapon = _startingData.StartingWeapon.WeaponType;
    }

    private void Start()
    {
        ResetData();
    }
    public static void ResetData()
        => _score = 0;
    public static void AddScore()
    { 
        _score += 1;
        UIManager.AddScore(ref _score);
    }

    private void Update()
    {
        _playerShootings.CheckShootingCooldown();
    }


    private void OnDisable()
    {
        _verticalMovementEvent.OnEventRaised -= _playerMovement.VerticalMovement;
        _horizontalMovementEvent.OnEventRaised -= _playerMovement.HorizontalMovement;


        _shootEvent.OnEventRaised -= _playerShootings.Shoot;
    }


    #region Boundrys


    [Space]
    [Header("Gizmos")]

    [Range(1, 10f)]
    [SerializeField] float _size;
    [SerializeField] float _offset;
    [SerializeField] float _xMax, _zMax;
    [SerializeField] float _xMin, _zMin;
    public static float BottomScreenPoint => Instance._zMin;
    public static float TopScreenPoint => Instance._zMax;
    public static float LeftScreenPoint=> Instance._xMin;
    public static float RightScreenPoint => Instance._xMax;
    private void OnDrawGizmos()
    {
        Vector3 leftDownPoint = new Vector3(_xMin, 0, _zMin);
        Vector3 topRightPoint = new Vector3(_xMax, 0, _zMax);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(leftDownPoint, Vector3.one * _size);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(topRightPoint, Vector3.one * _size);

        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(leftDownPoint, new Vector3(-1 * topRightPoint.x, topRightPoint.y, topRightPoint.z));
        Gizmos.DrawLine(topRightPoint, new Vector3(-1 * leftDownPoint.x, leftDownPoint.y, leftDownPoint.z));
        
        
        Gizmos.DrawLine(leftDownPoint, new Vector3(topRightPoint.x, topRightPoint.y, -1 * topRightPoint.z));
        Gizmos.DrawLine(topRightPoint, new Vector3(leftDownPoint.x, leftDownPoint.y,-1 *  leftDownPoint.z));

    }
    #endregion
}


public class PlayerShootings
{
    private Transform _transform;
    private Weapon _currentWeapon;

    public bool CanShoot { set; get; }
    private float currentTime;

    public static int WeaponLevel { get; set; }

    public PlayerShootings(Transform transform, Weapon currentWeapon)
    {
        _transform = transform;
        this._currentWeapon = currentWeapon;
        CanShoot = true;
        currentTime = 0;
        ResetWeaponLevel();
    }
    public static void ResetWeaponLevel() { 
        WeaponLevel = 1;
        UIManager.SetMisslesPerShoot(WeaponLevel);
    }
    public static void AddPowerUpLevel() 
    {
        WeaponLevel++;
      UIManager.SetMisslesPerShoot(WeaponLevel);
    }
    public void Shoot()
    {
        Debug.Log("Shootings");
        if (CanShoot)
        {
          EntityHandler.Instance.SpawnPlayerBullet(_currentWeapon, Quaternion.identity, WeaponLevel);
          CanShoot = false;
        }
    }
  public void  CheckShootingCooldown() 
    {
        if (CanShoot == false)
        {
            currentTime += Time.deltaTime;
            if (_currentWeapon.CoolDownSpeed < currentTime)
            {
                CanShoot = true;
                currentTime = 0;
            }
        }
    
    }
}

public class PlayerMovement
{
    private PlayerSettings _playerSettings;
    private Transform _transform;



    public static float SpeedBuffer;
    float _xMin;
    float _xMax;
    float _zMin;
    float _zMax;
    float _offset;
    public PlayerMovement(Transform transform,PlayerSettings startingData,ref float xMin,ref float xMax,ref float zMin,ref float zMax , ref float offset)
    {
        _transform = transform;
        _playerSettings = startingData;

        _xMin = xMin;
        _xMax = xMax;
        _zMin = zMin;
        _zMax = zMax;

        _offset = offset;

        SpeedBuffer = 0;
    }

    public void VerticalMovement(float value)
    {
        if (VerticalMovementCondition(value))
            _transform.Translate(Vector3.forward * value * (_playerSettings.VerticalSpeed+ SpeedBuffer) * Time.deltaTime);
    }

    public void HorizontalMovement(float value)
    {
        if (HorizontalMovementCondition(value))
                _transform.Translate(Vector2.right * value *( _playerSettings.HorizontalSpeed+ SpeedBuffer) * Time.deltaTime);

    }

    private bool HorizontalMovementCondition(float value)
    {

        if (value < 0)
            return Mathf.Abs(_transform.position.x - _xMin) > _transform.localScale.x + _offset;

        else
            return Mathf.Abs(_transform.position.x - _xMax) > _transform.localScale.x + _offset;
    }
    private bool VerticalMovementCondition(float value)
    {

        if (value < 0)
            return Mathf.Abs(_transform.position.z - _zMin) > _transform.localScale.z + _offset;

        else
            return Mathf.Abs(_transform.position.z - _zMax) > _transform.localScale.z + _offset;
    }
}
