using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDoorPainter : IDoorPainter<Color>
{


    public void PaintDoor(Color doorPaint, Portal targetDoor)
    {
        // TODO paint the door with the specified color
        targetDoor.SetColor(doorPaint);
    }
}
