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
    public struct Rankings
    {
        public UserScore[] top10;
        public int ranking;
    }

    private const string USER_AGENT = "Gravity Box Client";
    private const string API_END_POINT = "https://ossified-organized-thorn.glitch.me";
    private const string API_KEY = "glitch-leaderboard-VUTLNNfRxcq9fo8x";
    private const int NUM_ENTRIES = 10;

    public static string username = "Guest";

    [SerializeField] private LeaderboardEntry entryPrefab;
    [SerializeField] private LeaderboardEntry personalEntry;

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
        StartCoroutine(GetRankings());
    }

    private IEnumerator GetRankings()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(API_END_POINT + $"/ranks/{username}"))
        {
            request.SetRequestHeader("User-Agent", USER_AGENT);

            yield return request.SendWebRequest();

            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                Rankings rankings = JsonUtility.FromJson<Rankings>(json);
                DisplayScores(rankings);
            }
        }
    }

    private void DisplayScores(Rankings rankings)
    {
        UserScore[] top10 = rankings.top10;
        for (int i = 0; i < top10.Length; i++)
        {
            entries[i].DisplayScore(i + 1, top10[i].username, top10[i].score);
        }

        if (rankings.ranking < 0)
        {
            personalEntry.gameObject.SetActive(false); //This works because the scene will be reloaded if you get a high score
        }
        else
        {
            int highScore = PlayerPrefs.GetInt(ScoreSystem.HIGH_SCORE_PREF);
            personalEntry.DisplayScore(rankings.ranking + 1, username, highScore);
        }
    }

    public static IEnumerator SetUserScore(string username, int score)
    {
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogWarning("Empty username passed to SetUserScore.");
            yield break;
        }

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
