using UnityEngine;

public class MapManager : MonoBehaviour
{
    public MapData mapData;
    public MapGenerator generator;

    public static MapManager Instance; // 접근 편의를 위한 싱글톤

    public MapNode SelectedNode;
    public MapNode PlayerNode;
    public PlayerData playerData;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // 중복 방지
    }

    void Start()
    {
        PlayerNode = playerData.playerNode;
        mapData.currentFloor = PlayerNode.blueprintPos.y;
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

    public void SetCurrentNode(MapNode node)
    {
        SelectedNode = node;
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
            Debug.Log("s");
            // 현재 노드의 nextNodes에 선택한 노드의 좌표가 있는지 체크
            // (실제 구현 시 현재 노드 데이터도 MapData에 저장해둬야 함)
            if (mapData.currentFloor != -1)
            {
                for (int i = 0; i < PlayerNode.nextNodes.Count; i++)
                {
                    Debug.Log("v");
                    if (PlayerNode.nextNodes[i].x == selectedNode.blueprintPos.x)
                    {
                        Debug.Log("sa");
                        mapData.currentFloor = selectedNode.blueprintPos.y;
                        mapData.currentPlayerNode = selectedNode.blueprintPos;
                        PlayerNode = SelectedNode;
                        playerData.playerNode = PlayerNode;

                        // 전투 노드라면 전투 씬으로!
                        if (selectedNode.nodeType == NodeType.Battle)
                        {
                            UnityEngine.SceneManagement.SceneManager.LoadScene("BattleScene");
                        }
                    }
                }
            }else if(mapData.currentFloor == -1)
            {
                Debug.Log("a");
                mapData.currentFloor = selectedNode.blueprintPos.y;
                mapData.currentPlayerNode = selectedNode.blueprintPos;
                PlayerNode = SelectedNode;
                playerData.playerNode = PlayerNode;
                // 전투 노드라면 전투 씬으로!
                if (selectedNode.nodeType == NodeType.Battle)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("BattleScene");
                }
            }
            /*mapData.currentFloor = selectedNode.blueprintPos.y;
            mapData.currentPlayerNode = selectedNode.blueprintPos;
            PlayerNode = SelectedNode;
            // 전투 노드라면 전투 씬으로!
            if (selectedNode.nodeType == NodeType.Battle)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("BattleScene");
            }*/
        }
    }
}