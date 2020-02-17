using UnityEngine;

class ColorSpaceStandardRGB : IColorChange
{
    static int HUE_INDEX = 0;
    static int SATURATION_INDEX = 1;
    static int BRIGHTNESS_INDEX = 2;

    public Color32[] AdjustColor(float[][] _cppnOutput)
    {
        Color32[] output = new Color32[_cppnOutput.Length];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = new Color(
                _cppnOutput[Mathf.FloorToInt(i)][HUE_INDEX],
                ActivationFunctions.Activation(FTYPE.HLPIECEWISE, _cppnOutput[Mathf.FloorToInt(i)][SATURATION_INDEX]),
                Mathf.Abs(ActivationFunctions.Activation(FTYPE.PIECEWISE, _cppnOutput[Mathf.FloorToInt(i)][BRIGHTNESS_INDEX]))
                );


        }

        return output;
    }
}