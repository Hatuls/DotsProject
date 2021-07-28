using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Fields
  [SerializeField]  bool _enableInput;
    public bool EnableInput { get => _enableInput; set => _enableInput = value; }
    #endregion

    #region Events
   
    [SerializeField]   EventOfFloat _verticalMovementEvent;
    [SerializeField] EventOfFloat _horizontalMovementEvent;
    [SerializeField] EventOfInt _weaponSelectionEvent;
    [SerializeField] EventDispatcher _shootEvent;

    #endregion

    #region Keys
    [SerializeField] KeyCode _forwardKey;
    [SerializeField] KeyCode _backwardKey;
    [SerializeField] KeyCode _leftKey;
    [SerializeField] KeyCode _rightKey;

    [SerializeField] KeyCode _weaponOneKey;
    [SerializeField] KeyCode _weaponTwoKey;
    [SerializeField] KeyCode _weaponThreeKey;

    [SerializeField] KeyCode _shootKey;

    
    #endregion

    private void Update()
    {
        if (_enableInput == true)
        {
        MovementInputs();
        WeaponSelection();
        ShootInput();
        }
    }

    private void ShootInput()
    {
        if (Input.GetKey(_shootKey))
            _shootEvent?.RaiseEvent();
    }

    private void WeaponSelection()
    {
        if (Input.GetKey(_weaponOneKey))
            _weaponSelectionEvent?.RaiseEvent(1);

        else if (Input.GetKey(_weaponOneKey))
            _weaponSelectionEvent?.RaiseEvent(2);

        else if (Input.GetKey(_weaponOneKey))
            _weaponSelectionEvent?.RaiseEvent(3);
    }

    private void MovementInputs()
    {
        if (Input.GetKey(_forwardKey) || Input.GetKey(_backwardKey))
           _verticalMovementEvent?.RaiseEvent(Input.GetAxis("Vertical"));

        if (Input.GetKey(_leftKey) || Input.GetKey(_rightKey))
           _horizontalMovementEvent?.RaiseEvent(Input.GetAxis("Horizontal"));
    }
}
