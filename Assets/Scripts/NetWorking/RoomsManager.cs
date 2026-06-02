using UnityEngine;
using Fusion;
using TMPro;
using System.Collections.Generic;

public class RoomsManager:MonoBehaviour
{
    [SerializeField] TMP_InputField serverName;
    [SerializeField] TMP_InputField serverPassword;
    [SerializeField] GameObject createLobbyHolder;
    [SerializeField] GameObject lobbiesHolder;
    [SerializeField] Transform servers;
    public GameObject ServerPrefab;
    public void OnCreateRoom()
    {
        string roomname = serverName.text;
        string password = serverPassword.text;

        FusionManager.Instance.CreateRoom(roomname, password);

    }
    public async void SearchForLobby()
    {
        createLobbyHolder.SetActive(false);
        lobbiesHolder.SetActive(true);
        FusionManager.Instance.CreateRunnerIfNeeded();
        await FusionManager.Instance.runner.JoinSessionLobby(
            SessionLobby.Shared);
    }

    public void UpdateLobbyList(List<SessionInfo> sessionList)
    {
        foreach (Transform child in servers)
            Destroy(child.gameObject);

        foreach (SessionInfo session in sessionList)
        {
            RoomEntryUIManager room =
                Instantiate(serverPassword, servers)
                .GetComponent<RoomEntryUIManager>();

            room.Setup(session);
        }
    }
}
