using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class Leaderboard : MonoBehaviour
{
    private const string USER_AGENT = "Gravity Box Client";
    private const string API_END_POINT = "https://ossified-organized-thorn.glitch.me";
    private const string API_KEY = "glitch-leaderboard-test";

    void OnEnable()
    {
        StartCoroutine(GetLeaderboard());
        StartCoroutine(SetUserScore("Kurtis", 1000));
    }

    private IEnumerator GetLeaderboard()
    {
        using(UnityWebRequest request = UnityWebRequest.Get(API_END_POINT))
        {
            request.SetRequestHeader("User-Agent", USER_AGENT);

            yield return request.SendWebRequest();

            if(request.isHttpError || request.isNetworkError)
            {
                Debug.LogError(request.error);
            } else 
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

    public static IEnumerator SetUserScore(string username, int score)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{ \"key\": ");
        sb.Append("\"");
        sb.Append(API_KEY);
        sb.Append("\", \"score\": ");
        sb.Append(score);
        sb.Append("}");
        string json = sb.ToString();

        using(UnityWebRequest request = new UnityWebRequest())
        {
            request.url = $"{API_END_POINT}/{username}";
            request.uploadHandler = new UploadHandlerRaw(Encoding.ASCII.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.method = UnityWebRequest.kHttpVerbPOST;

            request.SetRequestHeader("User-Agent", USER_AGENT);

            yield return request.SendWebRequest();

            if(request.isHttpError || request.isNetworkError)
            {
                Debug.LogError(request.error);
            } else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
}
