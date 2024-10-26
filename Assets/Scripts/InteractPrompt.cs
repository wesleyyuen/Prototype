using System;
using UnityEngine;

public class InteractPrompt : MonoBehaviour
{
    [SerializeField, HideInInspector] private CanvasGroup _canvasGroup;
    private Camera _cameraToFace;
    private bool _isPromopted;
    
    private void OnValidate()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Awake()
    {
        _canvasGroup.alpha = 0;
    }

    private void Update()
    {
        if (!_isPromopted || !_cameraToFace) return;
        
        transform.rotation = _cameraToFace.transform.rotation;
    }

    public void ShowPrompt()
    {
        _isPromopted = true;
        _canvasGroup.alpha = 1;
    }

    public void HidePrompt()
    {
        _isPromopted = false;
        _canvasGroup.alpha = 0;
    }

    public void SetCamera(Camera cameraToFace)
    {
        _cameraToFace = cameraToFace;
    }
}
