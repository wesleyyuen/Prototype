using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private CinemachineInputAxisController _playerControl;
    [SerializeField] private LayerMask _screenLayer;

    private Vector3 OrientationForward => _playerCamera.transform.forward;
    private Vector3 OrientationRight => _playerCamera.transform.right;
    
    private PanelControl _currPanel;
    private Vector2 _movementInput;

    private Rigidbody _rigidbody;
    private CinemachineBrain _cinemachineBrain;

    private void OnValidate()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _cinemachineBrain = _playerCamera.GetComponent<CinemachineBrain>();
    }

    public void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
    
    public void OnInteract()
    {
        Debug.DrawRay(_playerCamera.transform.position, OrientationForward * 10f, Color.magenta, 10f);
        if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out RaycastHit hit, 10f, _screenLayer))
        {
            if (hit.transform.TryGetComponent(out PanelControl panel))
            {
                _currPanel = panel;
                _playerControl.enabled = false;
                panel.ActivateControl();
            }
        }
    }

    public void OnExit()
    {
        if (!_currPanel) return;

        StartCoroutine(EnablePlayerControlAfterSeconds(_cinemachineBrain.DefaultBlend.Time));
        _currPanel.DeactivateControl();
        
    }

    private IEnumerator EnablePlayerControlAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        _playerControl.enabled = true;
        _currPanel = null;
    }

    private void FixedUpdate()
    {
        if (!_currPanel)
        {
            Move();
        }
    }

    private void LateUpdate()
    {
        // rotate player to match camera rotation
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, _playerCamera.transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }

    private void Move()
    {
        Vector3 movementDirection = OrientationRight * _movementInput.x + OrientationForward * _movementInput.y;
        Vector2 movementVector = new Vector2(movementDirection.x, movementDirection.z).normalized * (_speed * Time.fixedDeltaTime);
        _rigidbody.linearVelocity = new Vector3(movementVector.x, 0, movementVector.y);
    }
}
