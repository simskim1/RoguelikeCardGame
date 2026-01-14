using UnityEngine;

public class MapManager : MonoBehaviour
{
    public MapData mapData;
    public MapGenerator generator;

    void Start()
    {
        if (mapData.nodes.Count == 0)
        {
            // 데이터가 없으면 새로 생성 (게임 시작 시)
            generator.GenerateMap();
        }
        else
        {
            // 데이터가 있으면 기존 맵 로드 (전투 종료 후 복귀 시)
            LoadExistingMap();
        }
        MapDisplayer.Instance.DisplayMap();
    }

    void LoadExistingMap()
    {
        Debug.Log("이전 맵 상태를 복구합니다. 현재 층: " + mapData.currentFloor);
        // 2일차에 구현할 UI 생성 함수를 여기서 호출하게 됩니다.
    }

    public void OnNodeSelected(MapNode selectedNode)
    {
        // 이동 가능 조건: 현재 층+1 이면서 현재 노드와 연결되어 있는지 확인
        if (selectedNode.blueprintPos.y == mapData.currentFloor + 1)
        {
            // 현재 노드의 nextNodes에 선택한 노드의 좌표가 있는지 체크
            // (실제 구현 시 현재 노드 데이터도 MapData에 저장해둬야 함)

            mapData.currentFloor = selectedNode.blueprintPos.y;
            mapData.currentPlayerNode = selectedNode.blueprintPos;

            // 전투 노드라면 전투 씬으로!
            if (selectedNode.nodeType == NodeType.Battle)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("BattleScene");
            }
        }
    }
}