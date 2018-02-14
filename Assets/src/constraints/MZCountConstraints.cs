using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Limits the {@link net.bytten.metazelda.generators.IMZDungeonGenerator} in
 * the <i>number</i> of keys, switches and rooms it is allowed to place.
 *
 * Also restrict to a grid of 1x1 rooms.
 *
 * @see IMZDungeonConstraints
 */
public class CountConstraints implements IMZDungeonConstraints {

    protected int maxSpaces, maxKeys, maxSwitches;
    
    protected MZIntMap<Vector2Int> gridCoords;
    protected Vector2IntMap<int> roomIds;
    protected int firstRoomId;
    
    public CountConstraints(int maxSpaces, int maxKeys, int maxSwitches) {
        this.maxSpaces = maxSpaces;
        this.maxKeys = maxKeys;
        this.maxSwitches = maxSwitches;

        gridCoords = new MZIntMap<Vector2Int>();
        roomIds = new Vector2IntMap<int>();
        Vector2Int first = new Vector2Int(0,0);
        firstRoomId = GetRoomId(first);
    }
    
    public int GetRoomId(Vector2Int xy) {
        if (roomIds.containsKey(xy)) {
            assert gridCoords.Get(roomIds.Get(xy)).Equals(xy);
            return roomIds.Get(xy);
        } else {
            int id = gridCoords.newInt();
            gridCoords.put(id, xy);
            roomIds.put(xy, id);
            return id;
        }
    }
    
    public Vector2Int GetRoomCoords(int id) {
        assert gridCoords.containsKey(id);
        return gridCoords.Get(id);
    }
    
    @Override
    public int GetMaxRooms() {
        return maxSpaces;
    }
    
    public void setMaxSpaces(int maxSpaces) {
        this.maxSpaces = maxSpaces;
    }
    
    @Override
    public Collection<int> initialRooms() {
        return Arrays.asList(firstRoomId);
    }

    @Override
    public int GetMaxKeys() {
        return maxKeys;
    }
    
    public void setMaxKeys(int maxKeys) {
        this.maxKeys = maxKeys;
    }
    
    @Override
    public bool isAcceptable(IMZDungeon dungeon) {
        return true;
    }

    @Override
    public int GetMaxSwitches() {
        return maxSwitches;
    }

    public void setMaxSwitches(int maxSwitches) {
        this.maxSwitches = maxSwitches;
    }

    protected bool validRoomCoords(Vector2Int c) {
        return c.y <= 0;
    }
    
    @Override
    public List<Pair<Double,int>> GetAdjacentRooms(int id, int keyLevel) {
        Vector2Int xy = gridCoords.Get(id);
        List<Pair<Double,int>> ids = new ArrayList<Pair<Double,int>>();
        for (Direction d: Direction.CARDINALS) {
            Vector2Int neighbor = xy.Add(d);
            if (validRoomCoords(neighbor))
                ids.Add(new Pair<Double,int>(1.0,getRoomId(neighbor)));
        }
        return ids;
    }

    @Override
    public Set<Vector2Int> GetCoords(int id) {
        return new Vector2IntSet(Arrays.asList(getRoomCoords(id)));
    }

    @Override
    public double edgeGraphifyProbability(int id, int nextId) {
        return 0.2;
    }

    @Override
    public bool roomCanFitItem(int id, MZSymbol key) {
        return true;
    }

}
