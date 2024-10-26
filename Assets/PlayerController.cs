using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    // TODO: disable playerControl
    [SerializeField] private CinemachineInputAxisController _playerControl;
    [SerializeField] private LayerMask _screenLayer;

    private PanelControl _currPanel;
    
    public void OnInteract()
    {
        Debug.DrawRay(transform.position, _playerCamera.transform.forward * 10f, Color.magenta, 10f);
        if (Physics.Raycast(transform.position, _playerCamera.transform.forward, out RaycastHit hit, 10f, _screenLayer))
        {
            if (hit.transform.TryGetComponent(out PanelControl panel))
            {
                _currPanel = panel;
                panel.ActivateControl();
            }
        }
    }

    public void OnExit()
    {
        if (!_currPanel) return;
        
        _currPanel.DeactivateControl();
        _currPanel = null;
    }
}
