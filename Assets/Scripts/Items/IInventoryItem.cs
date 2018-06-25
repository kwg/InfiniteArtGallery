using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IInventoryItem
{
    string Name { get; }
    Sprite Image { get; }
    TWEANNGenotype Geno { get; set; }
}
