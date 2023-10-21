using Photon.Pun;
using TMPro;
using UnityEngine;
using _Scripts.EventSystems;

namespace _Scripts.UI
{
    public class UIConnectionMenu : MonoBehaviour
    {
        #region Public fields

        [Header("| UI Objects References |")]
        [Space(5)]
        public TMP_InputField playerNameInputField;
        [Space(2)]
        public GameObject connectionUI;
        [Space(2)]
        public GameObject loadingUI;
    
        #endregion
    
        #region Private constant
    
        private const string playerNamePrefKey = "PlayerName";
    
        #endregion
    
        #region MonoBehaviour CallBacks

        private void Start()
        {
            string defaultName = string.Empty;
    
            if (playerNameInputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    //Debug.LogWarning($"Player name registered is : {PlayerPrefs.GetString("PlayerName")}");
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    playerNameInputField.text = defaultName;
                }
                else
                {
                    playerNameInputField.text = "";
                }
            }
    
            PhotonNetwork.NickName = defaultName;
        }
        
        
        private void OnEnable()
        {
            MultiplayerEventSystem.Instance.OnChangeConnexionPhase += ManageUi;
        }

        private void OnDisable()
        {
            MultiplayerEventSystem.Instance.OnChangeConnexionPhase -= ManageUi;
        }

    
        #endregion
    
        #region Public Methods
    
        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty !");
            }
    
            PhotonNetwork.NickName = value;
            PlayerPrefs.SetString(playerNamePrefKey, value);
        }

        private void ManageUi(MultiplayerEventSystem.ConnexionEvent @event)  
        {
            switch (@event)
            {
                case MultiplayerEventSystem.ConnexionEvent.Connect :
                    loadingUI.SetActive(false);
                    break;
                case MultiplayerEventSystem.ConnexionEvent.Connecting : 
                    connectionUI.SetActive(false);
                    loadingUI.SetActive(true);
                    break;
                case MultiplayerEventSystem.ConnexionEvent.Disconect :
                    //Implement new behaviour
                    break;
                case MultiplayerEventSystem.ConnexionEvent.FailedConnect :
                    //Implement new beahviour
                    break;
                default: Debug.LogError("This connexion @event is not handled yet.", this);
                    break;
            }
        }
    
        #endregion
    }
}

