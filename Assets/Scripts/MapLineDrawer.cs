using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class MapLineDrawer : MonoBehaviour
    {
        // 각 Floor의 노드들의 위치를 저장
        public List<Floor> floors;
        public Color lineColor = Color.white; // 선의 색상 설정
        public float lineWidth = 0.2f; // 선의 두께 설정
        private List<LineRenderer> lineRenderers = new List<LineRenderer>();

        private void Start()
        {
            DrawConnections();
        }

        public void DrawConnections()
        {
            // 기존 LineRenderer 제거
            foreach (var line in lineRenderers)
                Destroy(line.gameObject);

            lineRenderers.Clear();

            // 각 Floor 간의 연결을 그립니다.
            for (int i = 0; i < floors.Count - 1; i++)
            {
                var currentFloor = floors[i];
                var nextFloor = floors[i + 1];

                // 현재 층의 각 노드를 다음 층의 노드와 연결
                foreach (var currentNode in currentFloor.nodes)
                {
                    foreach (var nextNode in nextFloor.nodes)
                    {
                        if (currentNode.enabled && nextNode.enabled)
                        {
                            DrawLine(currentNode.transform.position, nextNode.transform.position);
                        }
                    }
                }
            }
        }

        private void DrawLine(Vector3 start, Vector3 end)
        {
            // LineRenderer를 동적으로 생성하여 선을 그립니다.
            GameObject lineObj = new GameObject("Line");
            lineObj.transform.SetParent(this.transform);
            LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = lineColor;
            lineRenderer.endColor = lineColor;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            lineRenderers.Add(lineRenderer);
        }
    }
}