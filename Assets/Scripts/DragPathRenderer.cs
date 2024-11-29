using UnityEngine;

public class DragPathRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 startPosition;
    private bool isDragging = false;

    // 베지어 곡선의 제어점
    private Vector3 controlPointOffset = new Vector3(0, 200, 0);

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 20;  // 곡선의 세밀함을 결정
        lineRenderer.enabled = false; // 초기에는 비활성화
    }

    public void StartDrag(Vector3 start)
    {
        startPosition = start;
        isDragging = true;
        lineRenderer.enabled = true;  // 라인 렌더러 활성화
        UpdateCurve(start, start);    // 드래그 시작 시 초기 곡선 설정
    }

    public void UpdateDrag(Vector3 currentPosition)
    {
        if (!isDragging) return;

        UpdateCurve(startPosition, currentPosition);  // 현재 마우스 위치로 곡선 업데이트
    }

    public void EndDrag()
    {
        isDragging = false;
        lineRenderer.enabled = false; // 라인 렌더러 비활성화
    }

    private void UpdateCurve(Vector3 start, Vector3 end)
    {
        // 제어점의 위치는 시작점과 끝점의 중간 지점에 Y 방향으로 offset
        Vector3 controlPoint = (start + end) / 2 + controlPointOffset;

        // LineRenderer의 각 점을 베지어 곡선의 점으로 설정
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float t = i / (float)(lineRenderer.positionCount - 1);
            Vector3 curvePosition = CalculateQuadraticBezierPoint(t, start, controlPoint, end);
            lineRenderer.SetPosition(i, curvePosition);
        }
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // 베지어 곡선 공식 적용
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = uu * p0;      // (1-t)^2 * P0
        point += 2 * u * t * p1;      // 2(1-t)t * P1
        point += tt * p2;             // t^2 * P2

        return point;
    }
}
