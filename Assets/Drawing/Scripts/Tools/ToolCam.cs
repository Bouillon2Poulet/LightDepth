using System.Collections.Generic;
using UnityEngine;
public static class ToolCam
{
    private static Vector3 lastClickedPositionToWorldPoint;
    public static bool needToUpdateLastClickedPositionToWorldPoint;
    private static float lastCameraOrthographicSize;

    public static void pan(Vector3 lastClickedPosition, ref Camera camera2D)
    {
        if (needToUpdateLastClickedPositionToWorldPoint) //Update seulement si nouveau clic !
        {
            Debug.Log("!");
            Vector3 lastClickedPositionTemp = lastClickedPosition;
            lastClickedPositionTemp.z = 5f;
            lastClickedPositionToWorldPoint = camera2D.ScreenToWorldPoint(lastClickedPositionTemp);
            needToUpdateLastClickedPositionToWorldPoint = false;
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;
        Vector3 mousePositionToWorldPoint = camera2D.ScreenToWorldPoint(mousePosition);

        Vector3 difference = lastClickedPositionToWorldPoint - mousePositionToWorldPoint;
        Debug.Log(lastClickedPositionToWorldPoint + "/" + mousePositionToWorldPoint);
        // difference;
        camera2D.transform.position += difference;
    }

    public static void zoom(Vector3 lastClickedPosition, ref Camera camera2D)
    {
        if (needToUpdateLastClickedPositionToWorldPoint) //Update seulement si nouveau clic !
        {
            Debug.Log("!");
            Vector3 lastClickedPositionTemp = lastClickedPosition;
            lastClickedPositionTemp.z = 5f;
            lastClickedPositionToWorldPoint = camera2D.ScreenToWorldPoint(lastClickedPositionTemp);
            needToUpdateLastClickedPositionToWorldPoint = false;

            lastCameraOrthographicSize = camera2D.orthographicSize;
        }
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;
        Vector3 mousePositionToWorldPoint = camera2D.ScreenToWorldPoint(mousePosition);

        Vector3 difference = mousePositionToWorldPoint - lastClickedPositionToWorldPoint;
        Debug.Log(difference);

        float newSize = lastCameraOrthographicSize - difference.x;
        if (newSize > 1f && newSize < 6.5f)
        {
            camera2D.orthographicSize = newSize;
        }

        return;
    }
}

