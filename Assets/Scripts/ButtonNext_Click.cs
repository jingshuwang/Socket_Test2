using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MIVR;
using UnityEngine.UI;

public class ButtonNext_Click : MonoBehaviour{

	public int sceneNum;
	Button btn;

	// Use this for initialization
	void Start () {
		
		btn = this.GetComponent<Button> ();
	}

	// Update is called once per frame
	void Update () {
		
		if (InputManager.ControllerState.AppButtonUp) {
			btn.onClick.AddListener (OnPointerClick);
		}
	}

	public void OnPointerClick(){

		Debug.Log ("OnPointerClick called.");
		// SceneManager.LoadScene (sceneNum);
		Splash.SplashGoTo (sceneNum);
	}


}
