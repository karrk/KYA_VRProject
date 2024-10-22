using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour, IPooledObj
{
    [SerializeField] private NavMeshAgent _nav = null;
    [SerializeField] private Transform _goal = null;

    private bool _isArriveCornerPos = false;

    [SerializeField] private float _attackRange = 6f;
    [SerializeField] private float _arriveOffsetDist = 6f;

    private Vector3 _randOffsetDist = Vector3.zero;

    [SerializeField] private Animator _anim = null;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _attackDelay = 3f;
    [SerializeField] private int _power = 1;

    private Coroutine _moveRoutine = null;
    private Coroutine _attackRoutine = null;

    private Transform _door = null;
    [SerializeField] private int _hp = 2;

    private List<ArrowController> _contactedArrows = new List<ArrowController>();

    private E_MonsterState _state = E_MonsterState.Idle;
    public E_MonsterState State
    {
        get { return _state; }
        set
        {
            if (_state == value) return;

            _state = value;

            switch (_state)
            {
                case E_MonsterState.Idle:
                    break;
                case E_MonsterState.Move:
                    MoveAction();
                    break;
                case E_MonsterState.Attack:
                    AttackAction();
                    break;
                case E_MonsterState.OnDamaged:
                    OnDamagedAction();
                    break;
                case E_MonsterState.Dead:
                    DeadAction();
                    break;
            }
        }
    }

    public E_PoolType MyPoolType => E_PoolType.Monster;

    public GameObject GameObject => this.gameObject;

    private void Start()
    {
        Init();
    }

    public void AddArrow(ArrowController m_arrow)
    {
        this._contactedArrows.Add(m_arrow);
    }

    public void Init()
    {
        if (_door == null)
            _door = Manager.Instance.Data.GetDestTr(E_Dest.Door);

        _nav.speed = _moveSpeed;
        int randomCornerNumber = Random.Range((int)E_Dest.Corner1, (int)E_Dest.Corner4 + 1);
        _goal = Manager.Instance.Data.GetDestTr((E_Dest)randomCornerNumber);
        _randOffsetDist = new Vector3(Random.Range(-6f, 6f), 0, Random.Range(-6f, 6f));
        _nav.SetDestination(_goal.position + _randOffsetDist);
        State = E_MonsterState.Move;
    }

    private void MoveAction()
    {
        StopPrevCoroutines();
        _anim.SetBool("Move", true);
        _moveRoutine = StartCoroutine(MoveRoutine());
        _nav.isStopped = false;
    }

    private void OnDamagedAction()
    {
        StopPrevCoroutines();
        _nav.speed = 0.1f;

        this._hp -= 1;
        _anim.SetTrigger("OnDamaged");

        if (this._hp <= 0)
            State = E_MonsterState.Dead;
        else
            State = E_MonsterState.Move;
    }


    // 경로이동은 네비가 처리. 판별을 위한 로직
    private IEnumerator MoveRoutine() 
    {
        while (true)
        {
            // 경로 추적중이 아니고, 목적지 근처에 도달했다면
            if (_nav.pathPending == false && _nav.remainingDistance <= _arriveOffsetDist)
            {
                // 현재 목적지를 날리고,
                // 다음 경로를 찾는다.

                if(_goal != _door)
                {
                    _goal = _door;
                    _randOffsetDist = Vector3.zero;
                    _nav.SetDestination(_goal.position);
                }
                else // 문앞에 도착함
                {
                    _nav.isStopped = true;
                    State = E_MonsterState.Attack;
                    break;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void StopPrevCoroutines()
    {
        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);

        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);
    }


    private void AttackAction()
    {
        StopPrevCoroutines();

        _attackRoutine = StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            if(Vector3.Distance(_door.position,transform.position) > _attackRange)
            {
                State = E_MonsterState.Move;
                break;
            }

            transform.forward = _door.forward;
            _anim.SetTrigger("Attack");
            _anim.SetBool("Move",false);

            yield return new WaitForSeconds(_attackDelay);
        }
    }

    private void DeadAction()
    {
        _anim.SetBool("Dead", true);
    }

    public void OnAttacked()
    {
        Manager.Instance.Data.DoorHP -= _power;
    }

    public void Recover()
    {
        _nav.speed = _moveSpeed;
    }

    public void FinishDeadAction()
    {
        //Debug.Log("죽음 애니메이션 종료됨");
        Return();
    }

    private void ReturnArrows()
    {
        foreach (var arrow in _contactedArrows)
        {
            arrow.Return();
        }

        _contactedArrows.Clear();
    }

    public void Return()
    {
        ReturnArrows();

        Manager.Instance.Pool.ReturnObject(this);
    }

}
