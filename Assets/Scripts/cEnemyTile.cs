using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyTileType
{
    none = 0, UpTile = 1 ,DownTile, LeftTile ,RightTile, SpeedUp,SpeedDown
}
public class cEnemyTile : MonoBehaviour
{
   // [SerializeField]
    public int moveDirection;
    SpriteRenderer renderer;
    public int xPosition;
    public int yPosition;

    [SerializeField]
    Sprite[] tilesprite;

    public enemyTileType thisType;//0은 타일노드 1은 스파이크노드 2는 레이저
    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        init();   
    }
    public void init()
    {
        if(renderer ==null)
            renderer = GetComponent<SpriteRenderer>();

        renderer.sprite = tilesprite[(int)thisType];
        transform.position = cGameManager.instance.WorldTilePosition[xPosition, yPosition];
    }
    public void NodeEndEvnet()
    {
        transform.gameObject.SetActive(false);
    }
    public void AnimEvent_Direction()
    {
        //Invoke("NodeEndEvnet", 1.0f);
        //if (moveDirection != 0&& (int)thisType >=2)//0일때는 생성안함 1일때 왼쪽 2일때 오른쪽 3일때 위 4일때 아래
        //{
        //    int horizontal = moveDirection / 2;
        //    int vertical = moveDirection % 2;
        //    Vector2Int direction = Vector2Int.zero;
        //    if (horizontal == 0 && vertical == 1)
        //    {
        //        direction = Vector2Int.left;
        //    }
        //    if (horizontal == 1 && vertical == 0)
        //    {
        //        direction = Vector2Int.right;
        //    }
        //    if (horizontal == 1 && vertical == 1)
        //    {
        //        direction = Vector2Int.up;
        //    }
        //    if (horizontal == 2 && vertical == 0)
        //    {
        //        direction = Vector2Int.down;
        //    }
        //    Vector2Int result = new Vector2Int(xPosition, yPosition) + direction;
        //    if (result.x >= cGameManager.MapSize-1 || result.x < 1 || result.y >= cGameManager.MapSize-1 || result.y < 1)
        //    {
        //        return;
        //    }
        //    cGameManager.instance.notecontroller.NoteSpawn(result.x, result.y, moveDirection, (int)thisType);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cPlayerController PC = collision.transform.GetComponent<cPlayerController>();
            //cMovement2D PCmove = collision.transform.GetComponent<cMovement2D>();
            //if(PCmove.IsMove== false)
            //{
            //    if (PC.NowXPos == xPosition && PC.NowYPos == yPosition)
            //    {
            //        PC.OnHit();
            //    }
            //}
            switch (thisType)
            {
                case enemyTileType.SpeedUp:
                    PC.SetPlayerSpeed(3.0f);
                    break;
                case enemyTileType.SpeedDown:
                    PC.SetPlayerSpeed(-3.0f);
                    break;
            }
            //print(thisType);
        }
    }
}
