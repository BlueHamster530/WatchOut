using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
public class MusicSelectInfomation : MonoBehaviour
{
    [SerializeField]
    AudioClip Music;
    [SerializeField]
    CamEffectInfo CamEffectNode;

    public void ButtonClicked(int _Difficult)
    {
        cGameDataManager.instance.SetMusic(Music, CamEffectNode, _Difficult);
        SceneManager.LoadScene("GameStage");
    }
}
