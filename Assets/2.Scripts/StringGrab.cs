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

    [SerializeField] float _pullDist = 0f;
    [SerializeField] float _maxPullDist = 1f;
    [SerializeField] float _maxPower = 100f;

    private void Update()
    {
        if (_contactHand == false || (_checker.IsSelecting == false && _isGrabed == false))
        {
            transform.position = _initTr.position;
            return;
        }
            
        if (_checker.IsSelecting)
        {
            _pullDist = Vector3.Distance(_initTr.position, transform.position);

            this.transform.position = _anchorPos.position;
            
            _isGrabed = true;

            if(_arrow != null)
            {
                _arrow.Power = ConvertDistToPower(_pullDist);
                _arrow.transform.LookAt(_arrowAlign);
            }
        }

        else if(_checker.IsSelecting == false && _isGrabed)
        {
             
            // 당겨진 거리를 반환받고,

            if(_arrow != null)
            {
                _arrow.Shoot();
                Debug.Log(_arrow.Power);

                _arrowInter.trackRotation = true;

                _arrowInter = null;
                _arrow = null;
            }

            ResetString();

            // 화살을 쥐고있는지 확인한다.

            _isGrabed = false;
        }
    }

    private float ConvertDistToPower(float m_dist)
    {
        m_dist = Mathf.Clamp(m_dist, 0, _maxPullDist);
        float power = Mathf.Pow((m_dist / _maxPullDist), 2) * _maxPower;

        return power;
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
