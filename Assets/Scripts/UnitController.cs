using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitController : MonoBehaviour
{
    [SerializeField] private float _interactDistance;
    
    [SerializeField] private Camera _unitCamera;
    [SerializeField] private CinemachineInputAxisController _unitCameraControl;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private LayerMask _UILayer;
    
    private Vector3 OrientationForward => _unitCamera.transform.forward;
    private Vector3 OrientationRight => _unitCamera.transform.right;

    private IInteractable _currInteractable;
    private IHighlightable _currHighlightable;

    [SerializeField, HideInInspector] private PlayerInput _unitInput;
    
    // TODO: use GameEvents instead
    public static event Action Event_ExitUnitControl;
    
    private void OnValidate()
    {
        _unitInput = GetComponent<PlayerInput>();
    }

    private void Awake()
    {
        EnableControl(false);
    }

    public void FixedUpdate()
    {
        IHighlightable prevHighlightable = _currHighlightable;
        _currHighlightable = null;
        _currInteractable = null;
        
        if (Physics.Raycast(_unitCamera.transform.position, OrientationForward, out RaycastHit hit, _interactDistance, _interactableLayer))
        {
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                _currInteractable = interactable;
            }
            if (hit.transform.TryGetComponent(out IHighlightable highlightable))
            {
                _currHighlightable = highlightable;
            }
        }
        
        if (_currHighlightable != prevHighlightable)
        {
            OnHighlightableChanged(_currHighlightable ?? prevHighlightable);
        }
    }

    public void OnInteract()
    {
        _currInteractable?.Interact(gameObject);
    }

    public void OnExit()
    {
        Event_ExitUnitControl?.Invoke();
    }

    public void BeginControl()
    {
        // Add UI layer to camera culling layerMask, so it renders the ui prompts in the environment
        _unitCamera.cullingMask |= _UILayer;
        EnableControl(true);
    }
    
    public void ExitControl()
    {
        // Remove UI layer to camera culling layerMask, so it stops rendering the ui prompts on the screen
        _unitCamera.cullingMask &= ~_UILayer;
        EnableControl(false);
    }

    private void OnHighlightableChanged(IHighlightable highlightable)
    {
        if (_currInteractable != null)
        {
            highlightable.Highlight(_unitCamera);
        }
        else
        {
            highlightable.Unhighlight();
        }
    }

    private void EnableControl(bool shouldEnable)
    {
        _unitInput.enabled = shouldEnable;
        _unitCameraControl.enabled = shouldEnable;
    }
}
