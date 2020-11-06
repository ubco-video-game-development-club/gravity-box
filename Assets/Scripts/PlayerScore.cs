using UnityEngine;
using UnityEngine.Events;

public class PlayerScore : MonoBehaviour
{
    [System.Serializable] public class OnScoreChangedEvent : UnityEvent<int> { }

    public int Score { get { return score; } }
    private int score;

    [SerializeField] private OnScoreChangedEvent onScoreChanged = new OnScoreChangedEvent();

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
