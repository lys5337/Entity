using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;  // 메인 카메라 참조
    private Vector3 originalPosition;  // 카메라의 원래 위치 저장
    private float originalSize;  // 카메라의 원래 크기 저장

    // 개별 코루틴 관리를 위한 변수 추가
    private Coroutine zoomInCoroutine;
    private Coroutine zoomOutCoroutine;
    private Coroutine shakeCoroutine;
    private Coroutine rotateCoroutine;

    private void Start()
    {
        // 메인 카메라 초기화 및 원래 위치와 크기 저장
        mainCamera = Camera.main;
        originalPosition = mainCamera.transform.position;
        originalSize = mainCamera.orthographicSize;

        Debug.Log($"Camera Initialized: Position = {originalPosition}, Size = {originalSize}");
    }

    // 카메라를 특정 위치로 확대하는 함수
    public void ZoomInTo(Vector3 targetPosition, float zoomSize, float duration)
    {
        Debug.Log($"ZoomInTo called: TargetPosition = {targetPosition}, ZoomSize = {zoomSize}, Duration = {duration}");

        // 현재 실행 중인 ZoomInCoroutine이 있으면 중지하고 새로 시작
        if (zoomInCoroutine != null)
        {
            StopCoroutine(zoomInCoroutine);
        }
        zoomInCoroutine = StartCoroutine(ZoomInCoroutine(targetPosition, zoomSize, duration));
    }

    // 카메라를 원래 위치로 되돌리는 함수
    public void ZoomOut(float duration)
    {
        Debug.Log($"ZoomOut called: Duration = {duration}");

        // 현재 실행 중인 ZoomOutCoroutine이 있으면 중지하고 새로 시작
        if (zoomOutCoroutine != null)
        {
            StopCoroutine(zoomOutCoroutine);
        }
        zoomOutCoroutine = StartCoroutine(ZoomOutCoroutine(duration));
    }

    // 카메라 진동 효과를 주는 함수
    public void ShakeCamera(float duration, float magnitude)
    {
        Debug.Log($"ShakeCamera called: Duration = {duration}, Magnitude = {magnitude}");

        // 현재 실행 중인 ShakeCoroutine이 있으면 중지하고 새로 시작
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    // 카메라를 흔드는 코루틴
    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // 카메라의 무작위 위치 설정
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            // 무작위 위치로 이동
            mainCamera.transform.position = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            // 시간 경과
            elapsed += Time.deltaTime;

            // 한 프레임 대기
            yield return null;
        }

        // 진동 후 원래 위치로 복구
        mainCamera.transform.position = originalPosition;
    }

    // 카메라를 줌 인하는 코루틴
    private IEnumerator ZoomInCoroutine(Vector3 targetPosition, float targetSize, float duration)
    {
        Vector3 startPosition = mainCamera.transform.position;
        float startSize = mainCamera.orthographicSize;
        float time = 0;

        Debug.Log($"ZoomInCoroutine started: StartPosition = {startPosition}, TargetPosition = {targetPosition}");

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // 카메라 위치와 크기를 Lerp로 자연스럽게 이동
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        // 최종 위치와 크기 설정
        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetSize;

        Debug.Log($"ZoomInCoroutine completed: Final Position = {mainCamera.transform.position}");
    }

    // 카메라를 원래 위치로 되돌리는 코루틴
    private IEnumerator ZoomOutCoroutine(float duration)
    {
        Vector3 startPosition = mainCamera.transform.position;
        float startSize = mainCamera.orthographicSize;
        float time = 0;

        Debug.Log($"ZoomOutCoroutine started: StartPosition = {startPosition}");

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // 카메라 위치와 크기를 Lerp로 자연스럽게 원래 값으로 복귀
            mainCamera.transform.position = Vector3.Lerp(startPosition, originalPosition, t);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, originalSize, t);

            yield return null;
        }

        // 최종 위치와 크기 설정
        mainCamera.transform.position = originalPosition;
        mainCamera.orthographicSize = originalSize;

        Debug.Log($"ZoomOutCoroutine completed: Final Position = {mainCamera.transform.position}");
    }

    // 카메라 회전 효과를 주는 함수
    public void RotateCamera(float duration, float angle)
    {
        Debug.Log($"RotateCamera called: Duration = {duration}, Angle = {angle}");

        // 현재 실행 중인 RotateCoroutine이 있으면 중지하고 새로 시작
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
        }
        rotateCoroutine = StartCoroutine(RotateCoroutine(duration, angle));
    }

    // 카메라를 회전시키는 코루틴
    private IEnumerator RotateCoroutine(float duration, float angle)
    {
        Quaternion startRotation = mainCamera.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        float elapsed = 0.0f;

        Debug.Log($"RotateCoroutine started: StartRotation = {startRotation.eulerAngles}, TargetRotation = {targetRotation.eulerAngles}");

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Lerp를 사용하여 카메라의 회전 각도를 부드럽게 변경
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            yield return null;
        }

        // 최종 회전값 설정
        mainCamera.transform.rotation = startRotation;  // 회전 후 원래 값으로 복구

        Debug.Log($"RotateCoroutine completed: Final Rotation = {startRotation.eulerAngles}");
    }
}
