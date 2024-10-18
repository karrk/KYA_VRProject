using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;

public class StringGrab : MonoBehaviour
{
    private LayerMask _rightHandLayer = 7;
    private SelectChecker _checker = null;

    [SerializeField] private bool _contactHand = false;

    private Transform _anchorPos = null;

    private XRBaseInteractor _interactor = null;
    [SerializeField] private float _minDist = default;
    [SerializeField] private Transform _initTr = null;

    private bool _isGrabed = false;
    private ArrowController _arrow = null;
    private XRGrabInteractable _arrowInter = null;

    [SerializeField] Transform _arrowAlign;
    [SerializeField] Transform _grabPos;

    private void Update()
    {
        if (_contactHand == false || (_checker.IsSelecting == false && _isGrabed == false))
        {
            transform.position = _initTr.position;
            return;
        }
            

        if (_checker.IsSelecting)
        {
            this.transform.position = _anchorPos.position;
            
            _isGrabed = true;

            if(_arrow != null)
            {
                _arrow.transform.LookAt(_arrowAlign);
                //_arrow.transform.rotation = Quaternion.LookRotation(_arrowAlign.transform.position - _grabPos.transform.position);
                //_arrow.transform.position = new Vector3(_arrowAlign.position.x, _arrowAlign.position.y, _grabPos.position.z);
            }
        }
        else if(_checker.IsSelecting == false && _isGrabed)
        {
            float pullDist = Vector3.Distance(_initTr.position, transform.position);
            
            if(_arrow != null)
            {
                _arrow.Shoot(pullDist * 10f);

                _arrowInter.trackRotation = true;

                _arrowInter = null;
                _arrow = null;
            }

            ResetString();

            // 화살을 쥐고있는지 확인한다.

            _isGrabed = false;
        }
    }

    private void ResetString()
    {
        transform.DOMove(_initTr.position, 0.02f).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != _rightHandLayer)
            return;

        _contactHand = true;

        if(_anchorPos == null)
        {
                _interactor = other.GetComponent<XRBaseInteractor>();
            _checker = other.GetComponent<SelectChecker>();
            _anchorPos = _checker.GetComponentInParent<ActionBasedController>().modelParent;
        }

        if(_interactor.selectTarget != null)
        {
            XRBaseInteractable obj = _interactor.selectTarget;

            if(obj.TryGetComponent<ArrowController>(out ArrowController arrow))
            { 
                _arrow = arrow;
                _arrowInter = arrow.GetComponent<XRGrabInteractable>();
                _arrowInter.trackRotation = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_contactHand == false)
            return;

        _contactHand = false;
    }
}
