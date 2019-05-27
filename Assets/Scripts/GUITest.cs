using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITest : MonoBehaviour {

	public Texture fadeImage;

	// Use this for initialization
	void Start () {

		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI() {
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeImage);

		GUI.Label(new Rect(0, 0, 100, 200), "11111");
        GUI.Label(new Rect(30, Screen.height - 50, 100, 200), "22222");
   	    GUI.Label(new Rect(Screen.width - 50, Screen.height - 50, 100, 200), "33333");
        GUI.Label(new Rect(Screen.width - 100, 30, 100, 200), "444444");
	}
}
