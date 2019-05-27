using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using DG.Tweening;

public class AnimateTest : MonoBehaviour {

	public GameObject camera;
	public GameObject panel;
	public GameObject bodyFat;
	public GameObject bodyNormal;
	public AudioSource audio;

	public Text _1s;
	public Text _4s;
	public Text _7s;
	public Text _10s;

	public Text sub_1s;
	public Text sub_4s;
	public Text sub_7s;
	public Text sub_10s;

	public Text _meet;
	public Text _the;
	public Text _new;
	public Text _you;

	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {

//		if (Input.GetMouseButton (0)) {
//			SocketManagerModels3.isPlay = true;
//		}
		if (SocketManagerModels3.isPlay) {
			panel.transform.DOScaleX (0, 0);
			bodyFat.transform.DOMoveX (0, 0);
			bodyNormal.SetActive (false);
			AboutText ();
			AboutCamera ();
			audio.Play ();
			StartCoroutine (OnBodyChange());
			StartCoroutine (OnPanel());
			StartCoroutine (OnBodyBack());

			SocketManagerModels3.isPlay = false;
		}
		
	}

	void AboutText(){

		StartCoroutine (OnSplash (_1s, 0.5f, 2.0f));
		StartCoroutine (OnTextFade (_1s, 2.5f));
		StartCoroutine (OnTextFade (sub_1s, 2.5f));


		StartCoroutine (OnSplash (_4s, 3.5f, 2.0f));
		StartCoroutine (OnTextFade (_4s, 5.5f));
		StartCoroutine (OnTextFade (sub_4s, 5.5f));


		StartCoroutine (OnSplash (_7s, 6.5f, 2.0f));
		StartCoroutine (OnTextFade (_7s, 8.5f));
		StartCoroutine (OnTextFade (sub_7s, 8.5f));


		StartCoroutine (OnSplash (_10s, 9.5f, 2.0f));
		StartCoroutine (OnTextFade (_10s, 11.5f));
		StartCoroutine (OnTextFade (sub_10s, 11.5f));


		StartCoroutine (OnSplash (_meet, 14.0f, 1.5f));
		StartCoroutine (OnSplash (_the, 14.5f, 1.5f));
		StartCoroutine (OnSplash (_new, 15.0f, 1.5f));
		StartCoroutine (OnSplash (_you, 15.5f, 1.5f));  

		StartCoroutine (OnTextFade (_meet, 17f));
		StartCoroutine (OnTextFade (_the, 17f));
		StartCoroutine (OnTextFade (_new, 17f));
		StartCoroutine (OnTextFade (_you, 17f));   
	
	}

	void AboutCamera(){

		StartCoroutine (OnCameraMove (0));
		StartCoroutine (OnCameraMove (3));
		StartCoroutine (OnCameraMove (6));
		StartCoroutine (OnCameraMove (9));
		StartCoroutine (OnCameraMove (12));

	}

	IEnumerator OnSplash(Text a, float b, float c)
	{
		yield return new WaitForSeconds(b);//等待时间
		a.transform.DOScaleX (1, c);
	}

	IEnumerator OnTextFade(Text a, float b)
	{
		yield return new WaitForSeconds(b);//等待时间
		a.DOFade (0, 0.5f);
	}

	IEnumerator OnCameraMove(float a)
	{
		yield return new WaitForSeconds(a);//等待时间

		if (a == 0) {
			camera.transform.DOMove (new Vector3 (0f, 30f, 20f), 0.5f);
		} else if (a == 3) {
			camera.transform.DOMove (new Vector3 (25f, 25f, 40f), 0.5f);
			camera.transform.DOLocalRotate (new Vector3 (0f, -90f, 0f), 0.5f);
		} else if (a == 6) {
			camera.transform.DOMove (new Vector3 (0f, 15f, 60f), 0.5f);
			camera.transform.DOLocalRotate (new Vector3 (0f, 180f, 0f), 0.5f, RotateMode.FastBeyond360);
		} else if (a == 9) {
			camera.transform.DOMove (new Vector3 (-20f, 0f, 40f), 0.5f);
			camera.transform.DOLocalRotate (new Vector3 (0f, 90f, 0f), 0.5f, RotateMode.FastBeyond360);
		} else if (a == 12) {
			camera.transform.DOMove (new Vector3 (0f, 15f, 0f), 0.5f);
			camera.transform.DOLocalRotate (new Vector3 (0f, 0f, 0f), 0.5f, RotateMode.FastBeyond360);
		}
		

	}

	IEnumerator OnBodyChange(){

		yield return new WaitForSeconds(13);//等待时间
		bodyFat.transform.DOScaleX (0.5f, 1);
	}

	IEnumerator OnBodyBack()
	{
		yield return new WaitForSeconds(18f);//等待时间
		bodyNormal.SetActive(true);
		bodyFat.transform.DOScaleX (0.8f, 0);
		bodyFat.transform.DOMoveX (-10, 0);
	}

	IEnumerator OnPanel()
	{
		yield return new WaitForSeconds(16.5f);//等待时间
		panel.transform.DOScaleX (1, 1.5f);
	}

}
