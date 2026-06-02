using UnityEngine;
using Fusion;
using System.Collections.Generic;
using Fusion.Sockets;
using System;

public class FusionManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public static FusionManager Instance;
    public NetworkRunner runner;
    public RoomsManager roomsManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreateRunnerIfNeeded()
    {
        if (runner == null)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
            runner.AddCallbacks(this);
        }
    }
    public void OnSessionListUpdated(
    NetworkRunner runner,
    List<SessionInfo> sessionList)
    {
        roomsManager.UpdateLobbyList(sessionList);
    }

    public async void CreateRoom(string roomName, string password)
    {
        runner = gameObject.AddComponent<NetworkRunner>();
        Dictionary<string, SessionProperty> properties = new()
        {
            { "HasPassword", !string.IsNullOrEmpty(password) }
        };
        var result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName = roomName,
            SessionProperties = properties

        }); ;


        Debug.Log($"Success: {result.Ok}");
        Debug.Log($"Reason: {result.ShutdownReason}");
        RoomRegister.RegisterRoom(new RoomData
        {
            roomName = roomName,
            password = password,
            maxPlayer = 10,
            currentPlayers = 1,
        });
        Debug.Log($"Room Created: {roomName}");

    }
    public async void SearchForLobbies()
    {
        CreateRunnerIfNeeded();

        var result = await runner.JoinSessionLobby(SessionLobby.Shared);

        Debug.Log($"Lobby Join Result: {result.Ok}");
    }


    #region Fusion Region
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new NotImplementedException();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("SERVER CONNECTED");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }
    #endregion
}

[System.Serializable]
public class RoomData
{
    public string roomName;
    public string password;
    public int maxPlayer;
    public int currentPlayers;
    public bool hasPassword => !string.IsNullOrEmpty(password);

}