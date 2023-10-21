using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.EventSystems
{
    public class MultiplayerEventSystem : MonoBehaviour
    {
        #region ConnexionEvent enum

        public enum ConnexionEvent
        {
            Connecting,
            Connect,
            Disconect,
            FailedConnect
        }


        #endregion

        public UnityAction<ConnexionEvent> OnChangeConnexionPhase;

        #region Event system singleton

        public static MultiplayerEventSystem Instance;

        #endregion

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                Debug.Log("Instance of MultiplayeEventSystem has been created.");
                DontDestroyOnLoad(Instance);
            }
        }


        #endregion
    }
}
