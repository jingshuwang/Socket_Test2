using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using DG.Tweening;

//using LitJson;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SocketManagerModels3 : MonoBehaviour {

//	SocketIOComponent m_socket;
	public static int panelNum = 0;
	public static float angle;
	public static float zoomPos;
	public static float axisX;
	public static float axisY;
	public static bool isPlay = false;

	public static GameObject camera;
	public static GameObject male;
	public static GameObject male_fat;

	// Use this for initialization
	void Start () {

		SocketManager.isCompare = true;

		camera = GameObject.Find ("MiCamera");
		male = GameObject.Find ("Male");
		male_fat = GameObject.Find ("MaleFat");

	}

	// Update is called once per frame
	void Update () {

	}
		
	#region 注册的事件


	#endregion

	public static void OnShiftZ(){                   //放大

		camera.transform.DOMoveZ (zoomPos, 1);
	}

	public static void OnShiftXY(){                  //沿XY轴移动镜头

		Vector3 temp = camera.transform.position;
		temp.x = axisX;
		temp.y = axisY;
		camera.transform.position = temp;

	}

	public static void OnRotate(){                   //旋转

		//		Vector3 posA = a.transform.localEulerAngles;
		//		Vector3 posB = b.transform.localEulerAngles;
		//		posA.y = 360 + posA.y;
		//		posB.y = -360 + posB.y;

		//		a.transform.localRotation = Quaternion.Euler (angle);
		//		b.transform.localRotation = Quaternion.Euler (-angle);

		male.transform.DOLocalRotate (new Vector3 (-90f, 360f, 180f), 5);
		male_fat.transform.DOLocalRotate (new Vector3 (-90f, 360f, 180f), 5);

	}

	public static void OnBackCamera(){               //回到相机初始位置
		camera.transform.DOMove (new Vector3 (0f, 15f, 0f), 0.5f);
	}

	public static void OnPlayAnimation(){               //回到相机初始位置
		isPlay = true;
	}
		
}
