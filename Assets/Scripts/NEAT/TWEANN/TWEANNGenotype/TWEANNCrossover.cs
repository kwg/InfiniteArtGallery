using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TWEANNCrossover
{

    private bool includeExcess;
    public bool Sucessful { get; set; }

    public TWEANNCrossover(bool includeExcess)
    {
        this.includeExcess = includeExcess;
    }

    public TWEANNGenotype Crossover(TWEANNGenotype toModify, TWEANNGenotype toReturn)
    {

        List<List<NodeGene>> alignedNodes = new List<List<NodeGene>>(2)
        {
            AlignNodesToArchetype(toModify.Nodes, toModify.GetArchetypeIndex()),
            AlignNodesToArchetype(toReturn.Nodes, toReturn.GetArchetypeIndex())
        };

        List<List<NodeGene>> crossedNodes = CrossNodes(alignedNodes[0], alignedNodes[1]);

        List<List<LinkGene>> alignedLinks = AlignLinkGenes(toModify.Links, toReturn.Links);
        List<List<LinkGene>> crossedLinks = CrossLinks(alignedLinks[0], alignedLinks[1]);

        toModify.Nodes = crossedNodes[0];
        toModify.Links = crossedLinks[0];
        toReturn.Nodes = crossedNodes[1];
        toReturn.Links = crossedLinks[1];

        Sucessful = true;
        Debug.Log("Crossover completed");
        return toReturn;
    }

    private List<List<LinkGene>> CrossLinks(List<LinkGene> left, List<LinkGene> right)
    {
        if (left.Count != right.Count) throw new Exception("Cannot cross lists of different sizes!");

        List<LinkGene> crossedLeft = new List<LinkGene>(left.Count);
        List<LinkGene> crossedRight = new List<LinkGene>(right.Count);

        for (int i = 0; i < left.Count; i++)
        {
            LinkGene leftGene = left[i];
            LinkGene rightGene = right[i];

            if (leftGene != null && rightGene != null)
            {
                CrossIndexLinks((LinkGene)leftGene.CopyGene(), (LinkGene)rightGene.CopyGene(), crossedLeft, crossedRight);
            }
            else
            {
                if (leftGene != null)
                {
                    crossedLeft.Add((LinkGene)leftGene.CopyGene());
                    if (includeExcess)
                    {
                        crossedRight.Add((LinkGene)leftGene.CopyGene());
                    }
                }

                if (rightGene != null)
                {
                    crossedRight.Add((LinkGene)rightGene.CopyGene());
                    if (includeExcess)
                    {
                        crossedLeft.Add((LinkGene)rightGene.CopyGene());
                    }
                }
            }
        }

        List<List<LinkGene>> pair = new List<List<LinkGene>>(2)
        {
            crossedLeft,
            crossedRight
        };

        return pair;
    }

    private void CrossIndexLinks(LinkGene leftGene, LinkGene rightGene, List<LinkGene> crossedLeft, List<LinkGene> crossedRight)
    {
        bool swap = RandomGenerator.NextBool(); ;
        if (swap)
        {
            Pair<LinkGene, LinkGene> p = SwapLinks(leftGene, rightGene);
            leftGene = p.t1;
            rightGene = p.t2;
        }
        crossedLeft.Add(leftGene);
        crossedRight.Add(rightGene);
    }

    private Pair<LinkGene, LinkGene> SwapLinks(LinkGene left, LinkGene right)
    {
        return new Pair<LinkGene, LinkGene>(right, left);
    }

    private Pair<NodeGene, NodeGene> SwapNodes(NodeGene left, NodeGene right)
    {
        return new Pair<NodeGene, NodeGene>(right, left);
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
                int? leftHasRightAt = ContainsLinkInnovationAt(left, rightInnovation);
                int? rightHasLeftAt = ContainsLinkInnovationAt(right, leftInnovation);


                if (leftHasRightAt == null)
                {
                    alignedLeft.Add(null);
                    alignedRight.Add(right[rightPos++]);
                }
                else if (rightHasLeftAt == null)
                {
                    alignedLeft.Add(left[leftPos++]);
                    alignedRight.Add(null);
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

        while (leftPos < left.Count)
        {
            alignedLeft.Add(left[leftPos++]);
            alignedRight.Add(null);
        }

        while (rightPos < right.Count)
        {
            alignedLeft.Add(null);
            alignedRight.Add(right[rightPos++]);
        }

        List<List<LinkGene>> pair = new List<List<LinkGene>>(2);
        pair.Add(alignedLeft);
        pair.Add(alignedRight);

        return pair;
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

    private void CrossIndexNodes(NodeGene leftGene, NodeGene rightGene, List<NodeGene> crossedLeft, List<NodeGene> crossedRight)
    {
        bool swap = RandomGenerator.NextBool(); ;
        if (swap)
        {
            Pair<NodeGene, NodeGene> p = SwapNodes(leftGene, rightGene);
            leftGene = p.t1;
            rightGene = p.t2;
        }
        crossedLeft.Add(leftGene);
        crossedRight.Add(rightGene);

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


    private int? ContainsLinkInnovationAt(List<LinkGene> genes, long innovation)
    {
        int? result = null;
        for (int i = 0; i < genes.Count; i++)
        {
            if (genes[i].Innovation == innovation)
            {
                result = i;
                break;
            }
        }
        return result;
    }


}