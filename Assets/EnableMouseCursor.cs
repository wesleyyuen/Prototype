using System;
using UnityEngine;

public class EnableMouseCursor : MonoBehaviour
{
    [SerializeField] private bool _shouldRunOnAwake = true;
    [SerializeField] private bool _shouldEnable;
    
    private void Awake()
    {
        if (_shouldRunOnAwake)
        {
            SetMouseCursor(_shouldEnable);
        }
    }

    public void SetMouseCursor(bool shouldEnable)
    {
        Cursor.lockState = shouldEnable ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = shouldEnable;
    }
}
