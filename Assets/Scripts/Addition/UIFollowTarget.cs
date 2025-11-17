using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
    Description: Lets a UI element follow soldier when moving
*/
public class UIFollowTarget : MonoBehaviour
{
    public RectTransform uiElement;
    public Transform target;
    public Canvas canvas;
    public Vector3 offset;
    public Camera worldCamera;

    Camera uiCamera;
    RectTransform canvasRect;
    bool ready;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(InitNextFrame());
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        ready = false;
        if (uiElement) uiElement.gameObject.SetActive(false);
    }

    // Wait one frame, then bind canvas and camera
    System.Collections.IEnumerator InitNextFrame()
    {
        yield return null;
        BindCanvasAndCamera();
        ready = true;
    }

    // Re-bind when a new scene is loaded
    void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        BindCanvasAndCamera();
    }

    void BindCanvasAndCamera()
    {
        // Auto-find parent Canvas if not set
        if (canvas == null) canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvasRect = canvas.transform as RectTransform;

            // For overlay mode, UI camera is not needed
            uiCamera = (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                ? null
                // For other modes, use canvas camera if present, otherwise fallback
                : (canvas.worldCamera != null ? canvas.worldCamera : worldCamera);
        }
        if (worldCamera == null) worldCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (!ready || uiElement == null || canvasRect == null)
            return;

        // If target is gone or disabled, hide UI
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            if (uiElement.gameObject.activeSelf) uiElement.gameObject.SetActive(false);
            return;
        }

        // Make sure we always have a world camera
        if (worldCamera == null) worldCamera = Camera.main;

        Vector3 worldPos = target.position + offset;
        Vector3 screenPos =
            (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                ? (worldCamera != null
                    ? worldCamera.WorldToScreenPoint(worldPos)
                    : new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f))
                : ((uiCamera != null ? uiCamera : worldCamera).WorldToScreenPoint(worldPos));

        // If target is behind the camera, hide the UI
        if (screenPos.z < 0f)
        {
            if (uiElement.gameObject.activeSelf) uiElement.gameObject.SetActive(false);
            return;
        }

        // Convert screen position to local position on the canvas
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPos,
                (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : uiCamera,
                out localPoint))
        {
            // Move UI element and show it
            uiElement.anchoredPosition = localPoint;
            if (!uiElement.gameObject.activeSelf) uiElement.gameObject.SetActive(true);
        }
        else
        {
            // If conversion failed, hide UI
            if (uiElement.gameObject.activeSelf) uiElement.gameObject.SetActive(false);
        }
    }
}
