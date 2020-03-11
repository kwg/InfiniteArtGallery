using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionCollection
{
    public int Count { get; private set; }

    private FTYPE[] useableFunctions;
    private int[] numFunctions;

    public FunctionCollection()
    {
        Count = 0;
        useableFunctions = new FTYPE[10];
        numFunctions = new int[10];

    }

    public bool Contains(FTYPE ftype)
    {
        bool result = false;
        foreach (FTYPE f in useableFunctions)
        {
            if (f == ftype) result = true;
        }
        return result;
    }

    public List<FTYPE> GetFunctionList()
    {
        List<FTYPE> result = new List<FTYPE>();
        foreach(FTYPE f in useableFunctions)
        {
            result.Add(f);
        }

        return result;
    }

    private int IndexOf(FTYPE ftype)
    {
        int index = -1;
        for(int f = 0; f < useableFunctions.Length; f++)
        {
            if (useableFunctions[f] == ftype) index = f;
        }
        return index;
    }

    public int NumOfFunctions(FTYPE ftype)
    {
        return numFunctions[IndexOf(ftype)];
    }

    public bool AddFunction(FTYPE ftype)
    {
        bool success = false;
        if (Contains(ftype))
        {
            numFunctions[IndexOf(ftype)]++;
            success = true;
        }
        else
        {
            if (useableFunctions.Length == Count)
            {
                DoubleArraySize();
            }
            useableFunctions[Count] = ftype;
            numFunctions[Count]++;
            Count++;
            success = true;
        }
        return success;
    }

    public bool AddFunction(List<FTYPE> ftypes)
    {
        bool success = true;
        foreach (FTYPE f in ftypes)
        {
            if (!AddFunction(f)) success = false;
        }
        return success;
    }

    public bool RemoveFunction(FTYPE ftype)
    {
        bool success = false;
        if (Contains(ftype) && numFunctions[IndexOf(ftype)] > 0)
        {
            numFunctions[IndexOf(ftype)]--;
            success = true;
        }

        return success;
    }

    private void DoubleArraySize()
    {
        FTYPE[] tempUseableFunctions = new FTYPE[Count * 2];
        int[] tempNumFunctions = new int[Count * 2];
        for (int i = 0; i < Count; i++)
        {
            tempUseableFunctions[i] = useableFunctions[i];
            tempNumFunctions[i] = numFunctions[i];
        }
        useableFunctions = tempUseableFunctions;
        numFunctions = tempNumFunctions;
    }

    public FTYPE GetRandom()
    {
        return useableFunctions[Random.Range(0, Count)];
    }

    public FTYPE GetWeightedRandom()
    {
        FTYPE result = FTYPE.ID;
        int random = Random.Range(0, GetTotalNumberOfCollectedFunctions());
        int totalCollected = 0;
        for (int nf = 0; nf < numFunctions.Length; nf++)
        {
            totalCollected += nf;
            if (totalCollected >= random)
            {
                result = useableFunctions[nf];
            }
        }
        return result;
    }

    private int GetTotalNumberOfCollectedFunctions()
    {
        int totalCollected = 0;
        foreach (int nf in numFunctions)
        {
            totalCollected += nf;
        }
        return totalCollected;
    }

}