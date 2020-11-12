using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveTimerDisplay : MonoBehaviour
{
    private TextMeshProUGUI textGUI;

    void Awake()
    {
        textGUI = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        GameManager.WaveSystem.AddTimerChangedListener(UpdateWaveTimer);
    }

    public void UpdateWaveTimer(int time)
    {
        textGUI.text = time.ToString();
    }
}
