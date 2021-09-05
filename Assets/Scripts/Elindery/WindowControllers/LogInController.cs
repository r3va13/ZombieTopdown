using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elindery.WindowControllers
{
    public class LogInController : MonoBehaviour
    {
#region Singleton
        static LogInController _instance;
        public static LogInController Instance
        {
            get
            {
                if (_instance) return _instance;
            
                _instance = GameObject.Find("Root").transform.Find("LogInController").GetComponent<LogInController>();
                _instance.GetWhatYouNeed();
            
                return _instance;
            }
        }
#endregion

        Button _connectBtn;
        TMP_Text _loginLbl;
    
        void GetWhatYouNeed()
        {
            _loginLbl = transform.Find("Text (TMP)").GetComponent<TMP_Text>();
            _connectBtn = transform.Find("ButtonConnect").GetComponent<Button>();
        
            OnLoginTextChanged(this, "Не подключено");
        
            _connectBtn.onClick.AddListener(OnConnectBtn);
        }

        public void Show()
        {
            Client.Client.OnDebugMessage += OnLoginTextChanged;
            _connectBtn.gameObject.SetActive(true);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            Client.Client.OnDebugMessage -= OnLoginTextChanged;
            gameObject.SetActive(false);
        }

        void OnConnectBtn()
        {
            _connectBtn.gameObject.SetActive(false);
            Client.Client.Connect();
        }

        void OnLoginTextChanged(object sender, string text)
        {
            _loginLbl.text = text;
        }
    }
}
