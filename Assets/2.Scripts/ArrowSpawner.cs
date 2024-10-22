using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnTr = null;

    private void Start()
    {
        Manager.Instance.Data.ArrowSpawner = this;
        ArrowController arrow = GetComponentInChildren<ArrowController>();
    }

    public void OutedArrow()
    {
        ArrowController newArrow = Manager.Instance.Pool.GetObject(E_PoolType.Arrow, transform).
            GameObject.GetComponent<ArrowController>();

        newArrow.transform.position = _spawnTr.position;
        newArrow.transform.rotation = _spawnTr.rotation;

    }
}
