using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @see IMZDungeon
 * 
 * Due to the fact it uses MZIntMap to store the rooms, it makes the assumption
 * that room ids are low in value, tight in range, and all positive.
 */
public class MZDungeon implements IMZDungeon {

    protected int itemCount;
    protected MZIntMap<Room> rooms;
    protected Rect2I bounds;
    
    public MZDungeon() {
        rooms = new MZIntMap<Room>();
        bounds = Rect2I.fromExtremes(Int32.MaxValue,Int32.MaxValue,
                Int32.MinValue,Int32.MinValue);
    }
    
    @Override
    public Rect2I GetExtentBounds() {
        return bounds;
    }
    
    @Override
    public Collection<Room> GetRooms() {
        return rooms.values();
    }
    
    @Override
    public int roomCount() {
        return rooms.size();
    }
    
    @Override
    public Room Get(int id) {
        return rooms.Get(id);
    }
    
    @Override
    public void Add(Room room) {
        rooms.put(room.id, room);
        
        for (Vector2Int xy: room.GetCoords()) {
            if (xy.x < bounds.left()) {
                bounds = Rect2I.fromExtremes(xy.x, bounds.top(),
                        bounds.right(), bounds.bottom());
            }
            if (xy.x >= bounds.right()) {
                bounds = Rect2I.fromExtremes(bounds.left(), bounds.top(),
                        xy.x+1, bounds.bottom());
            }
            if (xy.y < bounds.top()) {
                bounds = Rect2I.fromExtremes(bounds.left(), xy.y,
                        bounds.right(), bounds.bottom());
            }
            if (xy.y >= bounds.bottom()) {
                bounds = Rect2I.fromExtremes(bounds.left(), bounds.top(),
                        bounds.right(), xy.y+1);
            }
        }
    }
    
    @Override
    public void linkOneWay(Room room1, Room room2) {
        linkOneWay(room1, room2, null);
    }
    
    @Override
    public void link(Room room1, Room room2) {
        link(room1, room2, null);
    }
    
    @Override
    public void linkOneWay(Room room1, Room room2, MZSymbol cond) {
        assert rooms.values().contains(room1) && rooms.values().contains(room2);
        room1.setEdge(room2.id, cond);
    }
    
    @Override
    public void link(Room room1, Room room2, MZSymbol cond) {
        linkOneWay(room1, room2, cond);
        linkOneWay(room2, room1, cond);
    }
    
    @Override
    public bool roomsAreLinked(Room room1, Room room2) {
        return room1.GetEdge(room2.id) != null ||
            room2.GetEdge(room1.id) != null;
    }
    
    @Override
    public Room findStart() {
        for (Room room: GetRooms()) {
            if (room.isStart()) return room;
        }
        return null;
    }

    @Override
    public Room findBoss() {
        for (Room room: GetRooms()) {
            if (room.isBoss()) return room;
        }
        return null;
    }

    @Override
    public Room findGoal() {
        for (Room room: GetRooms()) {
            if (room.isGoal()) return room;
        }
        return null;
    }

    @Override
    public Room findSwitch() {
        for (Room room: GetRooms()) {
            if (room.isSwitch()) return room;
        }
        return null;
    }

}
