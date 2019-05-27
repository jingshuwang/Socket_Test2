using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.Video;
using DG.Tweening;

//using LitJson;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SocketManagerVideo : MonoBehaviour {

//	SocketIOComponent m_socket;

	public static GameObject playButton;

	public static int videoNum = 0;
	public static bool isPlay = true;

	public static Text videoTitle0;
	public static Text videoTitle1;
	public static Text videoTitle2;
	public static Text videoTitle3;

	public static VideoPlayer video0;
	public static VideoPlayer video1;
	public static VideoPlayer video2;
	public static VideoPlayer video3;

	public static GameObject picture0;
	public static GameObject picture1;
	public static GameObject picture2;
	public static GameObject picture3;

	public static GameObject text0;
	public static GameObject text1;
	public static GameObject text2;
	public static GameObject text3;


	// Use this for initialization
	void Start () {

		playButton = GameObject.Find ("PlayButton");

		picture0 = GameObject.Find ("Picture0");
		picture1 = GameObject.Find ("Picture1");
		picture2 = GameObject.Find ("Picture2");
		picture3 = GameObject.Find ("Picture3");

		text0 = GameObject.Find ("Text0");
		text1 = GameObject.Find ("Text1");
		text2 = GameObject.Find ("Text2");
		text3 = GameObject.Find ("Text3");

		text1.SetActive (false);
		text2.SetActive (false);
		text3.SetActive (false);

		video0 = GameObject.Find ("RawImage0").GetComponent<VideoPlayer>();
		video1 = GameObject.Find ("RawImage1").GetComponent<VideoPlayer>();
		video2 = GameObject.Find ("RawImage2").GetComponent<VideoPlayer>();
		video3 = GameObject.Find ("RawImage3").GetComponent<VideoPlayer>();

		videoTitle0 = GameObject.Find ("Text0").GetComponent<Text>();
		videoTitle1 = GameObject.Find ("Canvas/Video1/Text1/Text_title1").GetComponent<Text>();
		videoTitle2 = GameObject.Find ("Canvas/Video2/Text2/Text_title2").GetComponent<Text>();
		videoTitle3 = GameObject.Find ("Canvas/Video3/Text3/Text_title3").GetComponent<Text>();
		
	}
	
	// Update is called once per frame
	void Update () {

			
	}

	public static void OnPlayVideo(int num){

		playButton.transform.DOScaleX (0, 0);
		picture0.transform.DOScaleX (0, 0);

		if(num == 1){
			video1.transform.DOScaleX (3.2f, 0);
			text1.SetActive (true);
			video1.Play ();
			videoTitle0.text = "John, 53                                                                                  ";
			videoTitle1.DOColor (new Color (255f / 255f, 170f / 255f, 155f / 255f), 0);
		}else if(num == 2){
			video2.transform.DOScaleX (3.2f, 0);
			text2.SetActive (true);
			video2.Play ();
			videoTitle0.text = "Agerian, 49                                                                         ";
			videoTitle2.DOColor (new Color (255f / 255f, 170f / 255f, 155f / 255f), 0);
		}else if(num == 3){
			video3.transform.DOScaleX (3.2f, 0);
			text3.SetActive (true);
			video3.Play ();
			videoTitle0.text = "Stuart, 29                                                                              ";
			videoTitle3.DOColor (new Color (255f / 255f, 170f / 255f, 155f / 255f), 0);
		}else if(num == 0){
			video0.transform.DOScaleX (3.2f, 0);
			text0.SetActive (true);
			video0.Play ();
		}

	}

	public static void OnPauseVideo(){

		playButton.transform.DOScaleX (2, 0);

		if (videoNum == 1) {
			video1.Pause ();
		} else if (videoNum == 2) {
			video2.Pause ();
		} else if (videoNum == 3) {
			video3.Pause ();
		} else if (videoNum == 0) {
			video0.Pause ();
		}

	}

	public static void OnExitVideo(){

		if (videoNum == 1) {
			video1.Stop ();
			video1.transform.DOScaleX (0, 0f);
			text1.SetActive (false);
			videoTitle1.DOColor (new Color (255f / 255f, 255f / 255f, 255f / 255f), 0);
		} else if (videoNum == 2) {
			video2.Stop ();
			video2.transform.DOScaleX (0, 0f);
			text2.SetActive (false);
			videoTitle2.DOColor (new Color (255f / 255f, 255f / 255f, 255f / 255f), 0);
		} else if (videoNum == 3) {
			video3.Stop ();
			video3.transform.DOScaleX (0, 0f);
			text3.SetActive (false);
			videoTitle3.DOColor (new Color (255f / 255f, 255f / 255f, 255f / 255f), 0);
		} else if (videoNum == 0) {
			video0.Stop ();
			video0.transform.DOScaleX (0, 0f);
		}

		videoTitle0.text = "Health Partner for Weight Loss Surgery";
		picture0.transform.DOScaleX (3.98f, 0);
		playButton.transform.DOScaleX (2, 0);


	}
}
