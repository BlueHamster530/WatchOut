using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBuilder_KeyBoard : MonoBehaviour
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
    cNoteLabs notelabs;
    bool IsStart;
    float fStartWaitTime = 3.0f;
    bool IsAlreadySetNodeType=false;
    [SerializeField]
    GameObject NodeShowPrefab;
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

        if(fPlayerSpeed<=0)
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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TileBuild(enemyTileType.UpTile);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TileBuild(enemyTileType.DownTile);

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TileBuild(enemyTileType.LeftTile);

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TileBuild(enemyTileType.RightTile);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            TileBuild(enemyTileType.SpeedUp);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            TileBuild(enemyTileType.SpeedDown);
        }
    }
    private void TileBuild(enemyTileType type)
    {
        if (nearEnemyTile == null|| IsAlreadySetNodeType==true) return;
        nearEnemyTile.TypeChange((int)type);
        notelabs.SaveNodeInPlay();
        IsAlreadySetNodeType = true;
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
    private void SetPlayerSpeed(float Value)
    {
        fPlayerSpeed += Value;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EmptyTile"))
        {
            AddEnemyTileInCode(collision.transform.GetComponent<cNodeBuild>());
        }
    }

    private void AddEnemyTileInCode(cNodeBuild ctile)
    {
        if (nearEnemyTile != null)
        {
            if (nearEnemyTile.nodetype == (int)enemyTileType.SpeedUp)
                SetPlayerSpeed(3.0f);
            if (nearEnemyTile.nodetype == (int)enemyTileType.SpeedDown)
                SetPlayerSpeed(-3.0f);

            GameObject clone =Instantiate(NodeShowPrefab, nearEnemyTile.transform.position, Quaternion.identity);
            clone.GetComponent<ShowNodePrefab>().Init(nearEnemyTile.nodetype);
            nearEnemyTile.TypeChange(0);
        }
        nearEnemyTile = ctile;
        IsAlreadySetNodeType = false;
    }


}
