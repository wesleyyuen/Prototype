using PrimeTween;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable, IHighlightable
{
    [SerializeField] private InteractPrompt _uiPrompt;

    public void Interact(GameObject interactor)
    {
        Debug.Log($"Interacted with {gameObject.name}");
        Tween.PunchScale(transform, Vector3.one * 0.25f, 0.5f);
    }

    public void Highlight(Camera camera)
    {
        if (_uiPrompt)
        {
            _uiPrompt.SetCamera(camera);
            _uiPrompt.ShowPrompt();
        }
    }

    public void Unhighlight()
    {
        if (_uiPrompt)
        {
            _uiPrompt.HidePrompt();
        }
    }
}
