using System;
using System.Collections.Generic;
using UnityEngine;

public static class ToolBucket
{
    public enum CaseType
    {
        zero,
        left,
        right,
        top,
        bot
    }

    public static void fill(Texture2D texture, Vector2Int position, Action action, Color targetColor, Color currentColor, CaseType caseWhoCalledFunction)
    {
        if (position.x < 0 || position.x >= texture.width ||
            position.y < 0 || position.y >= texture.height)
        {
            // Arrêt si nous sommes en dehors des limites de la texture.
            return;
        }

        if (texture.GetPixel(position.x, position.y) != targetColor || texture.GetPixel(position.x, position.y) == currentColor)
        {
            // Arrêt si la couleur du pixel ne correspond pas à la couleur à remplir ou si le pixel a déjà la couleur actuelle.
            return;
        }

        List<Action.ActionData> actionDatas = new List<Action.ActionData>();
        Action.ActionData actionData = new Action.ActionData();
        actionData.position = position;
        actionData.colorBeforeAction = texture.GetPixel(position.x, position.y);
        actionData.colorAfterAction = currentColor;
        actionDatas.Add(actionData);
        action.addActionDatas(actionDatas);

        texture.SetPixel(position.x, position.y, currentColor);

        // Appels récursifs pour les voisins
        FillNeighbor(texture, new Vector2Int(position.x - 1, position.y), action, targetColor, currentColor, CaseType.left); // Gauche
        FillNeighbor(texture, new Vector2Int(position.x + 1, position.y), action, targetColor, currentColor, CaseType.right); // Droite
        FillNeighbor(texture, new Vector2Int(position.x, position.y + 1), action, targetColor, currentColor, CaseType.bot); // Haut
        FillNeighbor(texture, new Vector2Int(position.x, position.y - 1), action, targetColor, currentColor, CaseType.top); // Bas
    }

    private static void FillNeighbor(Texture2D texture, Vector2Int position, Action action, Color targetColor, Color currentColor, CaseType caseWhoCalledFunction)
    {
        fill(texture, position, action, targetColor, currentColor, caseWhoCalledFunction);
    }
}
