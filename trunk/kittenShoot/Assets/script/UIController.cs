using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

// Use this for initialization
    public Text timer_text;
    public Text kills_text;
    public Text SpawnRate_text;
    private float killCount = 0;
    private float Timer = 0f;
	

	// Update is called once per frame
	void Update () {
        Timer += Time.deltaTime;
        timer_text.text = "Time passed: " + (int)Timer;
	}

    void CountKills()
    {
        killCount++;
        kills_text.text = "Killed human: " + killCount;
    }

    void SpawnRateCount(float SpawnRate){
        SpawnRate_text.text = "Spawn Rate Now: " + (int)SpawnRate;
    }
}
