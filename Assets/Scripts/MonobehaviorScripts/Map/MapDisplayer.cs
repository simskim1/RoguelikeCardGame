using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapDisplayer : MonoBehaviour
{
    public static MapDisplayer Instance; // 접근 편의를 위한 싱글톤

    [SerializeField] private MapData mapData;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private RectTransform content;

    private float xSpacing = 150f;
    private float ySpacing = 200f;

    // 핵심: 논리적 좌표(Vector2Int)를 실제 UI 좌표(Vector2)로 연결하는 사전
    private Dictionary<Vector2Int, Vector2> nodePositions = new Dictionary<Vector2Int, Vector2>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // 중복 방지
    }


    public void DisplayMap()
    {
        nodePositions.Clear();
        foreach (Transform child in content) Destroy(child.gameObject);

        // 1. 전체 층수 파악 및 도화지 크기 설정
        int maxFloor = 0;
        foreach (var n in mapData.nodes)
        {
            if (n.blueprintPos.y > maxFloor) maxFloor = n.blueprintPos.y;
        }

        // 층 간격과 위아래 여백을 합쳐 전체 높이 계산
        float topPadding = 150f;
        float bottomPadding = 100f;
        float totalHeight = (maxFloor * ySpacing) + topPadding + bottomPadding;

        // 도화지 크기 설정 (Anchor가 Bottom 기준이어야 위로만 늘어남)
        content.sizeDelta = new Vector2(800, totalHeight);

        // 중요: 도화지가 중앙에 가 있는 것을 방지하기 위해 위치를 0으로 강제
        content.anchoredPosition = Vector2.zero;

        // 2. 층별 노드 개수 파악 (X축 정렬용)
        Dictionary<int, int> floorNodeCounts = new Dictionary<int, int>();
        foreach (var n in mapData.nodes)
        {
            if (!floorNodeCounts.ContainsKey(n.blueprintPos.y)) floorNodeCounts[n.blueprintPos.y] = 0;
            floorNodeCounts[n.blueprintPos.y]++;
        }

        // 3. 노드 생성 및 배치
        foreach (var node in mapData.nodes)
        {
            GameObject nodeObj = Instantiate(nodePrefab, content);

            NodeMapUI nodeUI = nodeObj.GetComponent<NodeMapUI>();
            if (nodeUI != null)
            {
                // 현재 루프에서 다루는 'node' 데이터를 넘겨줍니다.
                nodeUI.Setup(node);
            }

            RectTransform rect = nodeObj.GetComponent<RectTransform>();

            // X축: 층별 중앙 정렬
            int nodesInThisFloor = floorNodeCounts[node.blueprintPos.y];
            float centeredX = (node.blueprintPos.x - (nodesInThisFloor - 1) / 2f) * xSpacing;

            // Y축: 도화지 맨 바닥(0)에서 bottomPadding 만큼 띄워서 시작
            float yPos = (node.blueprintPos.y * ySpacing) + bottomPadding;

            Vector2 uiPos = new Vector2(centeredX, yPos);
            rect.anchoredPosition = uiPos;

            nodePositions.Add(node.blueprintPos, uiPos);

            var tmpText = nodeObj.GetComponentInChildren<TMPro.TMP_Text>();
            if (tmpText != null) tmpText.text = node.nodeType.ToString();//노드의 텍스트
        }

        DrawConnections();

        // 4. 스크롤 위치를 맨 아래(시작점)로 고정
        StartCoroutine(SetScrollBottom());
    }

    private System.Collections.IEnumerator SetScrollBottom()
    {
        yield return new WaitForEndOfFrame();
        ScrollRect scrollRect = GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }

    private void UpdateContentSize(int maxFloor)
    {
        // 도화지 전체 높이 계산
        float totalHeight = (maxFloor + 1) * ySpacing + 300f;
        content.sizeDelta = new Vector2(800, totalHeight);

        // 스크롤 초기 위치 설정
        ScrollRect scrollRect = GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            // Pivot (0.5, 1) 세팅에서는 0f가 최하단(시작점)입니다.
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
    private void DrawConnections()
    {
        foreach (var node in mapData.nodes)
        {
            // 현재 노드의 UI 위치
            Vector2 startPos = nodePositions[node.blueprintPos];

            // 연결된 다음 노드들을 순회하며 선 그리기
            foreach (Vector2Int nextCoord in node.nextNodes)
            {
                if (nodePositions.ContainsKey(nextCoord))
                {
                    Vector2 endPos = nodePositions[nextCoord];
                    DrawLine(startPos, endPos);
                }
            }
        }
    }

    private void DrawLine(Vector2 start, Vector2 end)
    {
        GameObject lineObj = Instantiate(linePrefab, content);
        lineObj.transform.SetAsFirstSibling(); // 선을 노드 뒤로 보냄

        RectTransform rect = lineObj.GetComponent<RectTransform>();
        Vector2 dir = end - start;
        float distance = Vector2.Distance(start, end);

        rect.sizeDelta = new Vector2(distance, 5f); // 선 두께 5
        rect.anchoredPosition = start + dir * 0.5f; // 중심점 맞추기

        // 방향에 맞춰 회전 (Atan2 사용)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rect.localRotation = Quaternion.Euler(0, 0, angle);
    }
}