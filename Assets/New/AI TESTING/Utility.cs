using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour {
    // have a list of curves for various utility inputs
    // Some Eval function that takes inputs that 1 to 1 match curve count
    // returns the index of the curve that returned the highest value based on inputs
    public AnimationCurve[] utilities;

    void Start()
    {
        Debug.Log("Evaluated " + Evaluate(new float[] { 0.5f , 0.5f  }) + " for [.5, .5] input!");
        Debug.Log("Evaluated " + Evaluate(new float[] { 0.75f, 0.75f }) + " for [.75, .75] input!");
    }

    public int Evaluate(float[] inputs)
    {
        if (inputs.Length != utilities.Length)
        {
            Debug.LogError("Evaluate--Unmatched inputs to curves!");
            return -1;
        }
        float maxValue = -1;
        int best = -1;
        for (int i = 0; i < utilities.Length; i++)
        {
            float value = utilities[i].Evaluate(inputs[i]);
            if (value > maxValue)
            {
                maxValue = value;
                best = i;
            }
        }
        return best;
    }
}
