using Elindery.Internals;
using UnityEngine;
using UnityEngine.UI;

namespace Elindery.WindowControllers
{
    public class RoomController : MonoBehaviour
    {
#region Singleton
        static RoomController _instance;
        public static RoomController Instance
        {
            get
            {
                if (_instance) return _instance;
            
                _instance = GameObject.Find("Root").transform.Find("RoomController").GetComponent<RoomController>();
                _instance.GetWhatYouNeed();
            
                return _instance;
            }
        }
#endregion

        Button _survivorBtn;
        Button _zombieBtn;

        void GetWhatYouNeed()
        {
            _survivorBtn = transform.Find("ButtonSurvivor").GetComponent<Button>();
            _zombieBtn = transform.Find("ButtonZombie").GetComponent<Button>();
            _survivorBtn.onClick.AddListener(FindRoomSurvivor);
            _zombieBtn.onClick.AddListener(FindRoomZombie);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    
        void FindRoomSurvivor()
        {
            TimeoutButtons();
            Client.Client.Send("find_room|survivor|" + Storage.ClientID);
        }
    
        void FindRoomZombie()
        {
            TimeoutButtons();
            Client.Client.Send("find_room|zombie|" + Storage.ClientID);
        }

        void TimeoutButtons()
        {
            Utils.ButtonClickTimeout(_survivorBtn, 3f);
            Utils.ButtonClickTimeout(_zombieBtn, 3f);
        }
    }
}
