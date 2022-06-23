using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cNodeBuildManager : MonoBehaviour
{
    public static cNodeBuildManager instance;
    string SoundPath;
    AudioClip soundClip;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene("NodeBuild");
    }
    public void SetSoundPath(string _value)
    {
        SoundPath = _value;
    }
    public void SetSoundClip(AudioClip _value)
    {
        soundClip = _value;
    }

    public string GetSoundPath()
    {
        return SoundPath;
    }
    public AudioClip GetSoundClip()
    {
        return soundClip;
    }

}
