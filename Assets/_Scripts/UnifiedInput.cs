using UnityEngine;
using TouchControlsKit;

public class UnifiedInput : MonoBehaviour
{
    [Header("Touch ControlsKit Setup")]
    public string moveJoystickName = "Joystick";
    public string lookTouchpadName = "Touchpad";

    [Header("Input Output")]
    public float horizontal;
    public float vertical;
    public float mouseX;
    public float mouseY;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = 0f;
        float v = 0f;
        float mx = 0f;
        float my = 0f;

#if UNITY_EDITOR || UNITY_STANDALONE
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        mx = Input.GetAxis("Mouse X");
        my = Input.GetAxis("Mouse Y");

        //bool usedTCK = false;
#endif

#if UNITY_ANDROID
        if (TCKInput.isActive)
        {
            if (TCKInput.CheckController(moveJoystickName))
            {
                Vector2 joy = TCKInput.GetAxis(moveJoystickName);
                if (joy.sqrMagnitude > 0.0025f) //(Mathf.Abs(joy.x) > 0.05f || Mathf.Abs(joy.y) > 0.05f)
                {
                    h = joy.x;
                    v = joy.y;
                    //usedTCK = true;
                }
            }

            if (TCKInput.CheckController(lookTouchpadName))
            {
                Vector2 look = TCKInput.GetAxis(lookTouchpadName);
                if (look.sqrMagnitude > 0.0025f) //(Mathf.Abs(look.x) > 0.05f || Mathf.Abs(look.y) > 0.05f)
                {
                    mx = look.x * 3f;
                    my = look.y * 3f;
                    //usedTCK = true;
                }
            }

        }
#endif

        /*
        if (!usedTCK)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            mx = Input.GetAxis("Mouse X");
            my = Input.GetAxis("Mouse Y");
        }
        */

        horizontal = Mathf.Clamp(h, -1f, 1f);
        vertical = Mathf.Clamp(v, -1f, 1f);
        mouseX = mx;
        mouseY = my;
        
        /*
        if (TCKInput.isActive && TCKInput.CheckController(moveJoystickName))
        {
            var controller = GameObject.Find(moveJoystickName)?.GetComponent<TCKJoystick>();
            if (controller != null && controller.gameObject.activeInHierarchy)
            {
                controller.SetAxes(new Vector2(horizontal, vertical));
            }
        }
        */
    }
}
