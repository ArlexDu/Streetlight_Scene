using UnityEngine;
using System.Collections;
using LitJson;
public class FetchInternet : MonoBehaviour {

	WWW http;
	string url;
	private float lastbright;
    float timeStart = 0;
    float duration = 1;
	// Use this for initialization
	void Start () {
        timeStart = Time.time;
		url = "http://10.60.42.62:8083/light/getOne?id=1";
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - timeStart > duration) {
            http = new WWW(url);
            StartCoroutine(WaitforRequest(http));
            InvokeRepeating("getInfo", 2, 0.1f);
            timeStart = Time.time;
        }
	}
	private void getInfo(){
		StartCoroutine (WaitforRequest(http));
	}
	/*
	 * json 格式：
	 * {
	 * "color" : "000050",
	 * "bright":  "1.5",
	 * "id"    :  2
 	 * }
	 * 
	 */

	IEnumerator WaitforRequest(WWW www){
		yield return www;
		if (www.error != null) {
			Debug.Log ("failed! "+ www.error);
		} else {
			if(www.isDone){
		//		Debug.Log ("succeed!");
				JsonData data = JsonMapper.ToObject(www.text);
				string bright = (string)data["bright"];
				//Debug.Log("bright is "+ bright);
				//改变灯光的强度
				if(lastbright!=float.Parse(bright)/34){
					GameObject.Find("Light").GetComponent<ControlLight>().changeIntensity(float.Parse(bright)/34);
					lastbright = float.Parse(bright)/34;
				}
			}
		}
	}
}
