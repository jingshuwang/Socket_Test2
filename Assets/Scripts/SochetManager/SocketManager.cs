using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using DG.Tweening;

//using LitJson;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SocketManager : MonoBehaviour {

	SocketIOComponent m_socket;
	float zoomPos0;
	float axisX0;
	float axisY0;

	// GameObject ca;

	public static bool isCompare;

	// Use this for initialization
	void Start () {

		m_socket = GetComponent<SocketIOComponent>();

		if (m_socket != null) {

			//系统的事件
			m_socket.On ("opem_socketn", OnSocketOpen);
			m_socket.On ("error", OnSocketError);
			m_socket.On ("close", OnSocketClose);
			m_socket.On ("connect", OnSocketConnect);
			//自定义的事件
			m_socket.On ("ClientListener", OnClientListener);

		} 
			
		DontDestroyOnLoad (this.gameObject);

		// ca = GameObject.Find ("MiCamera");

	}

	// Update is called once per frame
	void Update () {
	
		// if(Input.GetMouseButton(0)){
		// 	ca.transform.DORotate (new Vector3 (0f, 180f, 0f), 1);

		// }

	}
		
	#region 注册的事件

	public void OnSocketConnect(SocketIOEvent ev) {
		Debug.Log("OnSocketConnect updated socket id " + m_socket.sid);
		Dictionary<string, string> data = new Dictionary<string, string>();
		data["message"] = "connected";
		m_socket.Emit("VRConnect", new JSONObject(data), OnServerListenerCallback);
	}

	public void OnSocketOpen(SocketIOEvent ev) {
		Debug.Log("OnSocketOpen updated socket id " + m_socket.sid);
	}

	public void OnClientListener(SocketIOEvent e) {
		Debug.Log(string.Format("OnClientListener name: {0}, data: {1}", e.name, e.data));
		String msgString = e.data ["message"].str;

		if (msgString == "sync_succeed") {
			Splash.SplashGoTo (3);
		} 
		else if (msgString == "load_succeed") {
			Splash.SplashGoTo (4);
		} 
		else if (msgString == "sn") {
			Splash.SplashGoTo (3);
		} 
		else if (msgString == "es") {
			Splash.SplashGoTo (7);
		} 
		else if (msgString == "gm"){
			Splash.SplashGoTo (5);
		} 
		else if (msgString.StartsWith ("ps_play_")) {
			SocketManagerVideo.videoNum = int.Parse (msgString.Substring (8));
			SocketManagerVideo.OnPlayVideo (int.Parse (msgString.Substring (8)));
		} 
		else if (msgString == "ps_pause") {
			if (SocketManagerVideo.isPlay) {
				SocketManagerVideo.OnPauseVideo ();
				SocketManagerVideo.isPlay = !SocketManagerVideo.isPlay;
			} else {
				SocketManagerVideo.OnPlayVideo (SocketManagerVideo.videoNum);
				SocketManagerVideo.isPlay = !SocketManagerVideo.isPlay;
			}
		} 
		else if (msgString == "ps_exit") {
			SocketManagerVideo.OnExitVideo ();
		} 
		else if (msgString == "cd") {
			Splash.SplashGoTo (3);
		} 
		else if (msgString.StartsWith("vd_")){       //特定位置聚焦显示
			if (SocketManagerModels2.panelNum != 0) {
				SocketManagerModels2.ViewDetailBefore();
			}

			SocketManagerModels2.panelNum = int.Parse (msgString.Substring (3)) + 1;
			SocketManagerModels2.ViewDetailAfter();
		} 
		else if (msgString == "rc")    //回到整体视角 
		{
			if (!isCompare) {
				SocketManagerModels2.ResetCamera ();
			} else {
				SocketManagerModels3.OnBackCamera ();
			}
		}
		else if (msgString == "switch_normal") {    //切换正常体型 
			if (!isCompare) {
				if (SocketManagerModels2.isfront) {
					SocketManagerModels2.OnSwitchBody ();
				}
			} else {
				SocketManagerModels2.isfront = false;
				Splash.SplashGoTo (5);
			}

		} 
		else if (msgString == "switch_fat") {       //切换肥胖体型 
			if (!isCompare) {
				if (!SocketManagerModels2.isfront) {
					SocketManagerModels2.OnSwitchBody ();
				}
			} else {
				SocketManagerModels2.isfront = true;
				Splash.SplashGoTo (5);
			}
		}
		else if (msgString == "switch_comparison")    //切换对比体型 
		{
			if (!isCompare) {
				Splash.SplashGoTo (6);
			}
		}
		else if (msgString.StartsWith("hr"))   //水平旋转
		{
			if (!isCompare) {
				SocketManagerModels2.OnRotate ();
			} else{
				SocketManagerModels3.OnRotate ();
			}
		}
		else if (msgString.StartsWith("zt_"))   //zoom-in
		{
			zoomPos0 = float.Parse (msgString.Substring (3)) * 5f;

			if(!isCompare){
				if (SocketManagerModels2.isfront) {
					SocketManagerModels2.zoomPos = zoomPos0;
				} else {
					SocketManagerModels2.zoomPos = -zoomPos0;
				}
				SocketManagerModels2.OnShiftZ ();
			} else{
				SocketManagerModels3.zoomPos = zoomPos0;
				SocketManagerModels3.OnShiftZ ();
			}
				
		}
		else if (msgString.StartsWith("ls_"))   //移动视角
		{
			string[] substr;
			msgString = msgString.Substring(3);
			substr = msgString.Split ('&');

			if(!isCompare){
				axisX0 = ((float.Parse(substr [0])) * 2) / 43 - 15;
				axisY0 = - (float.Parse(substr [1]))/11 + 38;

				if (SocketManagerModels2.isfront) {
					SocketManagerModels2.axisX = axisX0;
				} else {
					SocketManagerModels2.axisX = -axisX0;
				}
				SocketManagerModels2.axisY = axisY0;

				SocketManagerModels2.OnShiftXY ();
			} else{
				SocketManagerModels3.axisX = ((float.Parse (substr [0])) * 6) / 43 - 39.5f;
				SocketManagerModels3.axisY = -(float.Parse (substr [1])) / 11 + 38;
				SocketManagerModels3.OnShiftXY ();
			}

		}
		else if(msgString.StartsWith("vc")){   //播放视频
			SocketManagerModels2.OnPlayVideo();
		}
		else if(msgString.StartsWith("go_video")){    //zoom-in器官，VR视频
			SocketManagerModels2.GoVideo();
		}
		else if(msgString.StartsWith("exit_video")){    //zoom-out器官，VR视频
			SocketManagerModels2.ExitVideo();
		}
		else if(msgString == "oz_in"){                   //zoom-in器官,3d模型
			SocketManagerModels2.GoOrgan();
		}
		else if(msgString == "oz_out"){                   //zoom-out器官,3d模型
			SocketManagerModels2.ExitOrgan();
		}
		else if (msgString == "pa") {       //播放对比动画
			SocketManagerModels3.OnPlayAnimation();
		}

	}

	public void OnSocketError(SocketIOEvent e) {
		Debug.Log("OnSocketError: " + e.name + " " + e.data);
	}

	public void OnSocketClose(SocketIOEvent e) {
		Debug.Log("OnSocketClose: " + e.name + " " + e.data);
	}

	#endregion

	public void OnServerListenerCallback(JSONObject json) {
		Debug.Log(string.Format("OnServerListenerCallback data: {0}", json));
	}

}

