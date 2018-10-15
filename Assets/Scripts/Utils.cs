using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static Transform findChild(Transform parent, TAGS tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (Tags.Compare(parent.GetChild(i), tag))
            {
                return parent.GetChild(i);
            }
        }
        return parent;
    }

    public static Transform[] findChildren(Transform parent, TAGS tag)
    {
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < parent.childCount; i++)
        {
            if (Tags.Compare(parent.GetChild(i), tag))
            {
                children.Add(parent.GetChild(i));
            }
        }
        return children.ToArray();
    }

    public static int Closest(Transform[] input, Transform relative)
    {
        float distance = Mathf.Infinity;
        int index = -1;
        for (int i = 0; i < input.Length; i++)
        {
            float d = Vector3.Distance(relative.position, input[i].position);
            if (d < distance)
            {
                distance = d;
                index = i;
            }
        }
        return index;
    }

    public static Crystal CrystalInRange(Vector3 location)
    {
        GameObject map = MapController.Map;
        Transform crystalHolder = Utils.findChild(map.transform, TAGS.Crystals);
        if (crystalHolder != map.transform)
        {
            Transform[] crystals = Utils.findChildren(crystalHolder, TAGS.Crystal);
            for (int i = 0; i < crystals.Length; i++)
            {
                if (Vector3.Distance(location, crystals[i].position) <= 2)
                {
                    return crystals[i].GetComponent<Crystal>();
                }
            }
        }
        return null;
    }

    public static Transform[] ClosestOrdered(Transform[] input, Vector3 point)
    {
        List<Transform> ordered = new List<Transform>();
        List<Transform> remaining = new List<Transform>(input);
        while (remaining.Count > 0)
        {
            float distance = Mathf.Infinity;
            int index = 0;
            for (int i = 0; i < remaining.Count; i++)
            {
                float d = Vector3.Distance(point, input[i].position);
                if (d < distance)
                {
                    distance = d;
                    index = i;
                }
            }
            ordered.Add(remaining[index]);
            remaining.RemoveAt(index);
        }
        return ordered.ToArray();
    }
}
