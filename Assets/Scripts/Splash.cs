using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {

	public float fadeSpeed = 3.0f;
	private Material material;
	private static int sceneNum;
	private static bool isSceneStaring = false;
	private static bool isScenenEnding = false;
	private float ColorR;
	private float ColorG;
	private float ColorB;
	private Color colorTemp;
	private Color colorWhite;
	private Color colorBlack;

	// Use this for initialization
	void Start () {

		material = transform.GetComponent<MeshRenderer>().material;
		
		colorTemp = material.GetColor("_TintColor");
		ColorR = colorTemp.r;
		ColorG = colorTemp.g;
		ColorB = colorTemp.b;

		colorWhite = new Color (ColorR,ColorG,ColorB,0);
		colorBlack = new Color (ColorR,ColorG,ColorB,1);

		DontDestroyOnLoad (this.gameObject);
		
	}
	
	// Update is called once per frame
	void Update () {

		if (isSceneStaring) {
			StartScene();
		}

		if (isScenenEnding) {
			EndScene();
		}

		material.SetColor("_TintColor",colorTemp);
		
	}

	void FadeToWhite(){

		colorTemp = Color.Lerp (colorTemp, colorWhite, fadeSpeed * Time.deltaTime);

	}

	void StartScene(){

		FadeToWhite ();

		if (colorTemp.a <= 0.05f) {

			colorTemp = colorWhite;
			isSceneStaring = false;

//			Destroy (this.gameObject);

		}

	}

	void FadeToBlack(){

		colorTemp = Color.Lerp (colorTemp, colorBlack, fadeSpeed * Time.deltaTime);
	}

	void EndScene(){

		FadeToBlack ();

		if (colorTemp.a >= 0.95f) {

			colorTemp = colorBlack;
			isSceneStaring = true;
			isScenenEnding = false;

			SceneManager.LoadScene (sceneNum);

		}
	}

	public static void SplashGoTo(int level)
	{
		isSceneStaring = false;
		isScenenEnding = true;
		sceneNum = level;  
	}
}
