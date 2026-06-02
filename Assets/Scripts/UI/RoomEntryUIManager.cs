using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fusion;
using System.Collections.Generic;

public class RoomEntryUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI serverName, playerCount;
    [SerializeField] Image passwordImage;
    private SessionInfo SessionInfo;

    public void OnSessionListUpdated(NetworkRunner runner,List<SessionInfo> sessionList)
    {
        Debug.Log("Salas encontradas: " + sessionList.Count);
    }


    public void Setup(SessionInfo session)
    {
        SessionInfo = session;

        serverName.text = session.Name;
        playerCount.text = $"{session.PlayerCount}/{session.MaxPlayers}";

        bool hasPassword = false;

        if (session.Properties.TryGetValue("hasPassword", out var value))
            hasPassword = value > 0;

        passwordImage.color = hasPassword
            ? Color.red
            : Color.green;
    
    }


}
