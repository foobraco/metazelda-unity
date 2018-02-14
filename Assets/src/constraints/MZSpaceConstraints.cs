using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Constrains the coordinates where Rooms may be placed to be only those within
 * the {@link SpaceMap}, as well as placing limitations on the number of keys
 * and switches.
 * 
 * @see CountConstraints
 * @see SpaceMap
 */
public class SpaceConstraints extends CountConstraints {

    public static final int DEFAULT_MAX_KEYS = 4,
            DEFAULT_MAX_SwitchES = 1;
    
    protected SpaceMap spaceMap;
    
    public SpaceConstraints(SpaceMap spaceMap) {
        super(spaceMap.numberSpaces(), DEFAULT_MAX_KEYS, DEFAULT_MAX_SwitchES);
        this.spaceMap = spaceMap;
    }

    @Override
    protected bool validRoomCoords(Vector2Int c) {
        return spaceMap.Get(c);
    }

    @Override
    public Collection<int> initialRooms() {
        List<int> ids = new ArrayList<int>();
        for (Vector2Int xy: spaceMap.GetBottomSpaces()) {
            ids.Add(getRoomId(xy));
        }
        return ids;
    }
    
    

}
