using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private MapData mapData;

    public void GenerateMap()
    {
        mapData.nodes.Clear();

        // 1. 노드 생성 (0~9층)
        for (int y = 0; y < 10; y++)
        {
            int nodesInFloor = Random.Range(3, 6);
            for (int x = 0; x < nodesInFloor; x++)
            {
                NodeType type = (y == 9) ? NodeType.Boss : NodeType.Battle;
                type = (y == 4) ? NodeType.Rest : NodeType.Battle;
                mapData.nodes.Add(new MapNode(x, y, type));
            }
        }

        // 2. 계층 간 연결 로직 (0층부터 8층까지 수행)
        for (int y = 0; y < 9; y++)
        {
            ConnectLayers(y);
        }

        Debug.Log("절차적 맵 생성 및 검증 완료!");
    }

    private void ConnectLayers(int currentY)
    {
        var currentFloor = mapData.nodes.Where(n => n.blueprintPos.y == currentY).ToList();
        var nextFloor = mapData.nodes.Where(n => n.blueprintPos.y == currentY + 1).ToList();

        // [Step 1] Out-degree 보장: 현재 층의 모든 노드에서 위로 길을 냄
        foreach (var curr in currentFloor)
        {
            // 인접한(x 차이 1 이하) 다음 층 노드들을 찾음
            var neighbors = nextFloor.Where(next => Mathf.Abs(next.blueprintPos.x - curr.blueprintPos.x) <= 1).ToList();

            if (neighbors.Count > 0)
            {
                // 인접 노드가 있으면 랜덤하게 1~2개 연결
                var targets = neighbors.OrderBy(x => Random.value).Take(Random.Range(1, 3));
                foreach (var t in targets) curr.nextNodes.Add(t.blueprintPos);
            }
            else
            {
                // 인접 노드가 없으면 가장 가까운 노드 하나 강제 연결
                var closest = nextFloor.OrderBy(next => Mathf.Abs(next.blueprintPos.x - curr.blueprintPos.x)).First();
                curr.nextNodes.Add(closest.blueprintPos);
            }
        }

        // [Step 2] In-degree 보장: 다음 층 노드 중 '선택받지 못한' 노드 구제
        foreach (var next in nextFloor)
        {
            // 현재 층 노드들 중 이 next 노드를 가리키는 곳이 있는지 확인
            bool hasIncoming = currentFloor.Any(curr => curr.nextNodes.Contains(next.blueprintPos));

            if (!hasIncoming)
            {
                // 나를 선택한 놈이 없다면, 현재 층에서 가장 가까운 노드가 나를 가리키게 함
                var closestParent = currentFloor.OrderBy(curr => Mathf.Abs(curr.blueprintPos.x - next.blueprintPos.x)).First();
                closestParent.nextNodes.Add(next.blueprintPos);
            }
        }
    }
}