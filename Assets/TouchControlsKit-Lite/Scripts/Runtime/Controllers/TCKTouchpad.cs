/********************************************
 * Copyright(c): 2018 Victor Klepikov       *
 *                                          *
 * Profile: 	 http://u3d.as/5Fb		    *
 * Support:      http://smart-assets.org    *
 ********************************************/


using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TouchControlsKit
{
    public class TCKTouchpad : AxesBasedController,
        IPointerUpHandler, IPointerDownHandler, IDragHandler, IPointerEnterHandler
    {
        GameObject prevPointerPressGO;
        private Vector2 startTouchPos; // I added this
        private bool started = false; // I added this

        [SerializeField] private float maxRadius = 120f; // I added this

        // Set Visible
        protected override void OnApplyVisible()
        { }

        // I added this Update(), so the touch
        void Update()
        {
            if (touchDown)
            {
                //Vector2 touchPos = currentPosition;
                UpdatePosition(currentPosition);
            }
            else if (started)
            {
                ControlReset();
                started = false;
            }
        }

        // Update Position
        protected override void UpdatePosition( Vector2 touchPos )
        {
            if (!gameObject.activeInHierarchy) return; //I added this to prevent snapping when inactive

            if ( !axisX.enabled && !axisY.enabled )
                return;

            base.UpdatePosition( touchPos );

            //This is my if statement
            if (touchDown)
            {
                currentPosition = touchPos;
                currentDirection = currentPosition - startTouchPos;

                float distance = currentDirection.magnitude;

                //Vector2 normalized = currentDirection.normalized;
                //float maxRadius = 100f;

                if (distance > maxRadius) currentDirection = currentDirection.normalized * maxRadius;
                
                float strength = Mathf.Clamp01(distance / maxRadius);
                Vector2 final = currentDirection.normalized * strength * sensitivity;

                //SetAxes(currentDirection.normalized * (distance / maxRadius) * sensitivity);
                SetAxes(final);
                touchPhase = ETouchPhase.Moved;
            }
            else
            {
                touchDown = true;
                started = true;
                touchPhase = ETouchPhase.Began;

                startTouchPos = touchPos;
                currentPosition = touchPos;

                ResetAxes();
            }

            /* This is the old if(touchdown) statement
            if( touchDown )
            {
                if( axisX.enabled ) currentPosition.x = touchPos.x;
                if( axisY.enabled ) currentPosition.y = touchPos.y;

                currentDirection = currentPosition - defaultPosition;
                
                float touchForce = Vector2.Distance( defaultPosition, currentPosition ) * 2f;
                defaultPosition = currentPosition;

                SetAxes( currentDirection.normalized * touchForce / 100f * sensitivity );
            }
            else
            {
                touchDown = true;
                touchPhase = ETouchPhase.Began;

                currentPosition = defaultPosition = touchPos;
                UpdatePosition( touchPos );
                ResetAxes();
            }
            */
        }
               
        
        // OnPointer Enter
        public void OnPointerEnter( PointerEventData pointerData )
        {
            if( pointerData.pointerPress == null )
                return;

            if( pointerData.pointerPress == gameObject )
            {
                OnPointerDown( pointerData );
                return;
            }

            var btn = pointerData.pointerPress.GetComponent<TCKButton>();
            if( btn != null && btn.swipeOut )
            {
                prevPointerPressGO = pointerData.pointerPress;
                pointerData.pointerDrag = gameObject;
                pointerData.pointerPress = gameObject;
                OnPointerDown( pointerData );
            }
        }

        // OnPointer Down (this is the old version)
        /*
        public void OnPointerDown( PointerEventData pointerData )
        {
            if( touchDown == false )
            {
                touchId = pointerData.pointerId;
                UpdatePosition( pointerData.position );
            }
        }
        */

        public void OnPointerDown(PointerEventData pointerData)
        {
            if (!touchDown)
            {
                touchId = pointerData.pointerId;
                startTouchPos = pointerData.position;
                currentPosition = pointerData.position;
                touchDown = true;
                started = true;
                touchPhase = ETouchPhase.Began;

                ResetAxes();
            }
        }


        // OnDrag (this is the old version)
        /*
        public void OnDrag( PointerEventData pointerData )
        {
            if( Input.touchCount >= touchId && touchDown )
            {
                UpdatePosition( pointerData.position );
                StopCoroutine( "UpdateEndPosition" );
                StartCoroutine( "UpdateEndPosition", pointerData.position );
            }

            // I added this check
            if (gameObject.activeInHierarchy) StartCoroutine("UpdateEndPosition", pointerData.position);
        }
        */

        // I made a new OnDrag
        public void OnDrag(PointerEventData pointerData)
        {
            if (!touchDown) return;
            if(pointerData.pointerId != touchId) return;

            currentPosition = pointerData.position;
            UpdatePosition(currentPosition);

            /*
            if (Input.touchCount > 0 && touchDown)
            {
                currentPosition = pointerData.position;
                UpdatePosition(currentPosition);
            }
            */
        }


        // Update EndPosition (This is not used)
        /*
        private IEnumerator UpdateEndPosition( Vector2 position )
        {
            for( float el = 0f; el < .0025f; el += Time.deltaTime )
                yield return null;
            
            if( touchDown )
                UpdatePosition( position );
            else
                ControlReset();
        }
        */

        // OnPointer Up
        public void OnPointerUp( PointerEventData pointerData )
        {
            if( prevPointerPressGO != null )
            {
                ExecuteEvents.Execute( prevPointerPressGO, pointerData, ExecuteEvents.pointerUpHandler );
                prevPointerPressGO = null;
            }

            touchDown = false; // I added this
            touchPhase= ETouchPhase.Ended; // I added this
            ControlReset(); 
            started = false; // I added this
        }
    };
}