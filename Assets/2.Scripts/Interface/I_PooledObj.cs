using UnityEngine;

public interface IPooledObj
{
    public GameObject GameObject => this.GameObject;
    public E_PoolType MyPoolType { get; }
    public void Return(IPooledObj m_obj);
    public void SetPoolType(E_PoolType m_type);
}