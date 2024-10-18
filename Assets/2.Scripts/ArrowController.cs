using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable _interactor = null;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _head;

    private void Start()
    {
        _interactor.onSelectExited.AddListener((XRBaseInteractor _) => { Selected(); });
    }

    private void Selected()
    {
        _rb.isKinematic = false;
    }

    public void Shoot(float m_power)
    {
        _rb.AddForce(_head.forward * m_power, ForceMode.Impulse);
    }
}
