using UnityEngine;


public class PositionMarker : MonoBehaviour
{

    private Transform _transform;

    void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    public Vector2 GetPosition() => _transform.position;

}
