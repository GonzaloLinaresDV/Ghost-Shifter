using UnityEngine;
using TMPro;
public class RoomsManager:MonoBehaviour
{
    [SerializeField] TMP_InputField serverName;
    [SerializeField] TMP_InputField serverPassword;

    public void OnCreateRoom()
    {
        string roomname = serverName.text;
        string password = serverPassword.text;

        FusionManager.Instance.CreateRoom(roomname, password);

    }
}
