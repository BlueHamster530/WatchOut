using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class cGameManager : MonoBehaviour
{
    public static cGameManager instance;
    static public int MapSize { get; set; } = 10;
    public cNoteSpawner notecontroller;
    public Vector3[,] WorldTilePosition { get; set; } = new Vector3[MapSize, MapSize];//0과 9는 여백타일
    public GameObject[] EnemyTiles;//기본 0 스파이크1
    public bool isPause;
    public float CurrnetPlayTime { get; set; } = 0.0f;

    AudioSource audio;

    [SerializeField]
    TextMeshProUGUI PlayTimeText;
    [SerializeField]
    TMP_InputField SetTimeInputField;
    [SerializeField]
    bool IsNodeBuild;
    [SerializeField]
    GameObject NodeBuildTool;
    void Awake()
    {
        instance = this;
        audio = GetComponent<AudioSource>();
        //audio.clip= cGameDataManager.instance.GetStageMusicClip();//일반게임시사용
        WorldTileInit();
        audio.clip = cNodeBuildManager.instance.GetSoundClip();//노트빌드때사용
        Invoke("GameStart", 3.0f);
        isPause = false;
        if (NodeBuildTool.activeSelf == false)
        {
            if(IsNodeBuild == true)
            {
                NodeBuildTool.SetActive(true);
            }
        }
    }

    private void WorldTileInit()
    {
        for (int i = 0; i < MapSize; i++)
        {
            for (int ii = 0; ii < MapSize; ii++)
            {
                WorldTilePosition[i, ii] = new Vector3(-4.785f +(i*1.085f) , -5.3f + ( ii * 1.085f),0);
                //Instantiate(maptiletest, WorldTilePosition[i, ii], Quaternion.identity);
            }
        }
    }
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void SetSoundClip(AudioClip sound)
    {
        if (audio == null)
            audio = GetComponent<AudioSource>();
        audio.clip = sound;
    }
    private void GUIUpdate()
    {
        float Minit = audio.time / 60.0f;
        float Second = audio.time % 60.0f;
        PlayTimeText.text = string.Format("{0:00}:{1:00.00}", (int)Minit, Second);
    }
    private void Update()
    {
        CurrnetPlayTime += Time.deltaTime;
        GUIUpdate();
    }
    private void GameStart()
    {
        audio.Play();
    }
    public void GamePause()
    {
        isPause = !isPause;
        if (isPause == true)
        {
            audio.Pause();
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            audio.Play();
        }
    }
    public void ChangeAudioTime(float _time)
    {
        audio.time = _time;
    }
    public void GoToSetTime()
    {
        float text = int.Parse(SetTimeInputField.text) / 100.0f;
        if (text >= audio.maxDistance || text < 0)
        {
            return;
        }
        //audio.time = text;
        //CurrnetPlayTime = text+3.0f;
        //for (int i = 0; i < notecontroller.GameNodeInfo.Length; i++)
        //{
        //    if (notecontroller.GameNodeInfo[i].IsSpawned == true)
        //    {
        //        if (notecontroller.GameNodeInfo[i].time >= CurrnetPlayTime)
        //        {
        //            notecontroller.GameNodeInfo[i].IsSpawned = false;
        //        }
        //    }
        //    else
        //    {
        //        break;
        //    }
        //}
        GameObject.FindWithTag("Player").GetComponent<cPlayerController>().PlayerSetMoveToTime(text + 3.0f);


    }

    public void ReturnToMainMenu()
    {
        audio.Stop();
        SceneManager.LoadScene("GameMainMenu");
    }


}
