using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum NodeScoreType
{
    Bad = 0, Normal=25, Nice=50, Good=75, Perfect =100
}
public class cPlayerController : MonoBehaviour
{
    [SerializeField]
    int nHp = 3;
    [SerializeField]
    float fPlayerSpeed= 1.0f;


    [SerializeField]
    public int NowXPos;
    [SerializeField]
    public int NowYPos;
    private cMovement2D movement2D;

    int nCurrentWay = 0;//0은좌측상단 1은 우측상단 2는 우측하단 3은 좌측하단
    float nCurrentMoveTime=0;
    Vector3[] vWayPoint = new Vector3[4];

    cEnemyTile[] nearEnemyTile = new cEnemyTile[2];
    private int nScore;

    [Header("거리별판정기준")]
    [SerializeField]
    float fDistanceBad;
    [SerializeField]
    float fDistanceNormal;
    [SerializeField]
    float fDistanceNice;
    [SerializeField]
    float fDistanceGood;
    [SerializeField]
    float fDistancePerfect;
    [SerializeField]
    TextMeshProUGUI ScoreText;

    bool isMulSpeed = false;
    float GoToTime = 0;

    [SerializeField]
    CameraController cCamController;
    // Start is called before the first frame update
    void Awake()
    {
        movement2D = GetComponent<cMovement2D>();
        NowXPos = Mathf.Clamp(NowXPos, 1, 8);
        NowYPos = Mathf.Clamp(NowXPos, 1, 8);
        isMulSpeed = false;
         transform.position = cGameManager.instance.WorldTilePosition[NowXPos, NowYPos];
         vWayPoint[0] = cGameManager.instance.WorldTilePosition[1, 8];
         vWayPoint[1] = cGameManager.instance.WorldTilePosition[8, 8];
         vWayPoint[2] = cGameManager.instance.WorldTilePosition[8, 1];
         vWayPoint[3] = cGameManager.instance.WorldTilePosition[1, 1];
    }
    public void PlayerSetMoveToTime(float _time)
    {
        if (_time <= cGameManager.instance.CurrnetPlayTime) return;
        isMulSpeed = true;
        GoToTime = _time;
        Time.timeScale = 10;
    }
    private void SetmoveToTimSpeedup()
    {
        if (isMulSpeed == true)
        {
            if (cGameManager.instance.CurrnetPlayTime >= GoToTime)
            {
                cGameManager.instance.ChangeAudioTime(GoToTime - 3.0f);
                isMulSpeed = false;
                Time.timeScale = 1;
            }
        }
    }
    private void MovementFunction()
    {
        nCurrentMoveTime += Time.deltaTime * fPlayerSpeed  * 0.1f;
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
    private void CheckDistNode()
    {
        if (nearEnemyTile[0] != null)
        {
            float distance = Vector2.Distance(transform.position, nearEnemyTile[0].transform.position);
            if (distance > fDistanceBad)
            {
                SubEnemyTileInCode();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        KeyEvent();
        CheckDistNode();
        SetmoveToTimSpeedup();
    }
    private void HitScore(NodeScoreType _Score)
    {
        cCamController.VivrateForTime(0.1f);
        nScore += (int)_Score;
        ScoreText.text = nScore.ToString();
        print(_Score);
    }
    public void SetPlayerSpeed(float Value)
    {
        fPlayerSpeed += Value;
    }
    private void NodePress(enemyTileType presstile)
    {
        print("Press" + presstile);
        if (nearEnemyTile[0] == null) return;
        if (nearEnemyTile[0].thisType != presstile)
        {
            HitScore(NodeScoreType.Bad);
        }
        else
        {
            float distance = Vector2.Distance(transform.position, nearEnemyTile[0].transform.position);
            if (distance >= fDistanceBad)
            {
                HitScore(NodeScoreType.Bad);
            }
            else if (distance >= fDistanceNormal)
            {
                HitScore(NodeScoreType.Normal);
            }
            else if (distance >= fDistanceNice)
            {
                HitScore(NodeScoreType.Nice);
            }
            else if (distance >= fDistanceGood)
            {
                HitScore(NodeScoreType.Good);
            }
            else if (distance >= fDistancePerfect)
            {
                HitScore(NodeScoreType.Perfect);
            }
        }
        nearEnemyTile[0].NodeEndEvnet();
        if (nearEnemyTile[1] != null)
        {
            nearEnemyTile[0] = nearEnemyTile[1];
            nearEnemyTile[1] = null;
        }
        else
            nearEnemyTile[0] = null;
    }
    private void KeyEvent()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            NodePress(enemyTileType.UpTile);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            NodePress(enemyTileType.DownTile);

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NodePress(enemyTileType.LeftTile);

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NodePress(enemyTileType.RightTile);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            cGameManager.instance.GamePause();
        }
    }
    public void OnHit()
    {
        print("hit");
        nHp--;
        //if (nHp <= 0)
        //{
        //    print("dwa");
        //    cGameManager.instance.ChangeScene("SampleScene");
        //}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyTile"))
        {
            AddEnemyTileInCode(collision.transform.GetComponent<cEnemyTile>());
        }
    }

    private void AddEnemyTileInCode(cEnemyTile ctile)
    {
        bool IsClear = false;
        for (int i = 0; i < 2; i++)
        {
            if (nearEnemyTile[i] == null)
            {
                nearEnemyTile[i] = ctile;
                IsClear = true;
                break;
            }
        }
        if (IsClear == false)
        {
            nearEnemyTile[0].NodeEndEvnet();
            HitScore(NodeScoreType.Bad);
            nearEnemyTile[0] = nearEnemyTile[1];
            nearEnemyTile[1] = ctile;
        }
    }
    private void SubEnemyTileInCode()
    {
        if (nearEnemyTile[0] == null) return;
        HitScore(NodeScoreType.Bad);
        nearEnemyTile[0].NodeEndEvnet();
        if (nearEnemyTile[1] != null)
        {
            nearEnemyTile[0] = nearEnemyTile[1];
            nearEnemyTile[1] = null;
        }
        else
        {
            nearEnemyTile[0] = null;
        }
    }

}
