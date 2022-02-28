using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectUtility : MonoBehaviour
{
    [SerializeField]
    private static float _wantedAspectRatio = 0.5625f;

    public bool active = true;
    public bool landscapeModeOnly = true;
    public static bool _active = true;
    public static bool _landscapeModeOnly = true;
    public static bool isLongScreen = false;
    private static Camera cam;

    public static bool isWideScreen = false;

    private static float maxRatio = 9 / 18f, minRatio = 3 / 4f;
    private static float defaultRatio = 9 / 16f;

    private void Awake()
    {
        _landscapeModeOnly = landscapeModeOnly;
        _active = active;
        cam = GetComponent<Camera>();
        if (!cam)
        {
            cam = Camera.main;
            Debug.Log("Setting the main camera " + cam.name);
        }
        else
        {
            Debug.Log("Setting the main camera " + cam.name);
        }

        if (!cam)
        {
            Debug.LogError("No camera available");
            return;
        }
        // Doan này trc khi up nho sua lai
        _wantedAspectRatio = 0.5625f;// 0.5f;
        SetCamera();
    }

    private static void SetCamera()
    {
        float notchCutOffPercent = PlayerPrefs.GetFloat("notchcutoff", 1);
        float wantedAspectRatio = _wantedAspectRatio;
        float currentAspectRatio = 0.0f;

        if (Screen.orientation == ScreenOrientation.LandscapeRight ||
            Screen.orientation == ScreenOrientation.LandscapeLeft)
        {
            Debug.Log("Landscape detected...");
            currentAspectRatio = (float)Screen.width / Screen.height;
        }
        else
        {
            Debug.Log("Portrait detected...?");
            if (Screen.height > Screen.width && _landscapeModeOnly)
            {
                currentAspectRatio = (float)Screen.height / Screen.width;
            }
            else
            {
                currentAspectRatio = (float)Screen.width / Screen.height;
            }
        }
        // If the current aspect ratio is already approximately equal to the desired aspect ratio,
        // use a full-screen Rect (in case it was set to something else previously)

        //Debug.Log("currentAspectRatio = " + currentAspectRatio + ", wantedAspectRatio = " + wantedAspectRatio);

        isWideScreen = currentAspectRatio >= wantedAspectRatio;

        if (!_active)
            return;

        if ((int)(currentAspectRatio * 100) / 100.0f == (int)(wantedAspectRatio * 100) / 100.0f) // Tỷ lệ = tỉ lệ gốc
        {
            cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            //if (backgroundCam)
            //{
            //    Destroy(backgroundCam.gameObject);
            //}
            return;
        }

        if (currentAspectRatio < defaultRatio) // Màn dài hơn 9:16, chỉnh lại size camera vì màn dài bị bóp bề ngang
        {
            cam.orthographicSize = 6.4f * (defaultRatio / currentAspectRatio);
            isLongScreen = true;
        }

        // Doạn này trc khi up nhớ comment
        if (currentAspectRatio >= maxRatio) // Màn ngắn hơn hoặc bằng màn max
        {
            if (currentAspectRatio <= minRatio) // Màn dài hơn màn min
            {
                // Không letter box
                cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
                return;
            }
            else
            {
                wantedAspectRatio = minRatio;
            }
        }
        else
        {
            wantedAspectRatio = maxRatio;
        }

        // Pillarbox
        if (currentAspectRatio > wantedAspectRatio) // Cắt 2 bên
        {
            float inset = 1.0f - wantedAspectRatio / currentAspectRatio;
            cam.rect = new Rect(inset / 2, 0.0f, 1.0f - inset, 1.0f);
        }
        // Letterbox
        else // Cắt ngang
        {
            float inset = 1.0f - currentAspectRatio / wantedAspectRatio;
            Rect rect = new Rect(0.0f, inset / 2, 1.0f, 1.0f - inset);
            //float newHeight = rect.height - rect.height * (1 / 12f) * notchCutOffPercent;
            //cam.orthographicSize *= (newHeight / rect.height);

            //rect.height = newHeight;
            cam.rect = rect;
        }
        //if (!backgroundCam)
        //{
        //    // Make a new camera behind the normal camera which displays black; otherwise the unused space is undefined
        //    backgroundCam = new GameObject("BackgroundCam", typeof(Camera)).GetComponent<Camera>();
        //    backgroundCam.depth = int.MinValue;
        //    backgroundCam.clearFlags = CameraClearFlags.SolidColor;
        //    backgroundCam.backgroundColor = Color.black;
        //    backgroundCam.cullingMask = 0;
        //}
    }

    public static void ResetCamera(float percent)
    {
        PlayerPrefs.SetFloat("notchcutoff", percent);
        SetCamera();
    }

    public static int screenHeight
    {
        get
        {
            return (int)(Screen.height * cam.rect.height);
        }
    }

    public static int screenWidth
    {
        get
        {
            return (int)(Screen.width * cam.rect.width);
        }
    }

    public static int xOffset
    {
        get
        {
            return (int)(Screen.width * cam.rect.x);
        }
    }

    public static int yOffset
    {
        get
        {
            return (int)(Screen.height * cam.rect.y);
        }
    }

    public static Rect screenRect
    {
        get
        {
            return new Rect(cam.rect.x * Screen.width, cam.rect.y * Screen.height, cam.rect.width * Screen.width, cam.rect.height * Screen.height);
        }
    }

    public static Vector3 mousePosition
    {
        get
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.y -= (int)(cam.rect.y * Screen.height);
            mousePos.x -= (int)(cam.rect.x * Screen.width);
            return mousePos;
        }
    }

    public static Vector2 guiMousePosition
    {
        get
        {
            Vector2 mousePos = Event.current.mousePosition;
            mousePos.y = Mathf.Clamp(mousePos.y, cam.rect.y * Screen.height, cam.rect.y * Screen.height + cam.rect.height * Screen.height);
            mousePos.x = Mathf.Clamp(mousePos.x, cam.rect.x * Screen.width, cam.rect.x * Screen.width + cam.rect.width * Screen.width);
            return mousePos;
        }
    }
}
