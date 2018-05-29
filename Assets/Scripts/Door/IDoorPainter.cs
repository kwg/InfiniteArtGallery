using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDoorPainter {

    void PaintDoor<T>(T doorPaint, Portal targetDoor);

    
}
