using UnityEngine;
using UnityEngine.UI;

public class NodeMapUI : MonoBehaviour
{
    private Button _button;
    private MapNode _nodeData; // 이 노드가 가진 정보 (좌표, 타입 등)

    public void Setup(MapNode data)
    {
        _nodeData = data;
        _button = GetComponent<Button>();

        // 버튼 클릭 리스너 등록
        _button.onClick.AddListener(OnNodeClick);

        // 데이터에 따른 UI 설정 (예: 아이콘 변경)
        // GetComponent<Image>().sprite = ...
    }

    private void OnNodeClick()
    {
        // 1. 현재 선택된 노드 정보를 저장
        MapManager.Instance.SetCurrentNode(_nodeData);

        // 2. 씬 전환 실행
        MapManager.Instance.OnNodeSelected(MapManager.Instance.SelectedNode);
    }
}