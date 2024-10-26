using Unity.Cinemachine;
using UnityEngine;

public class PanelControl : MonoBehaviour, IInteractable
{
    [SerializeField] private CinemachineCamera _screenCam;
    [SerializeField] private UnitController _unitController;
    
    public void Interact(GameObject interactor)
    {
        _screenCam.Priority.Value = 1;
        _unitController.BeginControl();
    }

    public void DeactivateControl()
    {
        _screenCam.Priority.Value = 0;
        _unitController.ExitControl();
    }
}
