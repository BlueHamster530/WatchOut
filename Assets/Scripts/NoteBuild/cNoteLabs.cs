using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;

public class cNoteLabs : MonoBehaviour
{
    //타일,스파이크,레이저,카메라흔들기,화면흰색이팩트
    public struct NodeInfo
    {
        public NodeInfo(int _nodetype, int _x, int _y, int _dir)
        {
            nodeType = _nodetype;
            x = _x;
            y = _y;
            dir = _dir;
        }
        public int nodeType;
        public int x;
        public int y;
        public int dir;
    }

    public struct TimeInfo
    {
        public NodeInfo[] Nodes;
        public int IsActiveInfo;
    }

    [SerializeField]
    Sprite[] image;

    [SerializeField]
    GameObject mouseObject;

    [SerializeField]
    GameObject TilePrefab;

    int MusicCurrntPlayTime;

    public Vector3[,] WorldTilePosition { get; set; } = new Vector3[10, 10];//0과 9는 여백타일


    TimeInfo[] holyshit = new TimeInfo[100000];

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    TextMeshProUGUI PlayButton;

    [SerializeField]
    TextMeshProUGUI CurrentMusicTime;

    [SerializeField]
    GameObject[] TileNode;

    [SerializeField]
    GameObject[] SpikeNode;

    [SerializeField]
    TMP_InputField TimeGrid;

    [SerializeField]
    TMP_InputField GoToTime;
    
    public int WhatIGetOn = 0;
    cNodeBuild[] nodebuildcodes = new cNodeBuild[100];
    private void Start()
    {
        for (int i = 0; i < 100000; i++)
        {
            holyshit[i].Nodes = new NodeInfo[100];
        }
        for (int i = 0; i < 10; i++)
        {
            for (int ii = 0; ii < 10; ii++)
            {
                WorldTilePosition[i, ii] = new Vector3(-4.345f + (i * 0.965f), -4.345f + (ii * 0.965f), 0);
               GameObject clone = Instantiate(TilePrefab, WorldTilePosition[i, ii], Quaternion.identity);
                clone.transform.parent = GameObject.Find("NodeTiles").transform;
                nodebuildcodes[(i*10)+ii] = clone.GetComponent<cNodeBuild>();
                nodebuildcodes[(i * 10) + ii].SetPos(i, ii);
            }
        }       
        audio.clip = cNodeBuildManager.instance.GetSoundClip();
        NodeDataContoller.Instance.LoadData();
        MusicLoad();
        MusicPlay();
        MusicPlay();
    }

    public void MusicPlay()
    {
        if (audio.isPlaying)
        {
            PlayButton.text = "Play";
            audio.Pause();
            LoadNodes();
        }
        else
        {
            PlayButton.text = "Pause";
            SaveNodes();
            audio.Play();
        }
    }

    public bool IsMusicPlay()
    {
        return audio.isPlaying;
    }

    private void GUIUpdate()
    {
        float Minit = audio.time /60.0f;
        float Second = audio.time % 60.0f;
        CurrentMusicTime.text = string.Format("{0:00}:{1:00.00}", (int)Minit, Second);
    }
    public void MuSicReset()
    {
        SaveNodes();
        audio.Pause();
        PlayButton.text = "Play";
        audio.time = 0;
        LoadNodes();
    }
    public void MusicTimeChange(float _time)
    {
        SaveNodes();
        audio.time += _time;
        LoadNodes();
        audio.Pause();
    }
    private void KeyEvent()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            MusicPlay();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            MuSicReset();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))//지우기
        {
            ChangeMouseGet(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)|| Input.GetKeyDown(KeyCode.UpArrow))//위
        {
            ChangeMouseGet(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.DownArrow))//아래
        {
            ChangeMouseGet(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.LeftArrow))//왼쪽
        {
            ChangeMouseGet(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.RightArrow))//오른쪽
        {
            ChangeMouseGet(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)|| Input.GetKeyDown(KeyCode.UpArrow)&& Input.GetKey(KeyCode.RightShift))//속도업
        {
            ChangeMouseGet(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightShift))//속도다운
        {
            ChangeMouseGet(6);
        }
    }
    private void ChangeMouseGet(int num)
    {
        WhatIGetOn = num;
        mouseObject.GetComponent<SpriteRenderer>().sprite = image[num];
    }
    void Update()
    {

        MusicCurrntPlayTime = Mathf.FloorToInt(audio.time * 100);
        KeyEvent();
        GUIUpdate();

        Vector3 MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePoint.z = 0;
        mouseObject.transform.position = MousePoint;

    }

    public int GetWahtIGetOn()
    {
        return WhatIGetOn;
    }
    public Sprite GetWahtIGetOnImage()
    {
        return image[WhatIGetOn];
    }
    public void SaveNodes()
    {
        MusicCurrntPlayTime = Mathf.FloorToInt(audio.time * 100);
        holyshit[MusicCurrntPlayTime].Nodes = new NodeInfo[100];
        holyshit[MusicCurrntPlayTime].IsActiveInfo = 0;
        for (int i = 0; i < 100; i++)//맵사이즈변경시 (x*10+)y로 조건 변경
        {
            holyshit[MusicCurrntPlayTime].Nodes[i] = new NodeInfo(nodebuildcodes[i].nodetype, nodebuildcodes[i].PosX, nodebuildcodes[i].PosY, nodebuildcodes[i].direction);
            if(nodebuildcodes[i].nodetype>=1)
                holyshit[MusicCurrntPlayTime].IsActiveInfo = 1;
        }
    }
    public void SaveNodeInPlay()
    {
        MusicCurrntPlayTime = Mathf.FloorToInt(audio.time * 100);
        //MusicCurrntPlayTime -= 200;
        if (MusicCurrntPlayTime <= 0)
            MusicCurrntPlayTime = 0;
        holyshit[MusicCurrntPlayTime].Nodes = new NodeInfo[100];
        holyshit[MusicCurrntPlayTime].IsActiveInfo = 0;
        for (int i = 0; i < 100; i++)//맵사이즈변경시 (x*10+)y로 조건 변경
        {
            holyshit[MusicCurrntPlayTime].Nodes[i] = new NodeInfo(nodebuildcodes[i].nodetype, nodebuildcodes[i].PosX, nodebuildcodes[i].PosY, nodebuildcodes[i].direction);
        }
        holyshit[MusicCurrntPlayTime].IsActiveInfo = 1;
        //int musictime = Mathf.FloorToInt(audio.time * 100);
        //if(holyshit[musictime].IsActiveInfo == 0)
        //    holyshit[musictime].IsActiveInfo = 1;
        //holyshit[musictime].Nodes[(_x*10)+ _y] = 
        //    new NodeInfo(_type,_x, _y,0);
    }
    public void LoadNodes()
    {
        MusicCurrntPlayTime = Mathf.FloorToInt(audio.time * 100);
        for (int i = 0; i < holyshit[MusicCurrntPlayTime].Nodes.Length; i++)//맵사이즈변경시 (x*10+)y로 조건 변경
        {
            nodebuildcodes[i].SetNodeInfo(holyshit[MusicCurrntPlayTime].Nodes[i].nodeType, holyshit[MusicCurrntPlayTime].Nodes[i].dir);
        }

    }
    public void MusciCurrentTimeMove(int _value)
    {
        audio.Pause();
        PlayButton.text = "Play";
        SaveNodes();
        float text = int.Parse(TimeGrid.text);
        float gototimeis = audio.time + (text/100.0f) * _value;
        if (gototimeis >= audio.maxDistance || gototimeis < 0)
        {
            return;
        }
        audio.time = gototimeis;
        MusicCurrntPlayTime = Mathf.FloorToInt(audio.time * 100);
        LoadNodes();
    }
    public void GoToMusciCurrentTimeMove()
    {
        audio.Pause();
        PlayButton.text = "Play";
        SaveNodes();
        float text = int.Parse(GoToTime.text) / 100.0f;
        if (text >= audio.maxDistance || text < 0)
        {
            return;
        }
        audio.time = text;
        MusicCurrntPlayTime = Mathf.FloorToInt(audio.time * 100);
        LoadNodes();
    }

    public void FindBackOrFrontActiveNods(int _value)
    {
        SaveNodes();
        audio.Pause();
        bool isActive = false;
        int i = 0;
        if (_value == -1)
        {
            for (i = MusicCurrntPlayTime; i > 0; i--)
            {
                if (MusicCurrntPlayTime == i) continue;
                if (holyshit[i].IsActiveInfo == 1)
                {
                    break;
                }
            }
        }
        else if(_value == 1)
        {
            for (i = MusicCurrntPlayTime; i < 100000; i++)
            {
                if (MusicCurrntPlayTime == i) continue;
                if (holyshit[i].IsActiveInfo == 1)
                {
                    isActive = true;
                    break;
                }
            }
        }
        if (_value == 1 && isActive == false)
        {
            return;
        }
        MusicCurrntPlayTime = Mathf.Clamp(i,0,(int)((audio.maxDistance)*100.0f)+1);
        if (holyshit[MusicCurrntPlayTime].IsActiveInfo == 0)
        {
            MusicCurrntPlayTime += 1;
        }
        audio.time = MusicCurrntPlayTime/100.0f;
        LoadNodes();
    }
    public void MusicSaveNChangeScene(string NextScene)
    {
        SaveNodes();
        MusicNodeData newNodeData = new MusicNodeData();
        List<NodeSaveInfo> tempNodeSaveInfo = new List<NodeSaveInfo>();
        for (int i = 0; i < holyshit.Length; i++)
        {
            if (holyshit[i].IsActiveInfo == 1)
            {
                List<NodeData> tempNodeData = new List<NodeData>();
                for (int ii = 0; ii < holyshit[i].Nodes.Length; ii++)
                {
                    if (holyshit[i].Nodes[ii].nodeType != 0)
                    {
                        NodeData newJsonNodeData = new NodeData();
                        newJsonNodeData._nodetype = holyshit[i].Nodes[ii].nodeType;
                        newJsonNodeData._x = holyshit[i].Nodes[ii].x;
                        newJsonNodeData._y = holyshit[i].Nodes[ii].y;
                        newJsonNodeData._dir = holyshit[i].Nodes[ii].dir;
                        tempNodeData.Add(newJsonNodeData);
                    }
                }
                NodeSaveInfo newnodesaveinfo = new NodeSaveInfo();
                newnodesaveinfo._Time = i;
                newnodesaveinfo._Nodes = new NodeData[tempNodeData.Count];
                for (int ii = 0; ii < tempNodeData.Count; ii++)
                {
                    newnodesaveinfo._Nodes[ii] = tempNodeData[ii];
                }
                tempNodeSaveInfo.Add(newnodesaveinfo);
            }
        }
        newNodeData._NodesSave = new NodeSaveInfo[tempNodeSaveInfo.Count];
        for (int i = 0; i < newNodeData._NodesSave.Length; i++)
        {
            newNodeData._NodesSave[i] = tempNodeSaveInfo[i];
        }
        NodeDataContoller.Instance._GamenodeData = newNodeData;
        NodeDataContoller.Instance.SaveData(newNodeData);
        SceneManager.LoadScene(NextScene);
       
    }
    public void MusicLoad()
    {
        MusicNodeData myMusicNodeData = NodeDataContoller.Instance._GamenodeData;
        if (myMusicNodeData._NodesSave == null ) return;
        for (int i = 0; i <  myMusicNodeData._NodesSave.Length; i++)
        {
            holyshit[myMusicNodeData._NodesSave[i]._Time].IsActiveInfo = 1;

            for (int ii = 0; ii < myMusicNodeData._NodesSave[i]._Nodes.Length; ii++)
            {
                holyshit[myMusicNodeData._NodesSave[i]._Time].Nodes[(myMusicNodeData._NodesSave[i]._Nodes[ii]._x * 10) + myMusicNodeData._NodesSave[i]._Nodes[ii]._y].dir =
                  myMusicNodeData._NodesSave[i]._Nodes[ii]._dir;            
                holyshit[myMusicNodeData._NodesSave[i]._Time].
                    Nodes[(myMusicNodeData._NodesSave[i]._Nodes[ii]._x * 10) +
                    (myMusicNodeData._NodesSave[i]._Nodes[ii]._y)].x =
                  myMusicNodeData._NodesSave[i]._Nodes[ii]._x;
                holyshit[myMusicNodeData._NodesSave[i]._Time].
                    Nodes[(myMusicNodeData._NodesSave[i]._Nodes[ii]._x * 10) +
                    (myMusicNodeData._NodesSave[i]._Nodes[ii]._y)].y =
                  myMusicNodeData._NodesSave[i]._Nodes[ii]._y;
                holyshit[myMusicNodeData._NodesSave[i]._Time].
                    Nodes[(myMusicNodeData._NodesSave[i]._Nodes[ii]._x * 10) +
                    (myMusicNodeData._NodesSave[i]._Nodes[ii]._y)].nodeType =
                  myMusicNodeData._NodesSave[i]._Nodes[ii]._nodetype;
            }

        }
    }

    //public void MusicSaveNPlay()
    //{
    //    SaveNodes();
    //    MusicNodeData newNodeData = new MusicNodeData();
    //    List<NodeSaveInfo> tempNodeSaveInfo = new List<NodeSaveInfo>();
    //    for (int i = 0; i < holyshit.Length; i++)
    //    {
    //        if (holyshit[i].IsActiveInfo == 1)
    //        {
    //            List<NodeData> tempNodeData = new List<NodeData>();
    //            for (int ii = 0; ii < holyshit[i].Nodes.Length; ii++)
    //            {
    //                if (holyshit[i].Nodes[ii].nodeType != 0)
    //                {
    //                    NodeData newJsonNodeData = new NodeData();
    //                    newJsonNodeData._nodetype = holyshit[i].Nodes[ii].nodeType;
    //                    newJsonNodeData._x = holyshit[i].Nodes[ii].x;
    //                    newJsonNodeData._y = holyshit[i].Nodes[ii].y;
    //                    newJsonNodeData._dir = holyshit[i].Nodes[ii].dir;
    //                    tempNodeData.Add(newJsonNodeData);
    //                }
    //            }
    //            NodeSaveInfo newnodesaveinfo = new NodeSaveInfo();
    //            newnodesaveinfo._Time = i;
    //            newnodesaveinfo._Nodes = new NodeData[tempNodeData.Count];
    //            for (int ii = 0; ii < tempNodeData.Count; ii++)
    //            {
    //                newnodesaveinfo._Nodes[ii] = tempNodeData[ii];
    //            }
    //            tempNodeSaveInfo.Add(newnodesaveinfo);
    //        }
    //    }
    //    newNodeData._NodesSave = new NodeSaveInfo[tempNodeSaveInfo.Count];
    //    for (int i = 0; i < newNodeData._NodesSave.Length; i++)
    //    {
    //        newNodeData._NodesSave[i] = tempNodeSaveInfo[i];
    //    }
    //    NodeDataContoller.Instance._GamenodeData = newNodeData;
    //    NodeDataContoller.Instance.SaveData(newNodeData);
    //    SceneManager.LoadScene("GameStage");
    //}
}
