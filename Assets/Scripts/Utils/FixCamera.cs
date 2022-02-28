using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixCamera : AMonoBehaviour
{
    private void Awake()
    {
#if UNITY_STANDALONE
        Screen.SetResolution(540, 960, false);
        Screen.fullScreen = false;
#endif
        Camera camera = GetComponent<Camera>();
        if (camera.aspect >= 0.68f)
        {
            //Debug.Log(Camera.main.aspect);
            GameObject bg = GameObject.FindGameObjectWithTag("BGImage");
            if (bg != null)
            {
                bg.GetComponent<Image>().preserveAspect = false;
                Vector2 size = bg.GetComponent<RectTransform>().sizeDelta;
                size.x = 720;
                bg.GetComponent<RectTransform>().sizeDelta = size;
            }
            return;
        }

        //float cameraSize = camera.orthographicSize;
        //float defaultAspect = 9 / 16f;
        //camera.orthographicSize = cameraSize * defaultAspect / camera.aspect;
        if (gameObject.CompareTag("MainCamera"))
        {
            //Static.mainCamera = GetComponent<Camera>();
        }
    }
}
