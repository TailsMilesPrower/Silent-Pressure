/********************************************
 * Copyright(c): 2018 Victor Klepikov       *
 *                                          *
 * Profile: 	 http://u3d.as/5Fb		    *
 * Support:      http://smart-assets.org    *
 ********************************************/


using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TouchControlsKit
{
    /// <summary>
    /// isStatic = true;  - Switches a joystick in a static mode, in which it is only at the specified position.
    /// isStatic = false; - Switches a joystick in the dynamic mode, in this mode, it operates within the touch zone.
    /// </summary>

    public class TCKJoystick : AxesBasedController,
        IPointerUpHandler, IPointerDownHandler, IDragHandler
    {
        public Image joystickImage, backgroundImage;
        public RectTransform joystickRT, backgroundRT;
        
        [SerializeField, Label( "Mode" )]
        private bool isStatic = true;
                
        [Range( 1f, 9f )]
        public float borderSize = 5.85f;

        [LogicLabel( "isStatic", "Smooth Return", "Fadeout" )]
        public bool smoothReturn = false;

        [Range( 1f, 20f )]
        public float smoothFactor = 7f;

        [SerializeField]
        private Color32 joystickImageColor = new Color32( 255, 255, 255, 165 )
            , backgroundImageColor = new Color32( 255, 255, 255, 165 );

        private Vector2 startLocalPos; // I added this
        private Vector2 currentLocalDir; // I added this
        private float radius; // I added this
        private bool returning = false; // I added this 

        // Joystick Mode
        public bool IsStatic
        {
            get { return isStatic; }
            set
            {
                if( isStatic == value ) return;
                isStatic = value;
            }
        }
        
        
        // Control Awake
        public override void OnAwake()
        {
            backgroundRT = transform.GetChild( 0 ) as RectTransform;
            joystickRT = backgroundRT.GetChild( 0 ) as RectTransform;

            backgroundImage = backgroundRT.GetComponent<Image>();
            joystickImage = joystickRT.GetComponent<Image>();

            if( Application.isPlaying ) {
                joystickImage.enabled = backgroundImage.enabled = isStatic;
            }

            base.OnAwake();
        }

        // Refresh Enable
        protected override void OnApplyEnable() 
        {
            base.OnApplyEnable();
            backgroundImage.enabled = joystickImage.enabled = enable; //Old version
            
            //My version
            /*
            if (backgroundImage != null && joystickImage != null)
            {
                backgroundImage.enabled = joystickImage.enabled = enable;
            }
            */
        }


        // OnRefresh ActiveColors
        protected override void OnApplyActiveColors()
        {
            base.OnApplyActiveColors();
            joystickImage.color = GetActiveColor( joystickImageColor ); //Old version
            backgroundImage.color = GetActiveColor( backgroundImageColor ); //Old version

            //My version
            //if (joystickImage != null) joystickImage.color = GetActiveColor(joystickImageColor);
            //if (backgroundImage != null) backgroundImage.color = GetActiveColor(backgroundImageColor);
        }


        // Refresh Visible
        protected override void OnApplyVisible()
        {
            
            base.OnApplyVisible();
            joystickImage.color = visible ? GetActiveColor( joystickImageColor ) : Color.clear; //Old version
            backgroundImage.color = visible ? GetActiveColor( backgroundImageColor ) : Color.clear; //Old version

            //My version
            //if (joystickImage != null) joystickImage.color = visible ? GetActiveColor(joystickImageColor) : Color.clear;
            //if (backgroundImage != null) backgroundImage.color = visible ? GetActiveColor(backgroundImageColor) : Color.clear;

        }

        
        // I added Start();
        void Start()
        {
            radius = (backgroundRT.sizeDelta.magnitude / 2f) * borderSize / 16f;
        }
        
        
        /*
        //I addded Update();
        void Update()
        {
            if (touchDown)
            {
                float strength = Mathf.Clamp01(currentLocalDir.magnitude / radius);
                Vector2 final = currentLocalDir.magnitude > 0.01f ? (currentLocalDir.normalized * strength * sensitivity) : Vector2.zero;
                SetAxes(final);
                UpdateJoystickVisual();

                /*
                // My old version
                UpdateJoystickPosition();
                Vector2 normalized = currentDir.normalized;
                float strength = Mathf.Clamp01(currentDir.magnitude / radius);
                SetAxes(normalized * strength * sensitivity);
                
            }
            
            else
            {
                //SmoothReturn();
            }
        }
        
        */

        //protected override void OnApplyVisible() { }

        // Update Position
        
        protected override void UpdatePosition( Vector2 touchPos )
        {
            if( !axisX.enabled && !axisY.enabled )
                return;

            base.UpdatePosition( touchPos );

            if( touchDown )
            {
                UpdateCurrentPosition( touchPos );

                currentDirection = currentPosition - defaultPosition;

                float currentDistance = Vector2.Distance( defaultPosition, currentPosition );
                float touchForce = 100f;

                float calculatedBorderSize = ( backgroundRT.sizeDelta.magnitude / 2f ) * borderSize / 16f;
                
                if( currentDistance > calculatedBorderSize ) { // borderPosition 
                    currentPosition = defaultPosition + currentDirection.normalized * calculatedBorderSize;
                }                                    
                else {
                    touchForce = ( currentDistance / calculatedBorderSize ) * 100f;
                }                    

                UpdateJoystickPosition();
                SetAxes( currentDirection.normalized * touchForce / 100f * sensitivity );
            }
            else
            {
                touchDown = true;
                touchPhase = ETouchPhase.Began;

                if( isStatic == false ) {
                    UpdateTransparencyAndPosition( touchPos );
                }                    

                UpdateCurrentPosition( touchPos );
                UpdatePosition( touchPos );
                ResetAxes();
            }
        }
        

        // Get CurrentPosition
        private void UpdateCurrentPosition( Vector2 touchPos )
        {
            defaultPosition = currentPosition = backgroundRT.position;

            Vector2 worldPoint = GuiCamera.ScreenToWorldPoint( touchPos ); // Old version
            //Vector2 worldPoint = ScreenToLocalPosition(touchPos); // I added this

            if ( axisX.enabled ) currentPosition.x = worldPoint.x;
            if( axisY.enabled ) currentPosition.y = worldPoint.y;
        }
        
        // Update JoystickPosition
        private void UpdateJoystickPosition()
        {
            joystickRT.position = currentPosition;
            //joystickRT.position = backgroundRT.position + (Vector3)currentDir; // I added this (old)
            //joystickRT.anchoredPosition = currentDir; // I added this
        }

        // Update Transparency and Position for DynamicJoystick
        private void UpdateTransparencyAndPosition( Vector2 touchPos )
        {
            OnApplyVisible();
            joystickImage.enabled = backgroundImage.enabled = true;            
            joystickRT.position = backgroundRT.position = GuiCamera.ScreenToWorldPoint( touchPos ); //Old version
            //joystickRT.position = backgroundRT.position = ScreenToLocalPosition(touchPos); //My version
        }

        // SmoothReturn Run (This is the old version)
        private IEnumerator SmoothReturnRun()
        {
            bool smoothReturnIsRun = true;
            int defPosMagnitude = Mathf.RoundToInt( defaultPosition.sqrMagnitude );

            while( smoothReturnIsRun && touchDown == false )
            {                
                float smoothTime = smoothFactor * Time.smoothDeltaTime;

                currentPosition = Vector2.Lerp( currentPosition, defaultPosition, smoothTime );

                if( isStatic == false )
                {
                    joystickImage.color = Color.Lerp( joystickImage.color, Color.clear, smoothTime );
                    backgroundImage.color = Color.Lerp( backgroundImage.color, Color.clear, smoothTime );
                }

                if( Mathf.RoundToInt( currentPosition.sqrMagnitude ) == defPosMagnitude )
                {
                    currentPosition = defaultPosition;
                    smoothReturnIsRun = false;

                    if( isStatic == false ) {
                        joystickImage.enabled = backgroundImage.enabled = false;
                    }                                           
                }

                UpdateJoystickPosition();
                yield return null;
            }
        }

        /*
        // My old version of this IEnumerator
        private IEnumerator SmoothReturn()
        {
            while (Vector2.Distance(joystickRT.position, backgroundRT.position) > 0.1f)
            {
                joystickRT.position = Vector2.Lerp(joystickRT.position, backgroundRT.position, Time.deltaTime * smoothFactor);
                yield return null;
            }

            joystickRT.position = backgroundRT.position;
            if(!isStatic) joystickImage.enabled = backgroundImage.enabled = false;
        }
        
        // My new version of the IEnumerator
        private IEnumerator SmoothReturn()
        {
            returning = true;
            Vector2 start = joystickRT.anchoredPosition;
            Vector2 target = isStatic ? backgroundRT.anchoredPosition : backgroundRT.anchoredPosition;
            float t = 0f;
            
            while (t < 1f && returning)
            {
                t += Time.deltaTime * (smoothFactor * 0.5f);
                joystickRT.anchoredPosition = Vector2.Lerp(start, target, t);
                yield return null;
            }

            joystickRT.anchoredPosition = target;

            if (!isStatic)
            {
                if (joystickImage != null) joystickImage.enabled = false;
                if (backgroundImage != null) backgroundImage.enabled = false;
            }

            returning = false;
        }
        */

        
        // Control Reset
        protected override void ControlReset()
        {
            base.ControlReset();

            if( smoothReturn )
            {
                StopCoroutine( "SmoothReturnRun" );
                StartCoroutine( "SmoothReturnRun" );
            }
            else
            {
                joystickImage.enabled = backgroundImage.enabled = isStatic;
                currentPosition = defaultPosition;
                UpdateJoystickPosition();
            }
        }

        // OnPointer Down
        public void OnPointerDown( PointerEventData pointerData )
        {
            /*
            //My new version
            touchId = pointerData.pointerId;
            touchDown = true;
            touchPhase = ETouchPhase.Began;

            radius = (backgroundRT.sizeDelta.magnitude / 2f) * borderSize / 16f;

            //Vector2 local = ScreenToLocalAnchored(pointerData.position);
            Vector2 local;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundRT, pointerData.position, null, out local);

            startLocalPos = local;
            currentLocalDir = Vector2.zero;

            if (!isStatic)
            {
                backgroundRT.anchoredPosition = startLocalPos;
                joystickRT.anchoredPosition = startLocalPos;
                backgroundImage.enabled = joystickImage.enabled = true;
                //if (backgroundImage != null) backgroundImage.enabled = true;
                //if (joystickImage != null) joystickImage.enabled = true;
            }
            else
            {
                startLocalPos = backgroundRT.anchoredPosition;
                joystickRT.anchoredPosition = startLocalPos;
            }
            
            ResetAxes();

            //returning = false;

            
            // My old version
            touchDown = true;
            touchId = pointerData.pointerId;

            startTouchPos = ScreenToLocalPosition(pointerData.position);

            if (!isStatic)
            {
                Vector2 worldPos = ScreenToLocalPosition(pointerData.position);
                backgroundRT.position = worldPos;
                joystickRT.position = worldPos;
                backgroundImage.enabled = joystickImage.enabled = true;
            }

            currentDir = Vector2.zero;
            ResetAxes();
            */
            // Old version
            if ( touchDown == false )
            {
                touchId = pointerData.pointerId;
                UpdatePosition( pointerData.position );
            }
            
        }

        // OnDrag
        public void OnDrag( PointerEventData pointerData )
        {
            /*
            //My new version
            if (!touchDown) return;
            if (pointerData.pointerId != touchId) return;

            //Vector2 local = ScreenToLocalAnchored(pointerData.position);
            Vector2 local;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundRT, pointerData.position, null, out local);

            Vector2 dir = local - startLocalPos;

            if (dir.magnitude > radius)
            {
                dir = dir.normalized * radius;
            }

            currentLocalDir = dir;
            //touchPhase = ETouchPhase.Moved;
            //UpdateJoystickVisual();

            joystickRT.anchoredPosition = startLocalPos + currentLocalDir;

            float strength = Mathf.Clamp01(currentLocalDir.magnitude / radius);
            Vector2 final = currentLocalDir.normalized * strength * sensitivity;
            SetAxes(final);

            
            // My old version
            if (!touchDown) return;
            Vector2 worldPos = ScreenToLocalPosition(pointerData.position);
            currentDir = worldPos - startTouchPos;

            if (currentDir.magnitude > radius)
            {
                currentDir = currentDir.normalized * radius;
            }

            UpdateJoystickPosition();
            */
            // Old version
            if ( Input.touchCount >= touchId && touchDown ) {
                UpdatePosition( pointerData.position );
            }
            
        }

        // OnPointer Up
        public void OnPointerUp( PointerEventData pointerData )
        {
            /*
            //My new version
            if (!touchDown) return;
            
            touchDown = false;
            touchPhase = ETouchPhase.Ended;
            currentLocalDir = Vector2.zero;
            ResetAxes();

            if (smoothReturn)
            {
                StopCoroutine("SmoothReturn");
                StartCoroutine("SmoothReturn");
            }
            else
            {
                if (!isStatic)
                {
                    joystickRT.anchoredPosition = backgroundRT.anchoredPosition;
                }
                else
                {
                    if (joystickImage != null) joystickImage.enabled = false;
                    if (backgroundImage != null) backgroundImage.enabled = false;
                }
            }
            
            // My old version
            touchDown = false;
            currentDir = Vector2.zero;
            ResetAxes();

            if (smoothReturn) StartCoroutine(SmoothReturn());
            else
            {
                joystickRT.position = backgroundRT.position;
                if(!isStatic) joystickImage.enabled = backgroundImage.enabled = false;
            }
            */
            // Old version
            UpdatePosition( pointerData.position );
            ControlReset();
            
        }

        // I added this so UnifiedInput has access
        public void SetAxes(Vector2 input)
        {
            if (!gameObject.activeInHierarchy) return;

            Vector2 clamped = Vector2.ClampMagnitude(input, 1f);
            //joystickRT.anchoredPosition = clamped * (radius / 2f);

            
            if (joystickImage != null)
            {
                joystickImage.rectTransform.anchoredPosition = clamped * 0.5f;
            }
                        
            /*
            if (joystickRT != null)
            {
                joystickRT.anchoredPosition = (startLocalPos + clamped * (radius * 0.5f));
            }
            */

            axisX.SetValue(clamped.x);
            axisY.SetValue(clamped.y);
        }

        // I added this because I am not using the terrible GuiCamera script (Currently disabled)
        /*
        private Vector2 ScreenToLocalAnchored(Vector2 screenPos)
        {
            RectTransform parentRect = backgroundRT.parent as RectTransform;
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPos, null, out localPoint);
            return localPoint;
        }
        */

        // I added this
        private void UpdateJoystickVisual()
        {
            if (joystickRT != null)
            {
                joystickRT.anchoredPosition = startLocalPos + currentLocalDir;
            }
        }

    };
}