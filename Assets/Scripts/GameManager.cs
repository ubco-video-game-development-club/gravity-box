using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScoreSystem))]
public class GameManager : MonoBehaviour
{
    public static ScoreSystem ScoreSystem { get { return scoreSystem; } }
    private static ScoreSystem scoreSystem;

    private static GameManager singleton;

    void Awake() 
    {
        //Enfore singleton
        if(singleton != null) 
        {
            Destroy(gameObject);
            return;
        }

        //Initialize singleton
        singleton = this;
        DontDestroyOnLoad(gameObject);

        //Initialize systems
        scoreSystem = GetComponent<ScoreSystem>();
    }
}
