using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class Leaderboard : MonoBehaviour
{
    [System.Serializable]
    public struct UserScore
    {
        public string username;
        public int score;
    }

    [System.Serializable]
    public struct UserScores
    {
        public UserScore[] array;
    }

    private const string USER_AGENT = "Gravity Box Client";
    private const string API_END_POINT = "https://ossified-organized-thorn.glitch.me";
    private const string API_KEY = "glitch-leaderboard-VUTLNNfRxcq9fo8x";
    private const int NUM_ENTRIES = 10;

    public static string username = "Guest";

    [SerializeField] private LeaderboardEntry entryPrefab;

    private LeaderboardEntry[] entries;

    void Awake()
    {
        entries = new LeaderboardEntry[NUM_ENTRIES];
        for (int i = 0; i < NUM_ENTRIES; i++)
        {
            entries[i] = Instantiate(entryPrefab, transform);
            entries[i].Initialize(i);
        }
    }

    void OnEnable()
    {
        StartCoroutine(GetLeaderboard());
    }

    private IEnumerator GetLeaderboard()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(API_END_POINT))
        {
            request.SetRequestHeader("User-Agent", USER_AGENT);

            yield return request.SendWebRequest();

            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string json = "{ \"array\": " + request.downloadHandler.text + "}";
                UserScores scores = JsonUtility.FromJson<UserScores>(json);
                DisplayScores(scores.array);
            }
        }
    }

    private void DisplayScores(UserScore[] scores)
    {
        for (int i = 0; i < scores.Length; i++)
        {
            entries[i].DisplayScore(scores[i].username, scores[i].score);
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

        using (UnityWebRequest request = new UnityWebRequest())
        {
            request.url = $"{API_END_POINT}/{username}";
            request.uploadHandler = new UploadHandlerRaw(Encoding.ASCII.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.method = UnityWebRequest.kHttpVerbPOST;

            request.SetRequestHeader("User-Agent", USER_AGENT);

            yield return request.SendWebRequest();

            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
}
