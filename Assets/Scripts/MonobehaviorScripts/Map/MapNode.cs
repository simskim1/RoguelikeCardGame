using System.Collections.Generic;
using UnityEngine;
using static TreeEditor.TreeEditorHelper;

[System.Serializable]
public class MapNode
{
    public Vector2Int blueprintPos; // x: 가로 칸, y: 층수
    public NodeType nodeType;
    public List<Vector2Int> nextNodes = new List<Vector2Int>(); // 연결된 다음 노드들의 좌표
    public bool isVisited = false;

    public MapNode(int x, int y, NodeType type)
    {
        blueprintPos = new Vector2Int(x, y);
        nodeType = type;
    }
}