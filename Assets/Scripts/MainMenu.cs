using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame() {
        SceneManager.LoadScene("Game");
    }

    public void ShowControls() {
        SceneManager.LoadScene("Controls");
    }

    public void AboutGame() {
        SceneManager.LoadScene("About");
    }

    public void TitleScreen() {
        SceneManager.LoadScene("Title Screen");
    }
}
