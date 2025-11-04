using UnityEngine;
using UnityEngine.EventSystems;

public class TouchCameraLook : MonoBehaviour //, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("References")]
    public Transform playerBody;
    public RectTransform joystickZone;

    [Header("Settings")]
    public float touchLookSensitivity = 0.15f;
    //public float mouseLookSensitivity = 2f;
    public float minPitch = -60f;
    public float maxPitch = 60f;

    private float pitch = 0f;

    private bool usingTouch = false;
    private int activeTouchId = -1;
    private Vector2 lastTouchPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 angles = transform.localEulerAngles;
        pitch = angles.x > 180 ? angles.x - 360 : angles.x;
        /*
        pitch = transform.localEulerAngles.x;
        if (pitch > 180f) pitch -= 360f;
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (!usingTouch) StartTouchMode();
            //if (!usingTouch) usingTouch = true;

            for (int i = 0; i < Input.touchCount; ++i)
            {
                Touch t = Input.GetTouch(i);

                if (IsPointerOverUI(t.position)) continue;

                if (activeTouchId == -1 && t.phase == TouchPhase.Began || t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
                {
                    activeTouchId = t.fingerId;
                    lastTouchPos = t.position;
                    usingTouch = true;
                }

                if (t.fingerId == activeTouchId)
                {
                    if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
                    {
                        //Vector2 delta = t.position - lastTouchPos;
                        Vector2 delta = t.deltaPosition;
                        lastTouchPos = t.position;
                        ApplyTouchLook(delta);
                    }
                    
                    else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                    {
                        activeTouchId = -1;
                        usingTouch = false;
                    }
                }
            }
        }

        else
        {
            if (usingTouch) StopTouchMode();
            //usingTouch = false;
            activeTouchId = -1;
        }

    }

    void ApplyTouchLook(Vector2 delta)
    {
        //float yaw = delta.x * touchLookSensitivity;
        //playerBody.Rotate(Vector3.up * yaw);
        playerBody.Rotate(Vector3.up * delta.x * touchLookSensitivity);

        pitch -= delta.y * touchLookSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        //transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }

    bool IsPointerOverUI(Vector2 screenPos)
    {
        if (joystickZone ==  null) return false;
        return RectTransformUtility.RectangleContainsScreenPoint(joystickZone, screenPos, null);
    }


    void StartTouchMode()
    {
        usingTouch = true;
        activeTouchId = -1;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = true;
    }

    void StopTouchMode()
    {
        usingTouch = false;
        activeTouchId = -1;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(joystickZone, eventData.position)) return;

        usingTouch = true;
        lastTouchPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(usingTouch) return;

        Vector2 delta = eventData.position - lastTouchPos;
        lastTouchPos = eventData.position;

        playerBody.Rotate(Vector3.up * delta.x * touchLookSensitivity);

        pitch -= delta.y * touchLookSensitivity;
        pitch = Mathf.Clamp(pitch, -60f, 60f);
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        usingTouch = false;
    }

}
