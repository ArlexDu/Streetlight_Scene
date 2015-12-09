using UnityEngine;
using System.Collections;

public class ControlSence : MonoBehaviour {

    private Transform[] lights = new Transform[4];
	private int currentlight = 0;//当前展示的灯
	private int max;
    public GameObject man;
    private LightGestureListener listener;
    private bool ControlByKeyBoard;
    private bool ControlBykinect;
    private bool light = false;//当前是有灯的状态下
    // Use this for initialization
    void Start () {
        JsonManager jsonmanager = JsonManager.instance;
        //放置背景
        if (!jsonmanager.getfireworks())//关闭烟花
        {
            GameObject.Find("Position").SetActive(false);
        }
        if (!jsonmanager.getparticalbg())//关闭粒子效果
        {
            GameObject.Find("Background").SetActive(false);
        }
        ControlByKeyBoard = jsonmanager.getcontrolbyKey();
        ControlBykinect = jsonmanager.getcontrolbyKinect();
        //放置灯的位置
        Debug.Log("ShowLight");
        Transform Light = GameObject.Find ("Lights").transform;
		for (int i=0; i<Light.childCount; i++) {
			lights[i] = Light.GetChild(i);
            lights[i].localPosition = new Vector3(0, jsonmanager.getSvalue(i + 1), 0);
            lights[i].GetChild(0).localPosition = new Vector3(0, jsonmanager.getFvalue(i + 1), 0);
            if (i!=0){
                Debug.Log(i+"Hide");
                lights[i].gameObject.SetActive(false);
			}else{
                Debug.Log(i+"Show");
                lights[i].gameObject.SetActive(true);
			}
		}
		max = Light.childCount - 1;
        // hide mouse cursor
        Screen.showCursor = false;
        listener = GameObject.Find("Lights").GetComponent<LightGestureListener>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (ControlByKeyBoard)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                lights[currentlight].gameObject.SetActive(false);
                currentlight = (currentlight + 1) > max ? 0 : (currentlight + 1);
                Debug.Log("Count is " + currentlight);
                lights[currentlight].FindChild("Light").localPosition = new Vector3(0, 0, 0);
                lights[currentlight].FindChild("Light").localRotation = new Quaternion(0, 0, 0, 0);
                lights[currentlight].FindChild("fixed").localRotation = new Quaternion(0, 0, 0, 0);
                Debug.Log(lights[currentlight].FindChild("Light").localPosition.y);
                lights[currentlight].gameObject.SetActive(true);
            }
        }
        //手势输入
        if (ControlBykinect)
        {
                KinectManager kinectManager = KinectManager.Instance;
                if ((!kinectManager || !kinectManager.IsInitialized() || !kinectManager.IsUserDetected()))
                    return;
                if (listener.IsJump())
                {
                    if (light)
                    {
                        Debug.Log("show man");
                        hidelightshowman();
                    }
                    else
                    {
                        Debug.Log("show light");
                        hidemanshowlight();
                    }

                }
        }
    }

    private void hidelightshowman()
    {
        Transform Light = GameObject.Find("Lights").transform;
        for (int i = 0; i < Light.childCount; i++)
        {
                lights[i].gameObject.SetActive(false);
        }
        man.SetActive(true);
        light = false;
    }

    private void hidemanshowlight()
    {
        man.SetActive(false);
        JsonManager jsonmanager = JsonManager.instance;
        Transform Light = GameObject.Find("Lights").transform;
        for (int i = 0; i < Light.childCount; i++)
        {
            lights[i] = Light.GetChild(i);
            lights[i].localPosition = new Vector3(0, jsonmanager.getSvalue(i + 1), 0);
            lights[i].GetChild(0).localPosition = new Vector3(0, jsonmanager.getFvalue(i + 1), 0);
            if (i != 0)
            {
                lights[i].gameObject.SetActive(false);
            }
            else {
                lights[i].gameObject.SetActive(true);
            }
        }
        light = true;
    }
}
