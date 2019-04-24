using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {
    public float timeLeft = 3;
    public Text _text;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        _text.text = timeLeft.ToString("N0");
        if (timeLeft < 0)
        {
            SceneManager.LoadScene("demo", LoadSceneMode.Single);
        }
    }
}
