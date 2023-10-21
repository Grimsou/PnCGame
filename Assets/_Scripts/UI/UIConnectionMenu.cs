using Photon.Pun;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class UIConnectionMenu : MonoBehaviour
    {
        #region Public fields

        public TMP_InputField playerNameInputField;
    
        public GameObject connectionUI;
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

        public void ManageUi(MultiplayerEventSystem.ConnexionPhase phase)  
        {
            switch (phase)
            {
                case MultiplayerEventSystem.ConnexionPhase.Connect :
                    loadingUI.SetActive(false);
                    break;
                case MultiplayerEventSystem.ConnexionPhase.Connecting : 
                    connectionUI.SetActive(false);
                    loadingUI.SetActive(true);
                    break;
                case MultiplayerEventSystem.ConnexionPhase.Disconect :
                    //Implement new behaviour
                    break;
                case MultiplayerEventSystem.ConnexionPhase.FailedConnect :
                    //Implement new beahviour
                    break;
                default: Debug.LogError("This connexion phase is not handled yet.", this);
                    break;
            }
        }
    
        #endregion
    }
}

