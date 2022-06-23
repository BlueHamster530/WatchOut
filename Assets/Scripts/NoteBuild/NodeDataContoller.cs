using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NodeDataContoller : MonoBehaviour
{
    static GameObject _container;
    static GameObject Container
    {
        get
        {
            return _container;
        }
    }
    static NodeDataContoller _instance;
    public static NodeDataContoller Instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "NodeDataController";
                _instance = _container.AddComponent(typeof(NodeDataContoller)) as NodeDataContoller;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }
    public MusicNodeData _GamenodeData;
    public void LoadData()
    {
         string file = cNodeBuildManager.instance.GetSoundPath() + ".json";//³ëÆ®Âï±âºôµå½Ã »ç¿ë
        // string file = cNodeBuildManager.instance.GetSoundPath() + ".json";
        if (File.Exists(file))
        {
            print("Load Done");
            string FromJsonData = File.ReadAllText(file);
            _GamenodeData = JsonUtility.FromJson<MusicNodeData>(FromJsonData);
        }
        else
        {
            print("Create New Data");
            _GamenodeData = new MusicNodeData();
            SaveData();
        }
    }
    public void SaveData(MusicNodeData value = null)
    {
        string ToJsonData;
        if (value == null)
            ToJsonData = JsonUtility.ToJson(_GamenodeData);
        else
            ToJsonData = JsonUtility.ToJson(value);
         string filePath = cNodeBuildManager.instance.GetSoundPath() + ".json";
         File.WriteAllText(filePath, ToJsonData);
    }
    private void OnApplicationQuit()
    {
        SaveData();
    }
}
