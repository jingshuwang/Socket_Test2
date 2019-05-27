using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GoToNext : MonoBehaviour {

//	public int sceneNum;
float m_timer = 0;
	public GameObject JNJ;

	// Use this for initialization
	void Start () {

		ShowJNJ();

	}

	// Update is called once per frame
	void Update () {

		m_timer += Time.time;
		if (m_timer >= 350)
		{
			ShowB(1);
			m_timer = 0;
		}
	}

	private void ShowB(int sceneNum)
	{

//		SceneManager.LoadScene(sceneNum);
		Splash.SplashGoTo (sceneNum);
	}
		
	void ShowJNJ(){
		JNJ.GetComponent<SpriteRenderer> ().DOFade (1, 3);
	}
}
