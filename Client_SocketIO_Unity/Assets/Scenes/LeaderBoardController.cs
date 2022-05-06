using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardController : MonoBehaviour
{
    private string leadEndpoint = "https://rubamazzopiu.spaghetti-interactive.it/leaders";
    public ClientApi clientApi;
    
    public void GetLeaderBoardData(LeaderboardRequestPayload payload,Action<LeaderBoardResponse> callback)
    {
        clientApi.PostLeaderboardRequest(leadEndpoint, payload, responseStr => {
            LeaderBoardResponse result = OnLeaderboardDataReceived(responseStr);
            callback(result);
        });
    }
    public LeaderBoardResponse OnLeaderboardDataReceived(string json)
    {
        LeaderBoardResponse result = JsonUtility.FromJson<LeaderBoardResponse>(json);
        return result;
    }
}


[System.Serializable]
public class LeaderBoardResponse
{
    public bool success;
    public List<LeaderBoardPlayer> ALL = new List<LeaderBoardPlayer>();
    public string year;
    public string month;
}
[System.Serializable]
public class LeaderBoardPlayer
{   
    public int rank;
    public string guid;
    public int score;
    public string nick;
    public int levelNum;
    public string picture;
}


[System.Serializable]
public class LeaderboardRequestPayload
{
    public string period;
    public string level;
    public string type;
    public int lstart;
    public int lstop;

    [SerializeField]
    public LeaderboardRequestPayload(string period, string level, string type, int lstart, int lstop)
    {
        this.period = period;
        this.level = level;
        this.type = type;
        this.lstart = lstart;
        this.lstop = lstop;
    }
}
