using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public struct NodeData
{
    public int _nodetype;
    public int _x;
    public int _y;
    public int _dir;
}
[Serializable]
public struct NodeSaveInfo
{
    public int _Time;
    public NodeData[] _Nodes;
}
[Serializable]
public class MusicNodeData 
{
    public NodeSaveInfo[] _NodesSave;
}
