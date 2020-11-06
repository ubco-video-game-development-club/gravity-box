using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    [System.Serializable] public class OnScoreChangedEvent : UnityEvent<int> { }

    public static ScoreManager Singleton { get { return singleton; } }
    private static ScoreManager singleton = null;

    public int Score { get { return score; } }
    private int score;

    [SerializeField] private OnScoreChangedEvent onScoreChanged = new OnScoreChangedEvent();

    void Awake() 
    {
        if(singleton != null) 
        {
            //Okay so IIRC this will only destroy this specific component.
            //That's the intended functionality in case there are multiple
            //management scripts on here that might need to stay.
            //I might also be overthinking this, so lemme know.
            Destroy(this);
            return;
        }

        singleton = this;
        DontDestroyOnLoad(this);
    }

    public void AddScore(int amount) 
    {
        //This assumes we don't ever want to take away player score.
        //If we do, just change this accordingly.
        if(amount < 0) {
            Debug.LogError("Cannot add negative score.");
            return;
        }

        score += amount;
        onScoreChanged.Invoke(score);
    }

    public void AddScoreChangedListener(UnityAction<int> call) 
    {
        onScoreChanged.AddListener(call);
    }

    public void RemoveScoreChangedListener(UnityAction<int> call) 
    {
        onScoreChanged.RemoveListener(call);
    }
}
