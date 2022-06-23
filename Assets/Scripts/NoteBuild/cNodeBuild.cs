using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cNodeBuild : MonoBehaviour
{
    public int nodetype = 0;
    cNoteLabs notelabs;
    SpriteRenderer sprite;

    [SerializeField]
    Sprite[] TypeImage;

    public int PosX  = 0;
    public int PosY = 0;
    public int direction= 0;

    [SerializeField]
    GameObject DirectionCheck;
    private void Awake()
    {
        notelabs = GameObject.Find("Main Camera").GetComponent<cNoteLabs>();
        sprite = GetComponent<SpriteRenderer>();
        nodetype = 0;
        sprite.sprite = null;
        SetPos(0, 0);
        direction = 0;
    }
    public void SetPos(int _x, int _y)
    {
        PosX = _x;
        PosY = _y;
    }
    private void OnMouseOver()
    {
        if (notelabs.IsMusicPlay() == false)
        {
            if (Input.GetMouseButton(0))
            {
                TypeChange(notelabs.GetWahtIGetOn());
            }

           if (nodetype != 0)
           {
               if (Input.GetMouseButton(1))
               {
                    // if (nodetype >= 2)
                    //     SetDirection(direction + 1);
                    // else
                    //     SetTileNodeColor(direction + 1);
                    TypeChange(0);
               }
           }
        }
    }
    private void TypeChange(int num)
    {
        nodetype = num;
        DirectionCheck.SetActive(false);
        if (nodetype == 0)
        {
            sprite.sprite = null;
        }
        else
        {
            sprite.sprite = TypeImage[nodetype];
            direction = 0;
            //if (nodetype == 1)
            //{
            //    direction = 0;
            //}
            //if (nodetype >= 2)
            //{
            //    if (direction != 0)
            //        DirectionCheck.SetActive(true);
            //}
        }
    }
    private void SetDirection(int num)
    {
        if (nodetype == 0) return;
        direction = num;
        if (direction >= 5)
            direction = 0;
        if (direction == 0)
            DirectionCheck.SetActive(false);
        else
        {
            if (DirectionCheck.activeSelf == false)
                DirectionCheck.SetActive(true);
        }
        switch (direction)
        {
            case 0:
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }break;
            case 1://왼쪽
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                break;
            case 2://오른쪽
                {
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                break;
            case 3://위쪽
                {
                    transform.rotation = Quaternion.Euler(0, 0, 270);
                }
                break;
            case 4://아래쪽
                {
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                break;

        }
    }

    public void SetNodeInfo(int _type, int _direction)
    {
        nodetype = _type;
        if (nodetype == 0)
        {
            sprite.sprite = null;
            return;
        }
        sprite.sprite = TypeImage[nodetype];
        DirectionCheck.SetActive(false);
    }
}
