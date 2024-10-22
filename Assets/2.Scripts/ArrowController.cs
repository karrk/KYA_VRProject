using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowController : MonoBehaviour, IPooledObj
{
    [SerializeField] private XRGrabInteractable _interactor = null;
    [SerializeField] private Rigidbody _rb = null;
    [SerializeField] public Transform Head = null;
    [SerializeField] public Transform Tail = null;

    [SerializeField] private CapsuleCollider _collider = null;
    [SerializeField] private LineRenderer _line = null;

    private int _pointCount = 50;
    private float _moveInterval = 0.15f;

    private Vector3 _curPos = Vector3.zero;
    private Vector3 _curVelocity = Vector3.zero;

    private float _maxPower = 20f;

    private float _power = 0f;
    public float Power
    {
        get { return _power; }
        set
        {
            this._power = value;
            RenderLinePath();
        }
    }

    public GameObject GameObject => this.gameObject;

    public E_PoolType MyPoolType => E_PoolType.Arrow;

    private bool _isContacted = false;

    private void OnEnable()
    {
        _collider.enabled = true;
        _rb.useGravity = true;
    }

    private void Start()
    {
        _interactor.onSelectEntered.AddListener((XRBaseInteractor _) => { SelectReady(); });
        _interactor.onSelectExited.AddListener((XRBaseInteractor _) => { Selected(); });
        _line.positionCount = _pointCount;
    }


    private void SelectReady()
    {
        Manager.Instance.Data.ArrowSpawner.OutedArrow();
        _line.enabled = true;
    }

    private void Selected()
    {
        _rb.isKinematic = false;
    }

    public void Shoot()
    {
        _line.enabled = false;
        _rb.isKinematic = false;

        if (_power >= _maxPower)
            _rb.useGravity = false;

        _rb.AddForce(Head.forward * _power, ForceMode.Impulse);
    }

    private void RenderLinePath()
    {
        _curPos = Head.position;
        _curVelocity = Head.forward * _power;

        float gravityInfluence = 1f - Mathf.Clamp01(_power / _maxPower);

        for (int i = 0; i < _pointCount; i++)
        {
            _line.SetPosition(i, _curPos);

            _curPos += _curVelocity * _moveInterval + 0.5f * Physics.gravity * Mathf.Pow(_moveInterval, 2) * gravityInfluence;

            _curVelocity += Physics.gravity * _moveInterval * gravityInfluence;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        
        _rb.isKinematic = true;
        //transform.position = collision.GetContact(0).point;

        if(collision.collider.CompareTag("Monster"))
        {
            MonsterMovement monster = collision.collider.GetComponent<MonsterMovement>();
            monster.State = E_MonsterState.OnDamaged;
            _rb.velocity = Vector3.zero;
            transform.parent = monster.transform;
            monster.AddArrow(this);
        }

        _collider.enabled = false;
    }

    public void Return()
    {
        Manager.Instance.Pool.ReturnObject(this);
    }
}
