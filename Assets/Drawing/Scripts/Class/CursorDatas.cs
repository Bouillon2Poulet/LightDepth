using System.Collections.Generic;
using UnityEngine;

public class CursorDatas
{
    public Vector2Int currentPixelPosition;
    public Vector2Int lastPixelPosition;
    public List<Vector2Int> overedPositionsBetweenTwoFrames;
    public bool needForANewClick = true;

    public CursorDatas()
    {
        overedPositionsBetweenTwoFrames = new List<Vector2Int>();
    }

    public void update(Vector2Int pixelPosition)
    {
        lastPixelPosition = currentPixelPosition;
        currentPixelPosition = pixelPosition;
        updateOveredPositionsBetweenTwoFrames();
    }

    public void updateOveredPositionsBetweenTwoFrames()
    {
        overedPositionsBetweenTwoFrames.Clear();

        Vector2 currentPixelPositionTemp = new Vector2(currentPixelPosition.x, currentPixelPosition.y);
        Vector2 lastPixelPositionTemp = new Vector2(lastPixelPosition.x, lastPixelPosition.y);
        Vector2 drawVector = currentPixelPositionTemp - lastPixelPositionTemp;

        float drawVectorLength = drawVector.magnitude;
        Vector2 drawVectorNormalized = drawVector.normalized;
        overedPositionsBetweenTwoFrames.Add(lastPixelPosition);

        if (Mathf.Abs(currentPixelPosition.x - lastPixelPosition.x) > 1 || Mathf.Abs(currentPixelPosition.y - lastPixelPosition.y) > 1)
        {
            for (int i = 0; i < drawVectorLength; i++)
            {
                overedPositionsBetweenTwoFrames.Add(new Vector2Int((int)(lastPixelPosition.x + i * drawVectorNormalized.x), (int)(lastPixelPosition.y + i * drawVectorNormalized.y)));
            }
        }
        overedPositionsBetweenTwoFrames.Add(currentPixelPosition);
    }
}
