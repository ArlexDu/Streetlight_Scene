using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;
public class JsonManager : MonoBehaviour {

    private string filename = "data.txt";
    //灯的位置
    private float Slight1_y;
    private float Slight2_y;
    private float Slight3_y;
    private float Slight4_y;
    private float Flight1_y;
    private float Flight2_y;
    private float Flight3_y;
    private float Flight4_y;
    //随机烟花的位置
    private float[] fireworkpositionx = new float[10];
    private float[] fireworkpositiony = new float[10];
    private float[] fireworkpositionz = new float[10];
    //判断出现的场景
    private bool fireworks;
    private bool particalBg;
    //判断控制的方式，调试专用
    private bool ControlbyKeyboard;
    private bool Controlbykinect;
    private JsonData data;
    //初始的强度
    private float intensity;
    public static JsonManager shareJsonManager = null;
    void Awake()
    {
		StreamReader reader = new StreamReader(Application.dataPath+"/StreamingAssets/"+filename);
        string rawData = reader.ReadToEnd();
        Debug.Log("rawData is "+rawData);
        reader.Close();
        data = JsonMapper.ToObject(rawData);
        getvalue();
    }

    private void getvalue()
    {
        //导入灯的位置
        Slight1_y = float.Parse(data["Slight1_y"].ToString());
        Slight2_y = float.Parse(data["Slight2_y"].ToString());
        Slight3_y = float.Parse(data["Slight3_y"].ToString());
        Slight4_y = float.Parse(data["Slight4_y"].ToString());
        Flight1_y = float.Parse(data["Flight1_y"].ToString());
        Flight2_y = float.Parse(data["Flight2_y"].ToString());
        Flight3_y = float.Parse(data["Flight3_y"].ToString());
        Flight4_y = float.Parse(data["Flight4_y"].ToString());
        //导入烟花的随机位置
        for(int i = 0; i < 10; i++)
        {
            string position = "position" + (i+1) + "_x";
            fireworkpositionx[i] = float.Parse(data[position].ToString());
            position = "position" + (i + 1) + "_y";
            fireworkpositiony[i] = float.Parse(data[position].ToString());
            position = "position" + (i + 1) + "_z";
            fireworkpositionz[i] = float.Parse(data[position].ToString());
        }
        //背景的打开，烟花还是粒子效果
        fireworks = bool.Parse(data["fireworks"].ToString());
        particalBg = bool.Parse(data["particalBg"].ToString());
        //控制的方式
        ControlbyKeyboard = bool.Parse(data["ControlByKeyBoard"].ToString());
        Controlbykinect = bool.Parse(data["ControlBykinect"].ToString());
        //灯的初始亮度
        intensity = float.Parse(data["Intensity"].ToString());
    }

    public static JsonManager instance {
        get {
            if (shareJsonManager == null)
            {
                shareJsonManager=FindObjectOfType(typeof(JsonManager)) as JsonManager;

            }
            return shareJsonManager;
        } }

    public float getSvalue(int id)
    {
        switch (id)
        {
            case 1: return Slight1_y;
            case 2: return Slight2_y;
            case 3: return Slight3_y;
            case 4: return Slight4_y;
            default: return 0;
        }
    }

    public float getFvalue(int id)
    {
        switch (id)
        {
            case 1: return Flight1_y;
            case 2: return Flight2_y;
            case 3: return Flight3_y;
            case 4: return Flight4_y;
            default: return 0;
        }
    }

    public float getfireworkX(int i)
    {
        return fireworkpositionx[i];
    }
    public float getfireworkY(int i)
    {
        return fireworkpositiony[i];
    }
    public float getfireworkZ(int i)
    {
        return fireworkpositionz[i];
    }
    public bool getfireworks()
    {
        return fireworks;
    }
    public bool getparticalbg()
    {
        return particalBg;
    }
    public bool getcontrolbyKey()
    {
        return ControlbyKeyboard;
    }
    public bool getcontrolbyKinect()
    {
        return Controlbykinect;
    }
    public float getIntensity()
    {
        return intensity;
    }
}
