using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlLight : MonoBehaviour {
	private float last;
	private bool move = false;
	private int keep = 1;
	private int stop = 0;
	private LightGestureListener listener;
    private bool ControlByKeyBoard;
    private bool ControlBykinect;

    private float Light_intensity; // 存储当前的光强
    private float old_intensity; // 存储上一次变化的光强
    private float max_lighting = 3f;//最大的光照强度
    private GameObject lighting;
    // Use this for initialization
    void Start() {
         JsonManager jsonmanager = JsonManager.instance;
        // hide mouse cursor
        Screen.showCursor = false;
		listener = GameObject.Find("Lights").GetComponent<LightGestureListener>();
        ControlByKeyBoard = jsonmanager.getcontrolbyKey();
        ControlBykinect = jsonmanager.getcontrolbyKinect();
        lighting = transform.FindChild("lighting").gameObject;
        old_intensity = jsonmanager.getIntensity();
        lighting.GetComponent<Light>().intensity = old_intensity;
    }
	
	// Update is called once per frame
	
	void Update(){


		//键盘输入
		if (ControlByKeyBoard) {
            //旋转
            if (Input.GetKeyDown (KeyCode.C)) {
				//添加力矩旋转
				transform.GetComponent<Rigidbody> ().AddTorque (new Vector3 (0, 180, 0));
			}
            //随风飘动
            if (Input.GetKeyDown(KeyCode.V))
            {
                if (move)
                {
           //         Debug.Log("wind stop");
                    move = false;
                }
                else {
                    move = true;
                    last = Time.time;
           //         Debug.Log("wind begin");
                }
            }
            //亮度调节
            if (Input.GetKey(KeyCode.UpArrow))
            {
                TestChangeLightIntensity(0.05f);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {//点击下箭头光强减少
                TestChangeLightIntensity(-0.05f);
            }
        }
        //远程端对于亮度的调节,目前测试，以后要改这段代码
        if (false)
        {
            if (old_intensity < Light_intensity)
            {
                Debug.Log("change up");
                ChangeLightIntensity(0.05f);
            }
            else if (old_intensity > Light_intensity)
            {
                ChangeLightIntensity(-0.05f);
                Debug.Log("change down");
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

        //手势输入
        if (ControlBykinect)
        {
            KinectManager kinectManager = KinectManager.Instance;
            if ((!kinectManager || !kinectManager.IsInitialized() || !kinectManager.IsUserDetected()))
                return;
            if (listener.IsSwipeLeft())
            {
                transform.GetComponent<Rigidbody>().AddTorque(new Vector3(0, 180, 0));
            }
        }
    }


    //远程端改变光照的强度
    private void ChangeLightIntensity(float inten)
    {
        old_intensity += inten;
        if (old_intensity > max_lighting)
        {
            old_intensity = max_lighting;
        }
        else if (Light_intensity < 0)
        {
            Light_intensity = 0;
        }
        lighting.GetComponent<Light>().intensity = old_intensity;
        if ((old_intensity >= Light_intensity) && (inten > 0))
        {
            old_intensity = Light_intensity;
        }
        else if ((old_intensity <= Light_intensity) && (inten < 0))
        {
            old_intensity = Light_intensity;
        }
    }

    //测试改变光照的强度
    private void TestChangeLightIntensity(float inten)
    {
        old_intensity += inten;
        if (old_intensity > max_lighting)
        {
            old_intensity = max_lighting;
        }
        else if (Light_intensity < 0)
        {
            Light_intensity = 0;
        }
         lighting.GetComponent<Light>().intensity = old_intensity;
    }

    public void changeIntensity(float i)
    {
        Light_intensity = i;
    }
}
