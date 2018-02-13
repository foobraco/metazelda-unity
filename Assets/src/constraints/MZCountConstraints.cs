using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Limits the {@link net.bytten.metazelda.generators.IMZDungeonGenerator} in
 * the <i>number</i> of keys, switches and rooms it is allowed to place.
 *
 * Also restrict to a grid of 1x1 rooms.
 *
 * @see MZIDungeonConstraints
 */
public class CountConstraints implements MZIDungeonConstraints {

    protected int maxSpaces, maxKeys, maxSwitches;
    
    protected MZIntMap<Vec2I> gridCoords;
    protected Vec2IMap<Integer> roomIds;
    protected int firstRoomId;
    
    public CountConstraints(int maxSpaces, int maxKeys, int maxSwitches) {
        this.maxSpaces = maxSpaces;
        this.maxKeys = maxKeys;
        this.maxSwitches = maxSwitches;

        gridCoords = new MZIntMap<Vec2I>();
        roomIds = new Vec2IMap<Integer>();
        Vec2I first = new Vec2I(0,0);
        firstRoomId = getRoomId(first);
    }
    
    public int getRoomId(Vec2I xy) {
        if (roomIds.containsKey(xy)) {
            assert gridCoords.get(roomIds.get(xy)).equals(xy);
            return roomIds.get(xy);
        } else {
            int id = gridCoords.newInt();
            gridCoords.put(id, xy);
            roomIds.put(xy, id);
            return id;
        }
    }
    
    public Vec2I getRoomCoords(int id) {
        assert gridCoords.containsKey(id);
        return gridCoords.get(id);
    }
    
    @Override
    public int getMaxRooms() {
        return maxSpaces;
    }
    
    public void setMaxSpaces(int maxSpaces) {
        this.maxSpaces = maxSpaces;
    }
    
    @Override
    public Collection<Integer> initialRooms() {
        return Arrays.asList(firstRoomId);
    }

    @Override
    public int getMaxKeys() {
        return maxKeys;
    }
    
    public void setMaxKeys(int maxKeys) {
        this.maxKeys = maxKeys;
    }
    
    @Override
    public boolean isAcceptable(MZIDungeon dungeon) {
        return true;
    }

    @Override
    public int getMaxSwitches() {
        return maxSwitches;
    }

    public void setMaxSwitches(int maxSwitches) {
        this.maxSwitches = maxSwitches;
    }

    protected boolean validRoomCoords(Vec2I c) {
        return c.y <= 0;
    }
    
    @Override
    public List<Pair<Double,Integer>> getAdjacentRooms(int id, int keyLevel) {
        Vec2I xy = gridCoords.get(id);
        List<Pair<Double,Integer>> ids = new ArrayList<Pair<Double,Integer>>();
        for (Direction d: Direction.CARDINALS) {
            Vec2I neighbor = xy.add(d);
            if (validRoomCoords(neighbor))
                ids.add(new Pair<Double,Integer>(1.0,getRoomId(neighbor)));
        }
        return ids;
    }

    @Override
    public Set<Vec2I> getCoords(int id) {
        return new Vec2ISet(Arrays.asList(getRoomCoords(id)));
    }

    @Override
    public double edgeGraphifyProbability(int id, int nextId) {
        return 0.2;
    }

    @Override
    public boolean roomCanFitItem(int id, Symbol key) {
        return true;
    }

}
