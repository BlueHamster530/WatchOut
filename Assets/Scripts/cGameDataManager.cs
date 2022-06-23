using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class cGameDataManager : MonoBehaviour
{
    public static cGameDataManager instance;

    [SerializeField]
    string PathData;

    MusicNodeData StageNodeData;

    [SerializeField]
    AudioClip StageMusicClip;
    [SerializeField]
    GameObject test;
    // Start is called before the first frame update


    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetMusic(AudioClip _clip)
    {
        StageMusicClip = _clip;
        PathData = Application.streamingAssetsPath + "/NodeBuild/" + StageMusicClip.name + ".mp3.json";
       if (File.Exists(PathData))
       {
           print("Load Done");
           string FromJsonData = File.ReadAllText(PathData);
           StageNodeData = JsonUtility.FromJson<MusicNodeData>(FromJsonData);
       }
    }

    public MusicNodeData GetMusicnodeData()
    {
        return StageNodeData;
    }
    public AudioClip GetStageMusicClip()
    {
        return StageMusicClip;
    }
}
