using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlLight : MonoBehaviour {
	private float last;
	private bool move = false;
	private int keep = 1;
	private int stop = 0;
	private LightGestureListener listener;
	public bool slideChangeWithGestures = true;
	public bool slideChangeWithKeys = true;
 	// Use this for initialization
	void Start () {
		// hide mouse cursor
		Screen.showCursor = false;
		listener = GameObject.Find("Lights").GetComponent<LightGestureListener>();
	}
	
	// Update is called once per frame
	
	void FixedUpdate(){
		//键盘输入
		if (slideChangeWithKeys) {
			if (Input.GetKeyDown (KeyCode.C)) {
				//添加力矩旋转
				transform.GetComponent<Rigidbody> ().AddTorque (new Vector3 (0, 180, 0));
			}
		}
		//手势输入
		if (slideChangeWithGestures) {
			KinectManager kinectManager = KinectManager.Instance;
			if((!kinectManager || !kinectManager.IsInitialized() || !kinectManager.IsUserDetected()))
				return;
			if(listener.IsSwipeLeft()){
				transform.GetComponent<Rigidbody> ().AddTorque (new Vector3 (0, 180, 0));
		  }
		}
		if (Input.GetKeyDown (KeyCode.V)) {
			if(move){
				Debug.Log("wind stop");
				move = false;
			}else{
				move = true;
				last = Time.time;
				Debug.Log("wind begin");
			}
		}
		
		if (move) {
			
			if((Time.time-last)>stop){
	//			Debug.Log("stop is okay");
				if(Mathf.Abs (Time.time -last-stop)<keep){
					Debug.Log("move");
					int strength = Random.Range(0,3)*10;
					//已经用fixed的joint链接，给一个水平力
					transform.GetComponent<Rigidbody> ().AddForce (new Vector3 (0, 0, -1) * strength);
				}else{
					last  = Time.time;
					//mo模拟风力的持续时间
					keep = Random.Range(2,6);
					//模拟没有风的时间
					stop = Random.Range(3,10);
				}
			}
			
		}
	}
    
}
