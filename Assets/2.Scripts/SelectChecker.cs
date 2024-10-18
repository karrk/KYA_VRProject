using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectChecker : MonoBehaviour
{
    [SerializeField] private ActionBasedController _controller = null;
    public bool IsSelecting = false;

    private void Start()
    {
        _controller.selectAction.action.started += ((InputAction.CallbackContext _) => { IsSelecting = true; });
        _controller.selectAction.action.canceled += ((InputAction.CallbackContext _) => { IsSelecting = false; });
    }

}
