using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;
using _Scripts.EventSystems;

namespace _Scripts.Multiplayer
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        /// <summary>
        /// Maximum player number per room. As soon a room a full, we create a new one.
        /// </summary>
        [FormerlySerializedAs("maxPlayerPerRoom")] [SerializeField] private byte _maxPlayerPerRoom = 4;

        #endregion

        #region Private fields  

        //Current client version. 
        private string _gameVersion = "1";

        #endregion

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            // /!\ We ensure that calling PhotonNetwork.LoadLevel() will sync for each clients
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            MultiplayerEventSystem.Instance.OnChangeConnexionPhase?.Invoke(MultiplayerEventSystem.ConnexionEvent.Connect);
        }

        #endregion

        #region MonoBehaviourPun Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster() was called by PUN", this);
            MultiplayerEventSystem.Instance.OnChangeConnexionPhase?.Invoke(MultiplayerEventSystem.ConnexionEvent.Connecting);
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            MultiplayerEventSystem.Instance.OnChangeConnexionPhase?.Invoke(MultiplayerEventSystem.ConnexionEvent.Disconect);
            Debug.LogWarningFormat("OnDisconnected() was called by PUN, cause :", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.LogFormat("OnJoinRandomFailed was called by PUN : No random room available, so we create one. \nCalling PhotonNetwork.CreateRoom()");
            PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = _maxPlayerPerRoom});
        }

        public override void OnJoinedRoom()
        {
            MultiplayerEventSystem.Instance.OnChangeConnexionPhase?.Invoke(MultiplayerEventSystem.ConnexionEvent.Connect);
            Debug.Log("OnJoinedRoom() was called by PUN. Now this client is in a room", this);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Lance la connexion :
        /// - si déjà connecté, on essaye de rejoindre une room
        /// - si ce n'est pas le cas, on connecte cette aplication au Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = _gameVersion;
            }
        }

        #endregion
        
        
    }
}
