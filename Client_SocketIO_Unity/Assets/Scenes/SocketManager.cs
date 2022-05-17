using System;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;




public class SocketManager : MonoBehaviour
{
    public SocketIOUnity socket;
    public SocketIOResponse response;
    public InputField EventNameTxt;
    public InputField DataTxt;
    public Text ReceivedText;  

    public GameObject objectToSpin;
    
    // Start is called before the first frame update
    private void Start()
    {
        //TODO: check the Uri if Valid.
        var uri = new Uri("http://localhost:8000");
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
                {
                    {"token", "UNITY" }
                }
            ,
            EIO = 4
            ,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        socket.JsonSerializer = new NewtonsoftJsonSerializer();

        ///// reserved socketio events
        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("socket.OnConnected");
        };
        socket.OnPing += (sender, e) =>
        {
            Debug.Log("Ping");
        };
        socket.OnPong += (sender, e) =>
        {
            Debug.Log("Pong: ")/* + e.TotalMilliseconds)*/;
        };
        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Log("disconnect: " + e);
        };
        socket.OnReconnectAttempt += (sender, e) =>
        {
            if (e >= 5){
                Debug.Log("Socket disconnected");
                socket.Disconnect();
                socket.Dispose();
                return;
            }
            Debug.Log($"{DateTime.Now} Reconnecting: attempt = {e}");
            
        };
        ////

        Debug.Log("Connecting...");
        socket.Connect();

        socket.OnUnityThread("spin", (data) =>
        {
            rotateAngle = 0;
        });

        ReceivedText.text = "";
        socket.OnAnyInUnityThread((name, response) =>
        {
            ReceivedText.text += "Received On " + name + " : " + response.GetValue().GetRawText() + "\n";
        });
    }
    public void EmitTest()
    {
        string eventName = EventNameTxt.text.Trim().Length < 1 ? "hello" : EventNameTxt.text;
        string txt = DataTxt.text;
        List<string> achi = new List<string>();
        achi.Add("benvenuto");

        //PLAYER TRYOUT TO SEND ON BACKEND RUBAMAZZO
        Player player = new Player("32341", "ascnasc", "everyone", true, null, "it", "it", "caccolajoe",achi,"7.9.8","vaffanculo");
        
        socket.EmitAsync("identify me", resFromServer =>
        {
            Debug.Log("RESPONSE FROM SERVER: "+resFromServer);
        }, player);

        
        //LeaderBoard.current.OnLeaderboardShown(result =>
        //{
        //    socket.EmitAsync(eventName,response=> {
        //        Debug.Log(response);//NEED TO IMPLEMENT IF THE ASK IS NOT TRUE
        //    },result);
        //    Debug.Log("Result in Emittest: " + result);
        //});
        //if (!IsJSON(txt))
        //{
        //    socket.Emit(eventName, txt);
        //}
        //else
        //{
        //    socket.EmitStringAsJSON(eventName, txt);
        //}
    }

    public static bool IsJSON(string str)
    {
        if (string.IsNullOrWhiteSpace(str)) { return false; }
        str = str.Trim();
        if ((str.StartsWith("{") && str.EndsWith("}")) || //For object
            (str.StartsWith("[") && str.EndsWith("]"))) //For array
        {
            try
            {
                var obj = JToken.Parse(str);
                return true;
            }catch (Exception ex) //some other exception
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void EmitSpin()
    {
        socket.Emit("spin");
    }

    public void EmitClass()
    {
        //TestClass testClass = new TestClass(new string[] { "foo", "bar", "baz", "qux" });
        TestClass2 testClass2 = new TestClass2("lorem ipsum");
        socket.Emit("class", testClass2);
    }

    // our test class
    [System.Serializable]
    class TestClass
    {
        public string[] arr;

        public TestClass(string[] arr)
        {
            this.arr = arr;
        }
    }

    [System.Serializable]
    class TestClass2
    {
        public string text;

        public TestClass2(string text)
        {
            this.text = text;
        }
    }
    //

    [Serializable]
    public class Player
    {
        public string guid;
        public string shortId;
        public string visibleOnline;
        public bool isprem;
        public string noAds;
        public string language;
        public string clientLang;
        public string nick;
        public List<string> achievements;
        public string appversion;
        public string deviceid;

        public Player(string guid, string shortId, string visibleOnline, bool isprem, string noAds, string language, string clientLang, string nick, List<string> achievements, string appversion, string deviceid)
        {
            this.guid = guid;
            this.shortId = shortId;
            this.visibleOnline = visibleOnline;
            this.isprem = isprem;
            this.noAds = noAds;
            this.language = language;
            this.clientLang = clientLang;
            this.nick = nick;
            this.achievements = achievements;
            this.appversion = appversion;
            this.deviceid = deviceid;
        }
    }

    float rotateAngle = 45;
    readonly float MaxRotateAngle = 45;
    void Update()
    {
        if(rotateAngle < MaxRotateAngle)
        {
            rotateAngle++;
            objectToSpin.transform.Rotate(0, 1, 0);
        }
    }
}