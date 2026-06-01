using UnityEngine;
using Fusion;
using System.Collections.Generic;

public class FusionManager : MonoBehaviour
{
    public static FusionManager Instance;
    private NetworkRunner runner;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }
    public async void CreateRoom(string roomName,string password)
    {
        runner = gameObject.AddComponent<NetworkRunner>();
        Dictionary<string, SessionProperty> properties = new()
        {
            { "HasPassword", !string.IsNullOrEmpty(password) }
        };
       var result= await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName = roomName,
            SessionProperties=properties
            
        }); ;


        Debug.Log($"Success: {result.Ok}");
        Debug.Log($"Reason: {result.ShutdownReason}");
        RoomRegister.RegisterRoom(new RoomData
        {
           roomName=roomName,
           password=password,
           maxPlayer=10,
           currentPlayers=1,
        });
        Debug.Log($"Room Created: {roomName}");

    }
    // Update is called once per frame
    void Update()
    {
        
    }
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