using UnityEngine;

namespace _Project.Scripts.Core
{
    public class InputManager : MonoBehaviour
    {
        [Header("Swipe Settings")]
        [SerializeField] private float swipeThreshold = 50f;

        private Vector2 touchStartPos;

        private bool isHolding = false;

        private void Update()
        {
            #region Mouse or Touch Press
            if (Input.GetMouseButtonDown(0))
            {
                touchStartPos = Input.mousePosition;
                isHolding = true;

                InputEvents.TriggerClick(touchStartPos);
            }
            #endregion

            #region Holding
            if (isHolding && Input.GetMouseButton(0))
            {
                InputEvents.TriggerHold(Input.mousePosition);
            }
            #endregion

            #region Release & Swipe
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 releasePos = Input.mousePosition;
                InputEvents.TriggerRelease(releasePos);

                isHolding = false;

                Vector2 delta = releasePos - touchStartPos;
                if (delta.magnitude > swipeThreshold)
                {
                    Vector2 direction = delta.normalized;
                    InputEvents.TriggerSwipe(direction);
                }
            }
            #endregion

            #region Zoom (Mouse Wheel or Pinch)
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                InputEvents.TriggerZoom(scroll);
            }
            #endregion

            #region Drag
            if (Input.GetMouseButton(0))
            {
                Vector2 dragDelta = (Vector2)Input.mousePosition - touchStartPos;
                InputEvents.TriggerDrag(dragDelta);
            }
            #endregion
        }
    }
}