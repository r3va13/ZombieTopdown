using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        
        ChangeLoginText("Не подключено");
        
        _connectBtn.onClick.AddListener(OnConnectBtn);

        EventManager.LoginEvent += Hide;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void OnConnectBtn()
    {
        ClientServerController.Instance.Connect();
    }

    public void ChangeLoginText(string text)
    {
        _loginLbl.text = text;
    }
}
