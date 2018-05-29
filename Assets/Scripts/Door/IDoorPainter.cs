using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDoorPainter<T> {

    void PaintDoor(T doorPaint, Portal targetDoor);

    
}
