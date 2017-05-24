using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint RoomSize = 8;

    private string RoomName;
    private NetworkManager networkManager;

    void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }

    public void SetRoomName(string name)
    {
        RoomName = name;
    }

    public void CreateRoom()
    {
        if (RoomName != " " && RoomName != null)
        {
            Debug.Log("Creating Room " + RoomName + " of size " + RoomSize);
            networkManager.matchMaker.CreateMatch(RoomName, RoomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        }
    }
}
