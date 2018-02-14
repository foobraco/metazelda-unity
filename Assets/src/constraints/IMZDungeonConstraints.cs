using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Implementing classes may specify constraints to be placed on MZDungeon
 * generation.
 * 
 * @see net.bytten.metazelda.generators.IMZDungeonGenerator
 */
public interface IMZDungeonConstraints {

    /**
     * @return  the maximum number of Rooms an 
     * {@link net.bytten.metazelda.generators.IMZDungeonGenerator} may
     *          place in an {@link net.bytten.metazelda.IMZDungeon}
     */
    public int GetMaxRooms();
    
    /**
     * @return  the maximum number of keys an 
     * {@link net.bytten.metazelda.generators.IMZDungeonGenerator} may
     *          place in an {@link net.bytten.metazelda.IMZDungeon}
     */
    public int GetMaxKeys();

    /**
     * Gets the number of switches the
     * {@link net.bytten.metazelda.generators.IMZDungeonGenerator} is allowed to
     * place in an {@link net.bytten.metazelda.IMZDungeon}.
     * Note only one switch is ever placed due to limitations of the current
     * algorithm.
     * 
     * @return  the maximum number of switches an
     * {@link net.bytten.metazelda.generators.IMZDungeonGenerator} may
     *          place in an {@link net.bytten.metazelda.IMZDungeon}
     */
    public int GetMaxSwitches();
    
    /**
     * Gets the collection of ids from which an
     * {@link net.bytten.metazelda.generators.IMZDungeonGenerator} is allowed to
     * pick the entrance room.
     * 
     * @return the collection of ids
     */
    public Collection<int> initialRooms();
    
    /**
     * @return a weighted list of ids of rooms that are adjacent to the room
     * with the given id.
     */
    public List<Pair<Double,int>> GetAdjacentRooms(int id, int keyLevel);
    
    /**
     * @return desired probability for an extra edge to be Added between the
     * given rooms during the graphify phase.
     */
    public double edgeGraphifyProbability(int id, int nextId);
    
    /**
     * @return a set of Coords which the room with the given id occupies.
     */
    public List<Vector2Int> GetCoords(int id);
    
    /**
     * Runs post-generation checks to determine the suitability of the dungeon.
     * 
     * @param dungeon   the {@link net.bytten.metazelda.IMZDungeon} to check
     * @return  true to keep the dungeon, or false to discard the dungeon and
     *          attempt generation again
     */
    public bool isAcceptable(IMZDungeon dungeon);
    
    public bool roomCanFitItem(int id, MZSymbol key);
    
}
