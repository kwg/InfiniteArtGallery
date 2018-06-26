using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionaryHistory {

    public static long largestUnusedInnovationID = 0;
    public static long largestUnusedGenotypeID = 0;
    public static List<NodeGene>[] archetypes = null;
    public static int[] archetypeOut = null;

    // TODO - hook to logging utils



    public static long NextInnovationID()
    {
        return largestUnusedInnovationID++;
    }

    public static long NextGenotypeID()
    {
        return largestUnusedGenotypeID++;
    }

    public static void AddArchetype(int populationIndex, NodeGene node, string origin)
    {
        if(archetypes != null && archetypes[populationIndex] != null 
            && IndexOfArchetypeInnovation(populationIndex, node.Innovation) != -1)
        {
            archetypes[populationIndex].Add(node);
            if (node.nTYPE == NTYPE.OUTPUT)
            {
                archetypeOut[populationIndex]++;
            }
        }
    }

    public static void AddArchetype(int populationIndex, int pos, NodeGene node, string origin)
    {
        if (archetypes != null && archetypes[populationIndex] != null)
        {
            // TODO sanity checks - .Insert() is brutal about index out of bounds problems
            archetypes[populationIndex].Insert(pos, node);
        }
    }


    public static int IndexOfArchetypeInnovation(int populationIndex, long sourceInnovation)
    {
        int result = -1;
        if(archetypes[populationIndex] != null)
        {
            for(int i = 0; i < archetypes[populationIndex].Count; i++)
            {
                if(archetypes[populationIndex][i].Innovation == sourceInnovation)
                {
                    result = i;
                    break;
                }
            }
        }
        return result;
    }


    public static int FirstArchetypeOutputIndex(int archetypeIndex)
    {
        int result = ArchetypeSize(archetypeIndex) - archetypeOut[archetypeIndex];
        // TODO sanity checks
        return result;
    }

    public static int ArchetypeSize(int populationIndex)
    {
        return archetypes[populationIndex] == null ? 0 : archetypes[populationIndex].Count;
    }

}
