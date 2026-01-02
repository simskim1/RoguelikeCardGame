using UnityEngine;

public class TargetingArrow : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int pointsCount = 30;
    public float curveHeightFactor = 0.25f; // 너무 높으면 포물선이 과해지니 0.2~0.3 권장

    /*public void DrawCurve(Vector3 start, Vector3 end)
    {
        if (lineRenderer == null) return;

        // 반드시 월드 스페이스 해제 (Canvas 내 로컬 좌표 사용)
        lineRenderer.useWorldSpace = false;

        Vector3[] points = new Vector3[pointsCount];

        // 1. 방향 및 수직 벡터 계산 (진행 방향의 '옆'으로 휘게 함)
        Vector2 direction = end - start;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x).normalized;
        float distance = direction.magnitude;

        // 2. 제어점(Control Point) 설정: 시작과 끝의 중간에서 수직 방향으로 높이 부여
        // distance에 비례하게 높이를 설정하여 너무 멀리 날아가지 않게 함
        Vector3 controlPoint = (start + end) / 2f + (Vector3)(perpendicular * distance * curveHeightFactor);

        for (int i = 0; i < pointsCount; i++)
        {
            float t = i / (float)(pointsCount - 1);
            points[i] = CalculateBezierPoint(t, start, controlPoint, end);
            points[i].z = -1f; // 카드(Z=0)보다 약간 앞으로 나오게 설정
        }

        lineRenderer.positionCount = pointsCount;
        lineRenderer.SetPositions(points);
    }*/
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
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }
}