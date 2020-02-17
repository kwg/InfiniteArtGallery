using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IFunctionItem
{
    string Name { get; }
    Sprite Image { get; }
    FTYPE fTYPE { get; set; }
    int Count { get; set; }
}
