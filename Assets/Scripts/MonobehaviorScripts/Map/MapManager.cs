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
    }

    void LoadExistingMap()
    {
        Debug.Log("이전 맵 상태를 복구합니다. 현재 층: " + mapData.currentFloor);
        // 2일차에 구현할 UI 생성 함수를 여기서 호출하게 됩니다.
    }
}