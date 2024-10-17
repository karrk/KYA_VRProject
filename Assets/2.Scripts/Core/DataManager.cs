public class DataManager
{
    private PoolData _poolData = new PoolData()
    {
        InitPoolCount = 5
    };

    public int InitPoolCount;
}

public struct PoolData
{
    public int InitPoolCount;
}