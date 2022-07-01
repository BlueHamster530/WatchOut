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
    public void TypeChange(int num)
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
