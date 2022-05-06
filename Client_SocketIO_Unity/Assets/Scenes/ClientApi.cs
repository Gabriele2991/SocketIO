using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientApi : MonoBehaviour
{
    public void GetRequest(string url, System.Action<string> callback)
    {
        StartCoroutine(Get(url, callback));
    }

    public IEnumerator Get(string url, System.Action<string> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();


        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            //InitAlert(request.result.ToString(), content);
            Debug.Log("ERROR: " + request.error);
        }
        else
        {
            if (request.isDone)
            {
                var result = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                callback(result);
            }
            else
            {
                //InitAlert("Cannot get the Data", content);
                Debug.Log("Cannot get the Data");
            }
        }
    }

    public void PostFormRequest(string url, WWWForm data,System.Action<string> callback)
    {
        StartCoroutine(FormPost(url, data, callback));
    }

    public IEnumerator FormPost(string url,WWWForm data, System.Action<string> callback)
    {
        UnityWebRequest request = UnityWebRequest.Post(url, data);
        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.isDone)
            {
                var result = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                Debug.Log("LOGIN POST RESULT: " + result);

                callback(result);
            }
            else
            {
                Debug.Log("Cannot reach the data");
            }
        }
    }
    public void PostLeaderboardRequest(string url, LeaderboardRequestPayload data, System.Action<string> callback)
    {
        StartCoroutine(LeaderboardPost(url, data, callback));
    }

    public IEnumerator LeaderboardPost(string url, LeaderboardRequestPayload data, System.Action<string> callback)
    {

        var jsonData = JsonUtility.ToJson(data);

        UnityWebRequest request = UnityWebRequest.Post(url, jsonData);
        request.SetRequestHeader("content-type", "application/json");//IMPORTANT TO GET REQUEST FROM NET
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
        request.uploadHandler.contentType = "application/json";


        yield return request.SendWebRequest();



        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            //InitAlert(request.result.ToString(), content);
            Debug.Log(request.error);
        }
        else
        {
            if (request.isDone)
            {
                var result = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                callback(result);
            }
            else
            {
                //InitAlert("data cannot be reached", content);
                Debug.Log("data cannot be reached");
            }
        }
    }
}
