using UnityEngine;

public class DataManager : CoreComponent
{
    protected override void InitOptions()
    {
        SetDestData();
        DoorHP = 3;
    }

    private PoolData _poolData = new PoolData()
    {
        InitPoolCount = 5
    };

    public int InitPoolCount => _poolData.InitPoolCount;

    private DestinationData _destData = new DestinationData();

    private void SetDestData()
    {
        this._destData.Destinations =
            Manager.Instance.transform.GetChild(0).GetComponentsInChildren<Transform>();
    }

    public Transform GetDestTr(E_Dest e_dest)
    {
        return _destData.Destinations[(int)e_dest];
    }

    public int DoorHP = default;

    public ArrowSpawner ArrowSpawner = null;
}

public struct PoolData
{
    public int InitPoolCount;
}

public struct DestinationData
{
    public Transform[] Destinations;
}