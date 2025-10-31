/********************************************
 * Copyright(c): 2018 Victor Klepikov       *
 *                                          *
 * Profile: 	 http://u3d.as/5Fb		    *
 * Support:      http://smart-assets.org    *
 ********************************************/


using UnityEngine;

namespace TouchControlsKit
{
    [RequireComponent( typeof( Camera ) )]
    public sealed class GuiCamera : MonoBehaviour
    {
        public static Camera getCamera { get; private set; }
        public static Transform getTransform { get; private set; }


        // Awake
        void Awake()
        {
            getTransform = transform;
            getCamera = GetComponent<Camera>();
        }                 

        /*
        // ScreenToWorldPoint (This is the old one from the package)
        public static Vector2 ScreenToWorldPoint( Vector2 position )
        {
            return getCamera.ScreenToWorldPoint( position );
        }
        */

        // ScreenToWorldPoint (New - my version of it)
        public static Vector2 ScreenToWorldPoint(Vector2 position)
        {
            if (getCamera == null || !getCamera.enabled) return position;
            return getCamera.ScreenToWorldPoint(position);
        }
    };
}