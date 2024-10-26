using Unity.Cinemachine;
using UnityEngine;

public class PanelControl : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _screenCam;
    [SerializeField] private CinemachineInputAxisController _unitControl;
    
    public void ActivateControl()
    {
        _screenCam.Priority.Value = 1;
        _unitControl.enabled = true;
    }

    public void DeactivateControl()
    {
        _screenCam.Priority.Value = 0;
        _unitControl.enabled = false;
    }
}
