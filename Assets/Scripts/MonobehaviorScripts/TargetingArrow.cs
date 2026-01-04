using System.Net;
using UnityEngine;

public class TargetingArrow : MonoBehaviour
{
    public static TargetingArrow Instance;

    public LineRenderer lineRenderer;
    public int pointsCount = 30;
    public float curveHeightFactor = 0.25f; // 너무 높으면 포물선이 과해지니 0.2~0.3 권장
    public RectTransform arrowHead; // 화살촉 이미지의 RectTransform

    void Awake()
    {
        Instance = this; // 내가 태어나자마자 내 주소를 공개함
    }

    public void DrawCurve(Vector3 start, Vector3 end)
    {
        if (lineRenderer == null) return;

        lineRenderer.useWorldSpace = false;
        Vector3[] points = new Vector3[pointsCount];

        // 1. 방향 및 수직 벡터 계산
        Vector2 direction = end - start;
        // 기본 수직 벡터 (진행 방향의 왼쪽 방향)
        Vector2 perpendicular = new Vector2(-direction.y, direction.x).normalized;
        float distance = direction.magnitude;

        // 2. [핵심] 대칭 로직 추가
        // 마우스(end)가 시작점(start)보다 왼쪽에 있다면 수직 벡터를 반전시킴
        if (end.x < start.x)
        {
            perpendicular *= -1f;
        }

        // 3. 제어점(Control Point) 설정
        Vector3 controlPoint = (start + end) / 2f + (Vector3)(perpendicular * distance * curveHeightFactor);

        for (int i = 0; i < pointsCount; i++)
        {
            float t = i / (float)(pointsCount - 1);
            points[i] = CalculateBezierPoint(t, start, controlPoint, end);
            points[i].z = -1f;
        }

        lineRenderer.positionCount = pointsCount;
        lineRenderer.SetPositions(points);
        
        // 2. 화살촉 위치 설정
        arrowHead.localPosition = end;

        // 3. 화살촉 회전 설정 (핵심)
        // 끝점(P2)에서 조절점(P1)을 뺀 방향이 화살촉이 바라볼 방향입니다.
        Vector2 pointDirection = end - controlPoint;

        // Atan2를 사용하여 각도를 구합니다 (라디안 -> 도 변환)
        float angle = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;

        // 화살촉 이미지(삼각형)의 기본 방향이 위쪽(Up)을 향하고 있다면 -90도를 해줍니다.
        // 만약 이미지 자체가 오른쪽을 향하고 있다면 그냥 angle을 사용하세요.
        arrowHead.localRotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }
}