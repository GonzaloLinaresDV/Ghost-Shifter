using System.Collections.Generic;
public static class RoomRegister
{
    public static List<RoomData> Rooms = new();



    public static void RegisterRoom(RoomData room)
    {
        Rooms.Add(room);
    }
    public static RoomData GetRoom(string roomName)
    {
        return Rooms.Find(x => x.roomName == roomName);
    }
}
