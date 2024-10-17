using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private int _initCount = default;
    private IPooledObj _prefab = null;

    private List<IPooledObj> _list = null;
    private Transform _directory = null;

    private E_PoolType _type = E_PoolType.None;

    public void Init(E_PoolType m_type, IPooledObj m_prefab)
    {
        _list = new List<IPooledObj>();
        this._type = m_type;
        _initCount = Manager.Instance.Data.InitPoolCount;
        this._prefab = m_prefab;

        CreateDirectory();
        Create();
    }

    private void CreateDirectory()
    {
        _directory = new GameObject().transform;
        _directory.SetParent(Manager.Instance.Pool.MainPoolDir);
        _directory.name = $"{_prefab}s Pool";
    }

    public void Create()
    {
        for (int i = 0; i < _initCount; i++)
        {
            IPooledObj newObj = GameObject.Instantiate(_prefab.GameObject).GetComponent<IPooledObj>();
            newObj.GameObject.SetActive(false);
            newObj.SetPoolType(_type);
            newObj.GameObject.transform.SetParent(_directory);

            _list.Add(newObj);
        }
    }

    public IPooledObj GetObject()
    {
        if(_list.Count <= 0)
        {
            _initCount *= 2;
            Create();
        }

        IPooledObj obj = _list[_list.Count - 1];
        obj.GameObject.SetActive(true);

        return obj;
    }

    public void ReturnObj(IPooledObj m_obj)
    {
        m_obj.GameObject.SetActive(false);
        m_obj.GameObject.transform.SetParent(_directory);
        _list.Add(m_obj);
    }


}
