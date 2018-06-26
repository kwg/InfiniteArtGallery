using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TWEANNCrossover
{

    private bool includeExcess;

    public TWEANNCrossover(bool includeExcess)
    {
        this.includeExcess = includeExcess;
    }

    public TWEANNGenotype Crossover(TWEANNGenotype toModify, TWEANNGenotype toReturn)
    {

        List<List<NodeGene>> alignedNodes = new List<List<NodeGene>>(2)
        {
            AlignNodesToArchetype(toModify.GetNodes(), toModify.GetArchetypeIndex()),
            AlignNodesToArchetype(toReturn.GetNodes(), toReturn.GetArchetypeIndex())
        };

        List<List<NodeGene>> crossedNodes = CrossNodes(alignedNodes[0], alignedNodes[1]);

        List<List<LinkGene>> alignedLinks = AlignLinkGenes(toModify.GetLinks(), toReturn.GetLinks());


        return null;
    }

    private List<List<LinkGene>> AlignLinkGenes(List<LinkGene> left, List<LinkGene> right)
    {
        MergeDuplicates(left, right);
        SortLinkGenesByInnovationNumber(left);
        SortLinkGenesByInnovationNumber(right);

        int maxSize = Math.Max(left.Count, right.Count);
        List<LinkGene> alignedLeft = new List<LinkGene>(maxSize);
        List<LinkGene> alignedRight = new List<LinkGene>(maxSize);

        int leftPos = 0, rightPos = 0;

        while (leftPos < left.Count && rightPos < right.Count)
        {
            int l = leftPos, r = rightPos;
            long leftInnovation = left[leftPos].Innovation;
            long rightInnovation = right[rightPos].Innovation;
            if (leftInnovation == rightInnovation)
            {
                alignedLeft.Add(left[leftPos++]);
                alignedRight.Add(right[rightPos++]);
            }
            else
            {
                int leftHasRightAt = containsLinkInnovationAt(left, rightInnovation);
                int rightHasLeftAt = containsLinkInnovationAt(right, leftInnovation);


                if (leftHasRightAt == null)
                {
                    // System.out.println("leftHasRight is null");
                    alignedLeft.add(null);
                    alignedRight.add(right.get(rightPos++));
                }
                else if (rightHasLeftAt == null)
                {
                    // System.out.println("rightHasLeftAt is null");
                    alignedLeft.add(left.get(leftPos++));
                    alignedRight.add(null);
                }
            }
            if (l == leftPos && r == rightPos)
            {
                Debug.Log("No progress performing crossover: " + l + "," + r);
                Debug.Log("Left: " + left);
                Debug.Log("Right: " + right);
                Debug.Log("alignedLeft: " + alignedLeft);
                Debug.Log("alignedRight: " + alignedRight);

                return null;
            }


        }

        return null;
    }

    public static void SortLinkGenesByInnovationNumber(List<LinkGene> linkedGene)
    {
        linkedGene.Sort();
    }


    private void MergeDuplicates(List<LinkGene> left, List<LinkGene> right)
    {
        foreach (LinkGene lg in left)
        {
            foreach (LinkGene rg in right)
            {
                if (lg.GetSourceInnovation() == rg.GetSourceInnovation() && lg.GetTargetInnovation() == rg.GetTargetInnovation()
                        && lg.Innovation != rg.Innovation)
                {
                    rg.Innovation = lg.Innovation;
                }
            }
        }
    }

    private List<List<NodeGene>> CrossNodes(List<NodeGene> left, List<NodeGene> right)
    {
        if (left.Count != right.Count) throw new Exception("Cannot cross lists of different sizes!");

        List<NodeGene> crossedLeft = new List<NodeGene>(left.Count);
        List<NodeGene> crossedRight = new List<NodeGene>(right.Count);

        for (int i = 0; i < left.Count; i++)
        {
            NodeGene leftGene = left[i];
            NodeGene rightGene = right[i];

            if (leftGene != null && rightGene != null)
            {
                CrossIndexNodes((NodeGene)leftGene.CopyGene(), (NodeGene)rightGene.CopyGene(), crossedLeft, crossedRight);
            }
            else
            {
                if (leftGene != null)
                {
                    crossedLeft.Add((NodeGene)leftGene.CopyGene());
                    if (includeExcess)
                    {
                        crossedRight.Add((NodeGene)leftGene.CopyGene());
                    }
                }

                if (rightGene != null)
                {
                    crossedRight.Add((NodeGene)rightGene.CopyGene());
                    if (includeExcess)
                    {
                        crossedLeft.Add((NodeGene)rightGene.CopyGene());
                    }
                }
            }
        }

        List<List<NodeGene>> pair = new List<List<NodeGene>>(2)
        {
            crossedLeft,
            crossedRight
        };

        return pair;
    }

    private void CrossIndexNodes(NodeGene nodeGene1, NodeGene nodeGene2, List<NodeGene> crossedLeft, List<NodeGene> crossedRight)
    {
        throw new NotImplementedException();
    }

    private List<NodeGene> AlignNodesToArchetype(List<NodeGene> nodes, int archetypeIndex)
    {
        List<NodeGene> archetype = EvolutionaryHistory.archetypes[archetypeIndex];
        List<NodeGene> aligned = new List<NodeGene>(archetype.Count);

        int listPos = 0, archetypePos = 0;
        while (listPos < nodes.Count && archetypePos < archetype.Count)
        {
            long leftInnovation = nodes[listPos].Innovation;
            long rightInnovation = archetype[archetypePos].Innovation;
            if (leftInnovation == rightInnovation)
            {
                aligned.Add(nodes[listPos++]);
                archetypePos++;
            }
            else
            {
                aligned.Add(null);
                archetypePos++;
            }

        }

        while (archetypePos < archetype.Count)
        {
            aligned.Add(null);
            archetypePos++;
        }

        return aligned;
    }


    private int ContainsLinkInnovationAt(List<LinkGene> left, long rightInnovation)
    {
        throw new NotImplementedException();
    }
}