using UnityEngine;
using _Project.Scripts.Core;

public class InputTest : MonoBehaviour
{
    private void OnEnable()
    {
        InputEvents.OnZoom += HandleZoom;
        InputEvents.OnClick += HandleClick;
        InputEvents.OnHold += HandleHold;
        InputEvents.OnRelease += HandleRelease;
        InputEvents.OnDrag += HandleDrag;
    }

    private void OnDisable()
    {
        InputEvents.OnZoom -= HandleZoom;
        InputEvents.OnClick -= HandleClick;
        InputEvents.OnHold -= HandleHold;
        InputEvents.OnRelease -= HandleRelease;
        InputEvents.OnDrag -= HandleDrag;
    }

    #region ZOOM

    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 10f;
    [SerializeField] private float maxZoom = 60f;
    
    private void HandleZoom(float delta)
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float zoomAmount = delta * zoomSpeed;

        if (cam.orthographic)
        {
            float newSize = Mathf.Clamp(cam.orthographicSize - zoomAmount, minZoom, maxZoom);

            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
            Vector3 direction = mouseWorldPos - cam.transform.position;

            float moveFactor = delta > 0 ? 1.2f : 0.5f;
            cam.transform.position += direction.normalized * zoomAmount * moveFactor;

            cam.orthographicSize = newSize;
        }
        else
        {
            float newFOV = Mathf.Clamp(cam.fieldOfView - zoomAmount, minZoom, maxZoom);

            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane + 1f));
            Vector3 direction = mouseWorldPos - cam.transform.position;

            float moveFactor = delta > 0 ? 1.2f : 0.5f;
            cam.transform.position += direction.normalized * zoomAmount * moveFactor;

            cam.fieldOfView = newFOV;
        }
    }

    #endregion

    #region CLICK

    private void HandleClick(Vector3 pos)
    {
        Debug.Log($"[Test] Clicked at: {pos}");
    }
    
    private void HandleHold(Vector3 pos)
    {
        Debug.Log($"[Test] Holded at: {pos}");
    }

    private void HandleRelease(Vector3 pos)
    {
        Debug.Log($"[Test] Released at: {pos}");
    }
    
    private void HandleDrag(Vector2 delta)
    {
        // World pozisyonunu hesapla
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // Kamera ile obje arasındaki mesafeye göre ayarla

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = worldPos;

        Debug.Log($"[Drag] Moving to: {worldPos}");
    }

    #endregion
   
}