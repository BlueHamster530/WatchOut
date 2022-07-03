using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NodeBuild_CamEffect : MonoBehaviour
{
    [SerializeField]
    float fPlayerSpeed = 1.0f;
    [SerializeField]
    public int NowXPos;
    [SerializeField]
    public int NowYPos;
    int nCurrentWay = 0;//0은좌측상단 1은 우측상단 2는 우측하단 3은 좌측하단
    float nCurrentMoveTime = 0;
    Vector3[] vWayPoint = new Vector3[4];
    cNodeBuild nearEnemyTile = new cNodeBuild();
    [SerializeField]
    public cNoteLabs notelabs;
    bool IsStart;
    float fStartWaitTime = 1.0f;
    bool IsAlreadySetNodeType = false;
    [SerializeField]
    GameObject NodeShowPrefab;
    StreamWriter file;
    // Start is called before the first frame update
    void Start()
    {
        NowXPos = Mathf.Clamp(NowXPos, 1, 8);
        NowYPos = Mathf.Clamp(NowXPos, 1, 8);
        transform.position = notelabs.WorldTilePosition[NowXPos, NowYPos];
        vWayPoint[0] = notelabs.WorldTilePosition[1, 8];
        vWayPoint[1] = notelabs.WorldTilePosition[8, 8];
        vWayPoint[2] = notelabs.WorldTilePosition[8, 1];
        vWayPoint[3] = notelabs.WorldTilePosition[1, 1];
        if (File.Exists("Assets/CamEffectTimeLine")==false)
        {
           string path = Application.streamingAssetsPath + "/CamEffectTimeLine.txt";
            file = new StreamWriter(path);
        }
        print("!@#");
    }
    public void MusicPlay()
    {
        IsStart = !IsStart;
        if (fStartWaitTime <= 0)
        {
            notelabs.MusicPlay();
        }
    }
    private void MovementFunction()
    {
        if (IsStart == false) return;

        if (fPlayerSpeed <= 0)
            nCurrentMoveTime += Time.deltaTime * 0.1f * 0.1f;
        else
            nCurrentMoveTime += Time.deltaTime * fPlayerSpeed * 0.1f;
        if (nCurrentWay < 3)
        {
            transform.position = Vector3.Lerp(vWayPoint[nCurrentWay], vWayPoint[nCurrentWay + 1], nCurrentMoveTime);
        }
        else
        {
            transform.position = Vector3.Lerp(vWayPoint[3], vWayPoint[0], nCurrentMoveTime);
        }
        if (nCurrentMoveTime >= 1.0f)
        {
            nCurrentWay++;
            if (nCurrentWay >= 4)
                nCurrentWay = 0;

            transform.position = vWayPoint[nCurrentWay];
            nCurrentMoveTime = 0;
        }
    }
    private void FixedUpdate()
    {
        MovementFunction();
    }
    private void KeyEvent()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            file.WriteLine(notelabs.ReturnAudioPlayTime());
            file.Flush();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            file.WriteLine($"{notelabs.ReturnAudioPlayTime()}-wKey");
            file.Flush();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsStart == true)
        {
            if (fStartWaitTime > 0)
            {
                fStartWaitTime -= Time.deltaTime;
                if (fStartWaitTime <= 0)
                {
                    notelabs.MusicPlay();
                }
            }
            else
            {
                KeyEvent();
            }
        }
    }
    private void OnDisable()
    {
        file.Close();
    }
    private void OnDestroy()
    {
        file.Close();
    }

}
