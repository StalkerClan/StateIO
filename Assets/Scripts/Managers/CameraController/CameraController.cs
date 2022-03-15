using System;
using System.Collections;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject desiredPosition;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private float startSize;
    [SerializeField] private float desiredSize;
    [SerializeField] private float smoothTime;
    [SerializeField] private float delayTime;
    [SerializeField] private float stopZoomingTime;

    private Vector3 velocity1 = Vector3.zero;
    private float velocity2 = 0.0f;
    private bool stoppedZoom;

    public GameObject DesiredPosition { get => desiredPosition; set => desiredPosition = value; }
    public float DesiredSize { get => desiredSize; set => desiredSize = value; }
    public bool StoppedZoom { get => stoppedZoom; set => stoppedZoom = value; }

    private void Start()
    {
        startPosition = cam.transform.position;
        startSize = cam.orthographicSize;
        stoppedZoom = false;
    }

    private void Update()
    {
          
    }

    IEnumerator ZoomDelay()
    {
        WaitForSeconds zoomDelay = Utilities.GetWaitForSeconds(delayTime);
        yield return zoomDelay;

        ZoomIn();
    }

    public void ZoomIn()
    {
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, desiredPosition.transform.position - new Vector3(0f, 0f, 5f), ref velocity1, smoothTime);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, desiredSize, ref velocity2, smoothTime);
    }

    public void ZoomOut()
    {
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, startPosition, ref velocity1, smoothTime);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, startSize, ref velocity2, smoothTime);
    }

    IEnumerator StopZooming()
    {
        WaitForSeconds stopZooming = Utilities.GetWaitForSeconds(stopZoomingTime);
        yield return stopZooming;

        stoppedZoom = true;
    }
}
