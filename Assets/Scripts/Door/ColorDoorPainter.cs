using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDoorPainter<Color> : IDoorPainter
{
    public void PaintDoor<Color>(Color doorPaint, Portal targetDoor)
    {
        targetDoor.PaintDoor(doorPaint);
    }
}
