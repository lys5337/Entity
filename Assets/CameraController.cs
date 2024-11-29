using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 originalPosition;
    private float originalSize;

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

    }

    public void ZoomInTo(Vector3 targetPosition, float zoomSize, float duration)
    {

        if (zoomInCoroutine != null)
        {
            StopCoroutine(zoomInCoroutine);
        }
        zoomInCoroutine = StartCoroutine(ZoomInCoroutine(targetPosition, zoomSize, duration));
    }

    public void ZoomOut(float duration)
    {
        if (zoomOutCoroutine != null)
        {
            StopCoroutine(zoomOutCoroutine);
        }
        zoomOutCoroutine = StartCoroutine(ZoomOutCoroutine(duration));
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.position = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null; // 한 프레임 대기
        }

        mainCamera.transform.position = originalPosition;
    }

    private IEnumerator ZoomInCoroutine(Vector3 targetPosition, float targetSize, float duration)
    {
        Vector3 startPosition = mainCamera.transform.position;
        float startSize = mainCamera.orthographicSize;
        float time = 0;


        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetSize;

    }

    private IEnumerator ZoomOutCoroutine(float duration)
    {
        Vector3 startPosition = mainCamera.transform.position;
        float startSize = mainCamera.orthographicSize;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            mainCamera.transform.position = Vector3.Lerp(startPosition, originalPosition, t);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, originalSize, t);

            yield return null;
        }

        mainCamera.transform.position = originalPosition;
        mainCamera.orthographicSize = originalSize;

    }

    public void RotateCamera(float duration, float angle)
    {

        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
        }
        rotateCoroutine = StartCoroutine(RotateCoroutine(duration, angle));
    }

    private IEnumerator RotateCoroutine(float duration, float angle)
    {
        Quaternion startRotation = mainCamera.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        float elapsed = 0.0f;


        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            yield return null;
        }

        mainCamera.transform.rotation = startRotation; 

    }
}
