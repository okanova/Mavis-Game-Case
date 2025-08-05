using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target; 
    [SerializeField] private Vector3 offset = new Vector3(0, 5f, -10f);
    [SerializeField] private float followSpeed = 5f;

    [Header("Follow Axes")]
    public bool followX = false;
    public bool followY = false;
    public bool followZ = true;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    
    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = transform.position;

        if (followX) desiredPosition.x = target.position.x + offset.x;
        if (followY) desiredPosition.y = target.position.y + offset.y;
        if (followZ) desiredPosition.z = target.position.z + offset.z;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}