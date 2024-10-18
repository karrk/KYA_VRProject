using UnityEngine;

public class ForceColliderActivator : MonoBehaviour
{
    [SerializeField] private SphereCollider _collider;

    private void OnEnable()
    {
        _collider.enabled = true;
    }
}
