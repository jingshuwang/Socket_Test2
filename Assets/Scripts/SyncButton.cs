using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SyncButton : MonoBehaviour {

	public Text buttonText;

	// Use this for initialization
	void Start () {

	}

//    IEnumerator GetText()
//    {
//        using (UnityWebRequest www = UnityWebRequest.Get("http://honeybadgerworks.com/bariatric-vr/sync-request-code.json"))
//        {
//            yield return www.Send();
//
//            if (www.isNetworkError)
//            {
//                Debug.Log(www.error);
//				buttonText.text = www.error;
//            }
//            else
//            {
//                Debug.Log(www.downloadHandler.text);
//                MyClass myClass = JsonUtility.FromJson<MyClass>(www.downloadHandler.text);
//
//                // myClass.paringCode 是返回的配对码，可以显示到 button 上或者跳到其他场景显示
//                Debug.Log(myClass.paringCode);
//				buttonText.text = myClass.paringCode;
//
//            }
//        }
//    }

    // Update is called once per frame
    void Update () {
		
	}

    public void onClick (){
//        StartCoroutine(GetText());
		buttonText.text = "qwe123";
    }
}

[Serializable]
public class MyClass
{
    public string paringCode;
    public GeneratedAt generatedAt;
}

[Serializable]
public class GeneratedAt
{
    public Date date;
    public Time1 time;
}

[Serializable]
public class Date
{
    public int year;
    public int month;
    public int day;
}

[Serializable]
public class Time1
{
    public int hour;
    public int minute;
    public int second;
    public int nano;
}