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

        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        List<Action.ActionData> actionDatas = new List<Action.ActionData>();

        stack.Push(position);

        int i = 0;
        while (stack.Count > 0)
        {
            i++;
            if (i > texture.width * texture.height * 2) return; //TODO  : Find a better way to avoid infinite loop ?
            Vector2Int currentPos = stack.Pop();

            if (visited.Contains(currentPos))
            {
                continue; // Skip if already visited
            }

            visited.Add(currentPos);

            Action.ActionData actionData = new Action.ActionData
            {
                position = currentPos,
                colorBeforeAction = texture.GetPixel(currentPos.x, currentPos.y),
                colorAfterAction = currentColor
            };
            actionDatas.Add(actionData);

            texture.SetPixel(currentPos.x, currentPos.y, currentColor);

            // Ajout des voisins à la pile
            AddNeighborToStack(texture, stack, visited, currentPos.x - 1, currentPos.y, targetColor, currentColor); // Gauche
            AddNeighborToStack(texture, stack, visited, currentPos.x + 1, currentPos.y, targetColor, currentColor); // Droite
            AddNeighborToStack(texture, stack, visited, currentPos.x, currentPos.y + 1, targetColor, currentColor); // Haut
            AddNeighborToStack(texture, stack, visited, currentPos.x, currentPos.y - 1, targetColor, currentColor); // Bas
        }
        action.addActionDatas(actionDatas);
    }

    private static void AddNeighborToStack(Texture2D texture, Stack<Vector2Int> stack, HashSet<Vector2Int> visited, int x, int y, Color targetColor, Color currentColor)
    {
        if (x >= 0 && x < texture.width && y >= 0 && y < texture.height &&
            texture.GetPixel(x, y) == targetColor && texture.GetPixel(x, y) != currentColor)
        {
            stack.Push(new Vector2Int(x, y));
        }
    }
}
