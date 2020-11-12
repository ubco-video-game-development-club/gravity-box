using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveTimerDisplay : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;

    void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        GameManager.WaveSystem.AddTimerChangedListener(UpdateWaveTimer);
    }

    public void UpdateWaveTimer(int time)
    {
        textMeshProUGUI.text = time.ToString();
    }
}
