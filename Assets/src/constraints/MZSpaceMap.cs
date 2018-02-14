using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Controls which spaces are valid for an
 * {@link net.bytten.metazelda.generators.IMZDungeonGenerator} to create
 * {@link Room}s in.
 * <p>
 * Essentially just a Set<{@link Vector2Int}> with some convenience methods.
 *
 * @see Vector2Int
 * @see SpaceConstraints
 */
public class SpaceMap {
    protected Set<Vector2Int> spaces = new Vector2IntSet();
    
    public int numberSpaces() {
        return spaces.size();
    }
    
    public bool Get(Vector2Int c) {
        return spaces.contains(c);
    }
    
    public void Set(Vector2Int c, bool val) {
        if (val)
            spaces.Add(c);
        else
            spaces.remove(c);
    }
    
    private Vector2Int GetFirst() {
        return spaces.iterator().next();
    }
    
    public Collection<Vector2Int> GetBottomSpaces() {
        List<Vector2Int> bottomRow = new ArrayList<Vector2Int>();
        bottomRow.Add(getFirst());
        int bottomY = GetFirst().y;
        for (Vector2Int space: spaces) {
            if (space.y > bottomY) {
                bottomY = space.y;
                bottomRow = new ArrayList<Vector2Int>();
                bottomRow.Add(space);
            } else if (space.y == bottomY) {
                bottomRow.Add(space);
            }
        }
        return bottomRow;
    }
}
