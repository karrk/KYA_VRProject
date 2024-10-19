using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _nav = null;
    [SerializeField] private Transform _goal = null;

    private bool _isDestToDoor = false;

    [SerializeField] private float _attackRange = 6f;
    [SerializeField] private float _arriveOffsetDist = 4f;

    private Vector3 _randOffsetDist = Vector3.zero;

    private void Start()
    {
        _goal = Manager.Instance.Data.GetDestTr(E_Dest.Corner3);
        _randOffsetDist = new Vector3(Random.Range(-6, 6), 0, Random.Range(-6, 6));
        _nav.SetDestination(_goal.position + _randOffsetDist);
    }

    private void SetNextDest()
    {
        _goal = Manager.Instance.Data.GetDestTr(E_Dest.Door);
        _nav.SetDestination(_goal.position);
        _isDestToDoor = true;
    }

    private void Update()
    {
        if(_isDestToDoor == false)
        {
            if (_nav.pathPending == false && _nav.remainingDistance <= _arriveOffsetDist)
            {
                _goal = null;
                SetNextDest();
            }
        }
    }
}
