using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Node
{
    public Node(float _time,int _x, int _y, int _dir, int _type)
    {
        time = _time;
        x = _x;
        y = _y;
        direction = _dir;
        type = _type;
        IsSpawned = false;
    }
    public int x, y;
    public int direction;
    public int type;
    public float time;
    public bool IsSpawned;
}
public class cNoteSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] TileNode;
    cEnemyTile[] enemytilecodes;
    [SerializeField]
    Sprite[] tilenodeSprite;

    cEnemyTile[] enemySpikecodes;

    public Node[] GameNodeInfo;

    public MusicNodeData MusicNodeTimeLine;

    private void Start()
    {
        enemytilecodes = new cEnemyTile[TileNode.Length];
        for (int i = 0; i < TileNode.Length; i++)
        {
            enemytilecodes[i] = TileNode[i].GetComponent<cEnemyTile>();
            TileNode[i].SetActive(false);
        }

        //MusicNodeTimeLine = cGameDataManager.instance.GetMusicnodeData();//일반시사용
        MusicNodeTimeLine = NodeDataContoller.Instance._GamenodeData;//노드빌드시사용
        int NodesLenth = MusicNodeTimeLine._NodesSave.Length;
        for (int i = 0; i < MusicNodeTimeLine._NodesSave.Length; i++)
        {
            NodesLenth += MusicNodeTimeLine._NodesSave[i]._Nodes.Length;
        }
        if (NodesLenth > 0)
        {
            GameNodeInfo = new Node[NodesLenth];
           // print(MusicNodeTimeLine._NodesSave[0]._Time);
            int TimeLable = 0;
            for (int i = 0; i < MusicNodeTimeLine._NodesSave.Length; i++)
            {
                for (int ii = 0; ii < MusicNodeTimeLine._NodesSave[i]._Nodes.Length; ii++)
                {
                    GameNodeInfo[TimeLable] = new Node((MusicNodeTimeLine._NodesSave[i]._Time / 100.0f)+3.0f,
                        MusicNodeTimeLine._NodesSave[i]._Nodes[ii]._x,
                        MusicNodeTimeLine._NodesSave[i]._Nodes[ii]._y,
                        0,
                        MusicNodeTimeLine._NodesSave[i]._Nodes[ii]._nodetype
                        );
                    TimeLable++;
                }
            }
        }
    }
    public void NoteSpawn(int x, int y, int direction, int type)
    {
         for (int i = 0; i < TileNode.Length; i++)
         {
             if (TileNode[i].activeSelf == false)
             {
                 TileNode[i].transform.position = cGameManager.instance.WorldTilePosition[x, y];
                 enemytilecodes[i].xPosition = x;
                 enemytilecodes[i].yPosition = y;
                 enemytilecodes[i].moveDirection = 0;
                 enemytilecodes[i].thisType = (enemyTileType)type;
                 // enemytilecodes[i].GetComponent<SpriteRenderer>().sprite = tilenodeSprite[direction];
                 enemytilecodes[i].init();
                 TileNode[i].SetActive(true);
                 break;
             }
         }
    }

    private void NodeLoad()
    {
        for (int i = 0; i < GameNodeInfo.Length; i++)
        {
            if (cGameManager.instance.CurrnetPlayTime >= GameNodeInfo[i].time && GameNodeInfo[i].IsSpawned == false)
            {
                NoteSpawn(GameNodeInfo[i].x, GameNodeInfo[i].y, 0, GameNodeInfo[i].type);
                GameNodeInfo[i].IsSpawned = true;
            }
        }
    }
    public void Update()
    {
        NodeLoad();
    }
}
