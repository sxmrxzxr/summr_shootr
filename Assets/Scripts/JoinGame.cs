using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

    [SerializeField]
    private Text status;

    [SerializeField]
    private GameObject roomListItemPrefab;

    [SerializeField]
    private Transform roomListParent;

    List<GameObject> roomList = new List<GameObject>();
    private NetworkManager networkManager;

	// Use this for initialization
	void Start () {
        networkManager = NetworkManager.singleton;

        if (networkManager.matchMaker == null)
            networkManager.StartMatchMaker();

        RefreshRoomList();
	}

    public void RefreshRoomList()
    {
        ClearRoomList();
        networkManager.matchMaker.ListMatches(0, 20, "", false,0,0, OnMatchList);
        status.text = "Loading...";
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList) // 16:09
    {
        status.text = "";

        if (matchList == null)
        {
            status.text = "Couldn't get room list :(";
            return;
        }

        ClearRoomList();

        foreach (MatchInfoSnapshot m in matchList)
        {
            GameObject roomListItem = Instantiate(roomListItemPrefab);
            roomListItem.transform.SetParent(roomListParent);

            RoomListItem rli = roomListItem.GetComponent<RoomListItem>();
            if (rli != null)
            {
                rli.Setup(m, JoinRoom);
            }

            // have component sit on game object that will set up name, amount, etc.
            // as well as setup callback fucntion that will join a game
            roomList.Add(roomListItem);
        }

        if (roomList.Count == 0)
        {
            status.text = "There are currently no rooms";
        }
    }

    void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }

    public void JoinRoom(MatchInfoSnapshot m)
    {
        networkManager.matchMaker.JoinMatch(m.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        ClearRoomList();
        status.text = "Joining " + m.name;
    }
}
