using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjPoolManager : CoreComponent
{
    private Dictionary<E_PoolType, ObjectPool> _poolTable = null;
    [SerializeField] private List<PoolPrefab> _prefabs = null;

    private Transform _mainPoolDirectory = null;
    public Transform MainPoolDir => _mainPoolDirectory;

    protected override void InitOptions()
    {
        _poolTable = new Dictionary<E_PoolType, ObjectPool>();
        _mainPoolDirectory = new GameObject().transform;
        _mainPoolDirectory.SetParent(Manager.Instance.transform);
        _mainPoolDirectory.name = "Pools";

        //CreatePools();
    }

    private void CreatePools()
    {
        for (int i = 0; i < _prefabs.Count; i++)
        {
            PoolPrefab item = _prefabs[i];
            
            if(item.PoolType == E_PoolType.None)
            {
                throw new System.Exception("풀 : None형식이 정의됨");
            }    
            else if (item.Prefab.TryGetComponent<IPooledObj>(out IPooledObj obj))
            {
                ObjectPool newPool = new ObjectPool();
                _poolTable.Add(item.PoolType, newPool);

                newPool.Init(item.PoolType, obj);
            }
            else
                throw new System.Exception("풀링형식이 아닙니다.");
        }
    }

    public IPooledObj GetObject(E_PoolType m_type)
    {
        return _poolTable[m_type].GetObject();
    }

    public IPooledObj GetObjecgt(E_PoolType m_type,Transform m_parent)
    {
        IPooledObj obj = GetObject(m_type);
        obj.GameObject.transform.SetParent(m_parent);

        return obj;
    }

    public void ReturnObject(IPooledObj m_obj)
    {
        _poolTable[m_obj.MyPoolType].ReturnObj(m_obj);
    }
}

[System.Serializable]
public class PoolPrefab
{
    public E_PoolType PoolType;
    public GameObject Prefab;
}
