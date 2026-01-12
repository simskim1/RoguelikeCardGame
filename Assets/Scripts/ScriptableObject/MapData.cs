using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMapData", menuName = "Map/MapData")]
public class MapData : ScriptableObject
{
    public List<MapNode> nodes = new List<MapNode>();
    public Vector2Int currentPlayerNode; // 현재 플레이어가 위치한 노드 좌표
    public int currentFloor = 0;
}