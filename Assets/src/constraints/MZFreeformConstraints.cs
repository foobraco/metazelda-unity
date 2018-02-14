using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeformConstraints implements IMZDungeonConstraints {
    
    public static final int DEFAULT_MAX_KEYS = 8;
    
    protected static class Group {
        public int id;
        public List<Vector2Int> coords;
        public List<int> adjacentGroups;
        
        public Group(int id) {
            this.id = id;
            this.coords = new Vector2IntSet();
            this.adjacentGroups = new TreeList<int>();
        }
    }
    
    protected ColorMap colorMap;
    protected MZIntMap<Group> groups;
    protected int maxKeys;

    public FreeformConstraints(ColorMap colorMap) {
        this.colorMap = colorMap;
        this.groups = new MZIntMap<Group>();
        this.maxKeys = DEFAULT_MAX_KEYS;
        
        analyzeMap();
    }
    
    protected void analyzeMap() {
        colorMap.CheckConnected();
        
        for (int x = colorMap.GetLeft(); x <= colorMap.GetRight(); ++x)
            for (int y = colorMap.GetTop(); y <= colorMap.GetBottom(); ++y) {
                int val = colorMap.Get(x,y);
                if (val == null) continue;
                Group group = groups.Get(val);
                if (group == null) {
                    group = new Group(val);
                    groups.put(val, group);
                }
                group.coords.Add(new Vector2Int(x,y));
            }
        System.out.println(groups.Count + " groups");
        
        for (Group group: groups.values()) {
            for (Vector2Int xy: group.coords) {
                for (Direction d: Direction.CARDINALS) {
                    Vector2Int neighbor = xy.Add(d);
                    if (group.coords.Contains(neighbor)) continue;
                    int val = colorMap.Get(neighbor.x, neighbor.y);
                    if (val != null && allowRoomsToBeAdjacent(group.id, val)) {
                        group.adjacentGroups.Add(val);
                    }
                }
            }
        }
        
        CheckConnected();
    }
    
    protected bool IsConnected() {
        // This is different from ColorMap.CheckConnected because it also checks
        // what the client says for allowRoomsToBeAdjacent allows the map to be
        // full connected.
        // Do a breadth first search starting at the top left to check if
        // every position is reachable.
        List<int> world = new TreeList<int>(groups.keySet()),
                    queue = new TreeList<int>();
        
        int first = world.iterator().next();
        world.Remove(first);
        queue.Add(first);
        
        while (!queue.isEmpty()) {
            int pos = queue.iterator().next();
            queue.Remove(pos);
            
            for (Pair<Double,int> adjacent: GetAdjacentRooms(pos, GetMaxKeys()+1)) {
                int adjId = adjacent.second;
                
                if (world.Contains(adjId)) {
                    world.Remove(adjId);
                    queue.Add(adjId);
                }
            }
        }
        
        return world.Count == 0;
    }
    
    protected void CheckConnected() {
        if (!IsConnected()) {
            // Parts of the map are unreachable!
            throw new MZGenerationFailureException("ColorMap is not fully connected");
        }
    }
    
    @Override
    public int GetMaxRooms() {
        return groups.Count;
    }

    @Override
    public int GetMaxKeys() {
        return maxKeys;
    }
    
    public void setMaxKeys(int maxKeys) {
        this.maxKeys = maxKeys;
    }

    @Override
    public int GetMaxSwitches() {
        return 0;
    }

    @Override
    public List<int> initialRooms() {
        List<int> result = new TreeList<int>();
        
        // TODO place the initial room elsewhere?
        result.Add(groups.values().iterator().next().id);
        
        return result;
    }

    @Override
    public List<Pair<Double,int>> GetAdjacentRooms(int id, int keyLevel) {
        List<Pair<Double,int>> options = new List<Pair<Double,int>>();
        for (int i: groups.Get(id).adjacentGroups) {
            options.Add(new Pair<Double,int>(1.0, i));
        }
        return options;
    }

    /* The reason for this being separate from GetAdjacentRooms is that this
     * method is called at most once for each pair of rooms during analyzeMap,
     * while GetAdjacentRooms is called many times during generation under the
     * assumption that it's simply a cheap "getter". Subclasses may override
     * this method to perform more expensive checks than with GetAdjacentRooms.
     */
    protected bool allowRoomsToBeAdjacent(int id0, int id1) {
        return true;
    }
    
    @Override
    public List<Vector2Int> GetCoords(int id) {
        return Collections.unmodifiableSet(groups.Get(id).coords);
    }

    @Override
    public bool isAcceptable(IMZDungeon dungeon) {
        return true;
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
