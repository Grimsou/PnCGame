using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiplayerEventSystem : MonoBehaviour
{
    public enum ConnexionPhase
    {
        Connecting,
        Connect,
        Disconect, 
        FailedConnect
    }
    
    public static MultiplayerEventSystem Instance;

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

    // Start is called before the first frame update
    public UnityAction<ConnexionPhase> OnChangeConnexionPhase;
}
