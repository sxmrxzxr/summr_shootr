using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public MatchSettings matchSettings;

    private int RedTeamScore = 0;
    private int BlueTeamScore = 0; 

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one game manager in scene!");
        }
        else
        {
            instance = this;
        }
    }

    #region Player tracking

    private const string PLAYER_ID_PREFIX = "Player";
    private const string RED_TEAM_TAG = "RedTeam";
    private const string BLUE_TEAM_TAG = "BlueTeam";

    private static Dictionary<string, Player> Players = new Dictionary<string, Player>();
    private static List<Player> RedTeam = new List<Player>();
    private static List<Player> BlueTeam = new List<Player>();

    public static void RegisterPlayer(string netID, Player player)
    {
        string playerID = PLAYER_ID_PREFIX + " " + netID;
        Players.Add(playerID, player);
        player.transform.name = playerID;
        
        if (RedTeam.Count == 0 && BlueTeam.Count == 0)
        {
            player.tag = RED_TEAM_TAG;
            RedTeam.Add(player);
        }
        else if (RedTeam.Count > BlueTeam.Count)
        {
            player.tag = BLUE_TEAM_TAG;
            BlueTeam.Add(player);
        }
        else
        {
            player.tag = RED_TEAM_TAG;
        }
    }

    public static void UnRegisterPlayer(string playerId)
    {
        Players.Remove(playerId);
    }

    public static Player GetPlayer(string playerId)
    {
        return Players[playerId];
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();

        foreach (string playerId in Players.Keys)
        {
            GUILayout.Label(playerId + " - " + Players[playerId].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
    #endregion
    
}
