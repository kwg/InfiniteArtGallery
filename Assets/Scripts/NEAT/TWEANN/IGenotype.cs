using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Interface for all genotypes
/// </summary>
/// <typeparam name="T">Phenotype produced by the genotype</typeparam>
public interface IGenotype<T> {

    /// <summary>
    /// Indicate the ID of a parent of this genotype. Can be called multiple times.
    /// </summary>
    /// <param name="id">Genotype ID of a parent of this genotype</param>
    void AddParentID(long id);

    /// <summary>
    /// Get IDs of all parents of this genotype.
    /// </summary>
    /// <returns>List of all parent genotype IDs</returns>
    List<long> GetParentIDs();

    /// <summary>
    /// Make and return a copy of the genotype
    /// </summary>
    /// <returns>Copy of the genotype</returns>
    IGenotype<T> Copy();

    /// <summary>
    /// Mutate the genotype
    /// </summary>
    void Mutate();

    /// <summary>
    /// Cross this genotype with another genotype g to create up to two new
    /// offspring. One of the offspring is returned. Additionally, this genotype
    /// itself may be modified, thus representing another offspring, but this
    /// would only be appropriate if crossover is performed on a copy of the
    /// original genotype.
    /// </summary>
    /// <param name="g">Genotype to crossover with</param>
    /// <returns>One offspring</returns>
    IGenotype<T> Crossover(IGenotype<T> g);

    /// <summary>
    /// Decode the genotype to produce the pheotype
    /// </summary>
    /// <returns>A phenotype of type T</returns>
    T GetPhenotype();

    /// <summary>
    /// Any instance of a genotype has the capacity to create an entirely new
    /// instance. This method is typically used at the begining of evolution to
    /// initialize the population.
    /// </summary>
    /// <returns>New genotype for starting population</returns>
    IGenotype<T> NewInstance();

    /// <summary>
    /// Every genotype should have a unique ID assigned by EvolutionaryHistory
    /// </summary>
    /// <returns>Unique ID number</returns>
    long GetID();


}
