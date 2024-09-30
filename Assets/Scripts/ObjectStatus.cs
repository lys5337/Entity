using UnityEngine;

[System.Serializable]
public class ObjectState
{
    public string objectName;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public bool isActive;

    // Renderer 정보
    public bool hasRenderer;
    public bool isRendererEnabled;
    public Color objectColor;

    // Rigidbody 정보
    public bool hasRigidbody;
    public Vector3 velocity;
    public Vector3 angularVelocity;

    // Collider 정보
    public bool hasCollider;
    public bool isTrigger;
}
