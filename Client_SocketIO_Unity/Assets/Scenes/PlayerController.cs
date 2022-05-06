using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController current;
    [SerializeField] ClientApi clientApi;

    private string userEndpoint = "https://rubamazzopiu.spaghetti-interactive.it/userdata/";

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
    }

    public void GetUserdataByGuid(string guid, System.Action<PlayerResponse> callback)
    {
        clientApi.GetRequest(userEndpoint + guid, responseStr =>
        {
            //Debug.Log("GETUSERDATABYGUID RESPONSE: "+responseStr);
            PlayerResponse result = OnUserDataReceived(responseStr);
            callback(result);
        });
    }
    public PlayerResponse OnUserDataReceived(string json)
    {
        //Debug.Log("OnUSERDATARECEIVED RESPONSE: "+ json);
        PlayerResponse result = JsonUtility.FromJson<PlayerResponse>(json);
        //Debug.Log("OnUSerDATARECEIVED RESULT: " + result);
        return result;
    }

    [System.Serializable]
    public class PlayerResponse
    {
        public bool success;
        public Player user;
    }

    [System.Serializable]
    public class Player
    {
        [System.Serializable]
        public struct Achievements
        {
            public string achievement;
        }

        [System.Serializable]
        public struct Preferences
        {
            public string pm;
            public string cr;
            public string fr;
            public string vo;
            public string @as;
        }

        [System.Serializable]
        public struct Elo
        {
            public int score;
            public int perc;
        }

        [System.Serializable]
        public struct Level
        {
            public int level;
            public int levelScore;
            public int nextLevelScore;
            public int score;
        }

        [System.Serializable]
        public struct Trophies
        {
            public string id;
            public string level;
            public int rank;
            public string assigendAt;
        }

        public string language;
        public string avatar;
        public string shortId;
        public string guid;
        public string fbid;
        public string nick;
        public string created;
        public string city;
        public string province;
        public List<Achievements> achievements;
        public Preferences preferences;
        public string about;
        public Elo elo;
        public Level level;
        public Dictionary<string, Trophies> trophies; 
        public string picture;
        public bool picdis;
    }
}
