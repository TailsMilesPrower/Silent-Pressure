using UnityEngine;
using UnityEngine.EventSystems;

namespace TouchControlsKit
{
    [RequireComponent(typeof(RectTransform))]
    public class TCKOverlayFix : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private RectTransform rectTransform;
        private Canvas rootCanvas;
        private AxesBasedController controller;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            rootCanvas = GetComponent<Canvas>();
            controller = GetComponent<AxesBasedController>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            UpdateTouch(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdateTouch(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (controller != null) controller.SendMessage("ControlReset", SendMessageOptions.DontRequireReceiver);
        }

        private void UpdateTouch(PointerEventData eventData)
        {
            if (rootCanvas == null || controller == null) return;

            Vector2 localPos;
            Camera cam = rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : rootCanvas.worldCamera;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, cam, out localPos))
            {
                Vector2 size = rectTransform.sizeDelta;
                Vector2 normalized = new Vector2(localPos.x / (size.x * 0.5f), localPos.y / (size.y * 0.5f));
                normalized = Vector2.ClampMagnitude(normalized, 1f);

                controller.SendMessage("Set Axes", normalized, SendMessageOptions.DontRequireReceiver);
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

