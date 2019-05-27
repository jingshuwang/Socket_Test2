using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using DG.Tweening;

//using LitJson;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SocketManagerModels2 : MonoBehaviour {

//	SocketIOComponent m_socket;

	public static int panelNum = 0;
	public static bool isfront = true;
	public static float angle;
	public static float zoomPos;
	public static float axisX;
	public static float axisY;

	public static GameObject camera;
	public static GameObject male;
	public static GameObject male_fat;
	public static GameObject bones;
	public static GameObject organ_video;           //3D视频展示器官内容
	public static VideoPlayer video;                //2D视频
	public static VideoPlayer organVideo;       	//3D视频
	public static GameObject playButton;            //2D视频播放按钮
	public static GameObject panel1_1;
	public static GameObject panel1_2;
	public static GameObject panel1_3;
	public static GameObject panel1_4;
	public static GameObject panel1_5;
	public static GameObject panel2_1;
	public static GameObject panel2_2;
	public static GameObject panel2_3;
	public static GameObject panel2_4;
	public static GameObject panel2_5;
	// public static GameObject text1_1;
	// public static GameObject text1_2;
	// public static GameObject text1_3;
	// public static GameObject text1_4;
	// public static GameObject text1_5;
	// public static GameObject text2_1;
	// public static GameObject text2_2;
	// public static GameObject text2_3;
	// public static GameObject text2_4;
	// public static GameObject text2_5;

	// Use this for initialization
	void Start () {

		SocketManager.isCompare = false;

		camera = GameObject.Find ("MiCamera");
		male = GameObject.Find ("Male_Model");
		male_fat = GameObject.Find ("MaleFat_Model");
		bones = GameObject.Find ("Bones");
		organ_video = GameObject.Find ("Organ_video");           //3D视频展示器官内容
		// video = GameObject.Find ("RawImage").GetComponent<VideoPlayer>();              								  //2D视频
		organVideo = GameObject.Find ("Organ_video").GetComponent<VideoPlayer>();;
		playButton = GameObject.Find ("PlayButton");            //2D视频播放按钮

		panel1_1 = GameObject.Find ("Panel_brain1");
		panel1_2 = GameObject.Find ("Panel_heart1");
		panel1_3 = GameObject.Find ("Panel_liver1");
		panel1_4 = GameObject.Find ("Panel_pancreas1");
		panel1_5 = GameObject.Find ("Panel_joint1");
		panel2_1 = GameObject.Find ("Panel_brain2");
		panel2_2 = GameObject.Find ("Panel_heart2");
		panel2_3 = GameObject.Find ("Panel_liver2");
		panel2_4 = GameObject.Find ("Panel_pancreas2");
		panel2_5 = GameObject.Find ("Panel_joint2");

		// text1_1 = GameObject.Find ("Text1_1");
		// text1_2 = GameObject.Find ("Text1_2");
		// text1_3 = GameObject.Find ("Text1_3");
		// text1_4 = GameObject.Find ("Text1_4");
		// text1_5 = GameObject.Find ("Text1_5");
		// text2_1 = GameObject.Find ("Text2_1");
		// text2_2 = GameObject.Find ("Text2_2");
		// text2_3 = GameObject.Find ("Text2_3");
		// text2_4 = GameObject.Find ("Text2_4");
		// text2_5 = GameObject.Find ("Text2_5");


		if (isfront) {                                                 //识别从comparison返回时要切换的模式
			camera.transform.rotation = Quaternion.Euler (0f, 0f, 0f);
		} 
		else {
			camera.transform.rotation = Quaternion.Euler (0f, 180f, 0f);
		}
						
	}

	// Update is called once per frame
	void Update () {
		

	}

	#region 注册的事件



	#endregion

	public static void ViewDetailBefore(){
		if (isfront) {								 //判断是否从另一个面板跳转过来，如果是，则先消除前一个面板
			if (panelNum == 1) {
				HidePanel (panel1_1);
				// text1_1.transform.DOScaleX(0,0.5f);
			} else if (panelNum == 2) {
				HidePanel (panel1_2);
				// text1_2.transform.DOScaleX(0,0.5f);
			} else if (panelNum == 3) {
				HidePanel (panel1_3);
				// text1_3.transform.DOScaleX(0,0.5f);
			} else if (panelNum == 4) {
				HidePanel (panel1_4);
				// text1_4.transform.DOScaleX(0,0.5f);
			} else if (panelNum == 5) {
				HidePanel (panel1_5);
				// text1_5.transform.DOScaleX(0,0.5f);
			}
		} else {
			if (panelNum == 1) {
				HidePanel (panel2_1);
				// text2_1.transform.DOScaleX(0,0.5f);
			} else if (panelNum == 2) {
				HidePanel (panel2_2);
				// text2_2.transform.DOScaleX(0,0.5f);
			} else if (panelNum == 3) {
				HidePanel (panel2_3);
				// text2_3.transform.DOScaleX(0,0.5f);
			} else if (panelNum == 4) {
				HidePanel (panel2_4);
				// text2_4.transform.DOScaleX(0,0.5f);
			} else if (panelNum == 5) {
				HidePanel (panel2_5);
				// text2_5.transform.DOScaleX(0,0.5f);
			}
		}
	}

	public static void ViewDetailAfter(){	
		if (isfront) {                                         //判断模型
			if (panelNum == 1) {
				ShowPanel (panel1_1);
				// text1_1.transform.DOScaleX(5,0.5f);
				camera.transform.DOMove (new Vector3 (3f, 30f, 25f), 0.5f);
			} else if (panelNum == 2) {
				ShowPanel (panel1_2);
				// text1_2.transform.DOScaleX(5,0.5f);
				camera.transform.DOMove (new Vector3 (6f, 24f, 25f), 0.5f);
			} else if (panelNum == 3) {
				ShowPanel (panel1_3);
				// text1_3.transform.DOScaleX(5,0.5f);
				camera.transform.DOMove (new Vector3 (-4f, 20f, 25f), 0.5f);
			} else if (panelNum == 4) {
				ShowPanel (panel1_4);
				// text1_4.transform.DOScaleX(5,0.5f);
				camera.transform.DOMove (new Vector3 (4f, 17f, 25f), 0.5f);
			} else if (panelNum == 5) {
				ShowPanel (panel1_5);
				// text1_5.transform.DOScaleX(5,0.5f);
				camera.transform.DOMove (new Vector3 (-5f, 5f, 25f), 0.5f);
			}
		} else {
			if (panelNum == 1) {
				ShowPanel (panel2_1);
				// text2_1.transform.DOScaleX(5,0.5f);
				camera.transform.DOMove (new Vector3 (-3f, 30f, -25f), 0.5f);
			} else if (panelNum == 2) {
				ShowPanel (panel2_2);
				// text2_2.transform.DOScaleX(5,0.5f);
				camera.transform.DOMove (new Vector3 (-6f, 24f, -25f), 0.5f);
			} else if (panelNum == 3) {
				ShowPanel (panel2_3);
				// text2_3.transform.DOScaleX(5,0.5f);
				camera.transform.DOMove (new Vector3 (4f, 20f, -25f), 0.5f);
			} else if (panelNum == 4) {
				ShowPanel (panel2_4);
				// text2_4.transform.DOScaleX(5,0.5f);
				camera.transform.DOMove (new Vector3 (-4f, 17f, -25f), 0.5f);
			} else if (panelNum == 5) {
				ShowPanel (panel2_5);
				// text2_5.transform.DOScaleX(5,0.5f);
				camera.transform.DOMove (new Vector3 (-5f, 5f, -25f), 0.5f);
			}
		}

	}

	public static void ResetCamera(){
		if (panelNum != 0) {                //判断是否从面板状态退出，如果是，则先消除面板
			if (isfront) {
				if (panelNum == 1) {
					HidePanel (panel1_1);
					// text1_1.transform.DOScaleX(0,0.5f);
				} else if (panelNum == 2) {
					HidePanel (panel1_2);
					// text1_2.transform.DOScaleX(0,0.5f);
				} else if (panelNum == 3) {
					HidePanel (panel1_3);
					// text1_3.transform.DOScaleX(0,0.5f);
				} else if (panelNum == 4) {
					HidePanel (panel1_4);
					// text1_4.transform.DOScaleX(0,0.5f);
				} else if (panelNum == 5) {
					HidePanel (panel1_5);
					// text1_5.transform.DOScaleX(0,0.5f);
				}
			} else {
				if (panelNum == 1) {
					HidePanel (panel2_1);
					// text2_1.transform.DOScaleX(0,0.5f);
				} else if (panelNum == 2) {
					HidePanel (panel2_2);
					// text2_2.transform.DOScaleX(0,0.5f);
				} else if (panelNum == 3) {
					HidePanel (panel2_3);
					// text2_3.transform.DOScaleX(0,0.5f);
				} else if (panelNum == 4) {
					HidePanel (panel2_4);
					// text2_4.transform.DOScaleX(0,0.5f);
				} else if (panelNum == 5) {
					HidePanel (panel2_5);
					// text2_5.transform.DOScaleX(0,0.5f);
				}
			}

			panelNum = 0;
		} 

		OnBackCamera ();
	}

	public static void ShowPanel(GameObject a){                    //面板的显示
		// if (panelNum == 1 && isfront) {
		// 	a.transform.DOScale (new Vector3(0.9f, 0.5f, 1f),0.5f);
		// } else {
			a.transform.DOScale (new Vector3(0.2f, 0.2f, 1f),0.5f);
		// }
	}

	public static void HidePanel(GameObject a){
		
		a.transform.DOScale (new Vector3(0f, 0f, 0f),0.5f);

	}

	public static void OnShiftZ(){                   //放大

		camera.transform.DOMoveZ (zoomPos, 1);
	}

	public static void OnShiftXY(){                  //沿XY轴移动镜头

		Vector3 temp = camera.transform.position;
		temp.x = axisX;
		temp.y = axisY;
		camera.transform.position = temp;

//		camera.transform.DOMove (new Vector3 (axisX, axisY, 0f), 1);
	}

	public static void OnSwitchBody(){               //切换体型

		if (isfront) {
			camera.transform.DORotate (new Vector3 (0f, 180f, 0f), 1);
		} else {
			camera.transform.DORotate (new Vector3 (0f, 0f, 0f), 1);
		}
		ResetCamera ();
		isfront = !isfront;
	}

	public static void OnRotate(){                   //旋转

//		Vector3 posA = a.transform.localEulerAngles;
//		Vector3 posB = b.transform.localEulerAngles;
//		posA.y = 360 + posA.y;
//		posB.y = -360 + posB.y;
//
//		a.transform.localRotation = Quaternion.Euler (angle);
//		b.transform.localRotation = Quaternion.Euler (-angle);

		male.transform.DOLocalRotate (new Vector3 (-90f, 360, 180f), 5);
		male_fat.transform.DOLocalRotate (new Vector3 (-90f, 180, 180f), 5);

	}

	public static void OnBackCamera(){               //回到相机初始位置
		// video.Stop();
		organVideo.Stop ();
		organ_video.SetActive (false);
		bones.SetActive (false);
		camera.transform.DOMove (new Vector3 (0f, 15f, 0f), 0.5f);
	}

	public static void OnPlayVideo(){
		if (!video.isPlaying) {
			if (organVideo.isPlaying) {
				organVideo.Stop ();
				organVideo.transform.DOScale (new Vector3(0f, 0f, 2f),0.5f);
				camera.transform.DOMove (new Vector3 (3f, 30f, 15f), 0);
			}
			playButton.SetActive (false);
			video.Play ();
		} else {
			video.Pause ();
			playButton.SetActive (true);
		}
	}

	public static void GoVideo(){
		// video.Stop();
		organVideo.transform.DOScale (new Vector3(-2f, 2f, 2f),0.5f);
		camera.transform.DOMove (new Vector3 (0f, 32f, 38f), 0.5f);
		organVideo.Play ();
	}

	public static void ExitVideo(){
		organVideo.Stop ();
		organVideo.transform.DOScale (new Vector3(0f, 0f, 2f),0.5f);
		camera.transform.DOMove (new Vector3 (3f, 30f, 25f), 0.5f);
	}

	public static void GoOrgan(){
		camera.transform.DOMove (new Vector3 (0f, 24f, 45f), 0.5f);
		bones.transform.DOScale (new Vector3(0.005f, 0.005f, 0.005f),0.5f);;
	}

	public static void ExitOrgan(){
		bones.transform.DOScale (new Vector3(0f, 0f, 0.005f),0.5f);;
		camera.transform.DOMove (new Vector3 (6f, 24f, 25f), 0.5f);
	}
		
}
