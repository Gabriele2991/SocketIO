using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;


public class LeaderBoard : MonoBehaviour
{
    public static LeaderBoard current;
   
    [SerializeField] LeaderBoardController leaderBoardController;
 
    private int lstart = 1;
    private int lstop = 5;
    private string guid;
    private LeaderboardRequestPayload requestPayload;
    private string jsonPlayer = "";
   
   

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
    }
    public void OnLeaderboardShown(Action<string>callback)
    {

        jsonPlayer = "";
        requestPayload = new LeaderboardRequestPayload( "monthly", "multiplayer", "leaderboard", lstart, lstop);

        leaderBoardController.GetLeaderBoardData(requestPayload, results =>
        {
            if (results.success == true)
            {
                Debug.Log(results.ALL[0].guid);
                guid = results.ALL[0].guid;
                PlayerController.current.GetUserdataByGuid(guid, results =>
                {
                    Debug.Log(results.user.nick);
                    PlayerController.Player player = results.user;
                    jsonPlayer = JsonUtility.ToJson(player);
                    callback(jsonPlayer);

                });
            }
            else
                callback(null);
        });
    }
}

