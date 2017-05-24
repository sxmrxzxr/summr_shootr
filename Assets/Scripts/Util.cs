
using UnityEngine;

public class Util {

    public static void SetLayerRecursively(GameObject obj, int layerIndex)
    {
        if (obj == null)
        {
            return;
        }

        obj.layer = layerIndex;

        foreach (Transform child in obj.transform)
        {
            if (child == null)
                continue;

            SetLayerRecursively(child.gameObject, layerIndex);
        }
    }

}
