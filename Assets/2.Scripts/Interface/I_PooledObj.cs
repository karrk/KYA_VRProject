using UnityEngine;

public interface IPooledObj
{
    public GameObject GameObject { get; }
    public E_PoolType MyPoolType { get; }
    public void Return();
}