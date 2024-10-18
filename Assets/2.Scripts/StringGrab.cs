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

    [SerializeField] private float _minDist;
    [SerializeField] private Transform _initTr;

    private bool _isGrabed = false;

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
        }
        else if(_checker.IsSelecting == false && _isGrabed)
        {
            //float pullDist = Vector3.Distance(_initTr.position, transform.position);
            //Debug.Log(pullDist);

            //if (pullDist <= _minDist)
            //{
            //    ResetString();
            //}
            //else
            //{

            //}

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
            _checker = other.GetComponent<SelectChecker>();
            _anchorPos = _checker.GetComponentInParent<ActionBasedController>().modelParent;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_contactHand == false)
            return;

        _contactHand = false;
    }
}
