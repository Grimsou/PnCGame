using Photon.Pun;
using TMPro;
using UnityEngine;
using _Scripts.EventSystems;

namespace _Scripts.UI
{
    public class UiConnectionMenu : MonoBehaviour
    {
        #region Public fields

        [Header("| UI Objects References |")]
        [Space(5)]
        public TMP_InputField inputPlayerName;
        [Space(2)]
        public GameObject connectionUI;
        [Space(2)]
        public GameObject loadingUI;
    
        #endregion
    
        #region Private constant
    
        private const string PlayerNameKey = "PlayerName";
    
        #endregion
    
        #region MonoBehaviour CallBacks

        private void Start()
        {
            string defaultName = string.Empty;
    
            if (inputPlayerName != null)
            {
                if (PlayerPrefs.HasKey(PlayerNameKey))
                {
                    //Debug.LogWarning($"Player name registered is : {PlayerPrefs.GetString("PlayerName")}");
                    defaultName = PlayerPrefs.GetString(PlayerNameKey);
                    inputPlayerName.text = defaultName;
                }
                else
                {
                    inputPlayerName.text = "";
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
            PlayerPrefs.SetString(PlayerNameKey, value);
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

