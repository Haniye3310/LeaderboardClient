using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SimpleHttpClient : MonoBehaviour
{
    private const string BaseURL = "http://localhost:8080";
    private const string GetUrl = BaseURL+"/get";
    private const string UpdateUrl = BaseURL + "/update";

    public void OnGetClicked()
    {
        StartCoroutine(SendRequest(LeaderBoardRequest.GetLeaderBoard));
    }
    public void OnUpdateOrSetClicked() 
    {
        StartCoroutine(SendRequest(LeaderBoardRequest.UpdateLeaderBoard));
    }
    private IEnumerator SendRequest(LeaderBoardRequest leaderBoardRequest)
    {
        switch (leaderBoardRequest)
        {
            case LeaderBoardRequest.GetLeaderBoard:
                {
                    using (UnityWebRequest request = UnityWebRequest.Get(GetUrl))
                    {
                        // Send the request and wait for a response
                        yield return request.SendWebRequest();

                        if (request.result == UnityWebRequest.Result.Success)
                        {
                            Debug.Log("Response from server: " + request.downloadHandler.text);
                        }
                        else
                        {
                            Debug.LogError("Request failed: " + request.error);
                        }
                    }
                    yield break;

                }
            case LeaderBoardRequest.UpdateLeaderBoard:
                {
                    using (UnityWebRequest request = new UnityWebRequest(UpdateUrl, "POST"))
                    {
                        LeaderboardData user1Data = new LeaderboardData()
                        {
                            ID = 1,
                            Name = "Haniye",
                            Score = 100
                        };
                        string jsonData = JsonUtility.ToJson(user1Data);
                        byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
                        request.uploadHandler = new UploadHandlerRaw(dataBytes);
                        request.downloadHandler = new DownloadHandlerBuffer();
                        request.SetRequestHeader("Content-Type", "application/json");

                        // Send the request and wait for a response
                        yield return request.SendWebRequest();

                        if (request.result == UnityWebRequest.Result.Success)
                        {
                            Debug.Log("Field was updated successfully");
                        }
                        else
                        {
                            Debug.LogError("Request failed: " + request.error);
                        }
                    }
                    yield break;

                }
        }


        
    }
}
[Serializable]
class LeaderboardData
{
    public int ID;
    public string Name;
    public int Score;
}
enum LeaderBoardRequest
{
    GetLeaderBoard,
    UpdateLeaderBoard
}