using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _interactDistance;
    
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private CinemachineInputAxisController _playerCameraControl;
    [SerializeField] private LayerMask _screenLayer;

    private Vector3 OrientationForward => _playerCamera.transform.forward;
    private Vector3 OrientationRight => _playerCamera.transform.right;
    
    private PanelControl _currPanel;
    private Vector2 _movementInput;

    [SerializeField, HideInInspector] private PlayerInput _playerInput;
    [SerializeField, HideInInspector] private Rigidbody _rigidbody;
    [SerializeField, HideInInspector] private CinemachineBrain _cinemachineBrain;

    private void OnValidate()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _cinemachineBrain = _playerCamera.GetComponent<CinemachineBrain>();
    }

    private void OnEnable()
    {
        UnitController.Event_ExitUnitControl += OnExitUnitControl;
    }

    private void OnDisable()
    {
        UnitController.Event_ExitUnitControl -= OnExitUnitControl;
    }

    public void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
    
    public void OnInteract()
    {
        Debug.DrawRay(_playerCamera.transform.position, OrientationForward * _interactDistance, Color.magenta, 5f);
        if (Physics.Raycast(_playerCamera.transform.position, OrientationForward, out RaycastHit hit, _interactDistance, _screenLayer))
        {
            // TODO: get IInteractable instead?
            if (hit.transform.TryGetComponent(out PanelControl panel))
            {
                _currPanel = panel;
                EnableControl(false);
                panel.Interact(gameObject);
            }
        }
    }
    
    private void OnExitUnitControl()
    {
        StartCoroutine(EnablePlayerControlAfterSeconds(_cinemachineBrain.DefaultBlend.Time));
        _currPanel.DeactivateControl();
    }

    private IEnumerator EnablePlayerControlAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);

        EnableControl(true);
        _currPanel = null;
    }

    private void FixedUpdate()
    {
        if (!_currPanel)
        {
            Move();
        }
        
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, _playerCamera.transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }

    private void Move()
    {
        Vector3 movementDirection = OrientationRight * _movementInput.x + OrientationForward * _movementInput.y;
        Vector2 movementVector = new Vector2(movementDirection.x, movementDirection.z).normalized * (_speed * Time.fixedDeltaTime);
        _rigidbody.linearVelocity = new Vector3(movementVector.x, 0, movementVector.y);
    }
    
    private void EnableControl(bool shouldEnable)
    {
        _playerInput.enabled = shouldEnable;
        _playerCameraControl.enabled = shouldEnable;
    }
}
