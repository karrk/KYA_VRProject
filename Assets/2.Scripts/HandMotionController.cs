using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HandMotionController : MonoBehaviour
{
    private ActionBasedController _baseController = null;
    [SerializeField] private Animator _anim;

    private bool _onSelected = false;
    private bool _onActivated = false;

    private InputActionProperty _selectAction = default;
    private InputActionProperty _activateAction = default;
    
    private void Start()
    {
        _baseController = GetComponent<ActionBasedController>();

        _selectAction = _baseController.selectActionValue;
        _activateAction = _baseController.activateActionValue;

        _activateAction.action.performed += StartActivationAction;
        _activateAction.action.canceled += EndActivationAction;

        _selectAction.action.performed += StartSelectAction;
        _selectAction.action.canceled += EndSelectAction;

    }

    public void SetAnimator(Animator m_anim)
    {
        this._anim = m_anim;
    }

    private void StartActivationAction(InputAction.CallbackContext m_context)
    {
        _anim.SetFloat("Activation", m_context.ReadValue<float>());
    }

    private void EndActivationAction(InputAction.CallbackContext m_context)
    {
        _anim.SetFloat("Activation", 0);
    }

    private void StartSelectAction(InputAction.CallbackContext m_context)
    {
        _anim.SetFloat("Select", m_context.ReadValue<float>());
    }

    private void EndSelectAction(InputAction.CallbackContext m_context)
    {
        _anim.SetFloat("Select", 0);
    }
}
