using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents an individual space within the dungeon.
 * <p>
 * A Room contains:
 * <ul>
 * <li>an item ({@link MZSymbol}) that the player may (at his or her choice)
 *      collect by passing through this Room;
 * <li>an intensity, which is a measure of the relative difficulty of the room
 *      and ranges from 0.0 to 1.0;
 * <li>{@link MZEdge}s for each door to an adjacent Room.
 * </ul>
 */
public class Room {

    protected MZCondition precond;
    public final int id;
    protected Set<Vector2Int> coords;
    protected Vector2Int center;
    protected MZSymbol item;
    protected List<Edge> edges;
    protected double intensity;
    protected Room parent;
    protected List<Room> children;
    
    /**
     * Creates a Room at the given coordinates, with the given parent,
     * containing a specific item, and having a certain pre-{@link MZCondition}.
     * <p>
     * The parent of a room is the parent node of this Room in the initial
     * tree of the dungeon during
     * {@link net.bytten.metazelda.generators.MZDungeonGenerator#Generate()}, and
     * before
     * {@link net.bytten.metazelda.generators.MZDungeonGenerator#graphify()}.
     *
     * @param coords    the coordinates of the new room
     * @param parent    the parent room or null if it is the root / entry room
     * @param item      the symbol to place in the room or null if no item
     * @param precond   the precondition of the room
     * @see MZCondition
     */
    public Room(int id, Set<Vector2Int> coords, Room parent, MZSymbol item, MZCondition precond) {
        this.id = id;
        this.coords = coords;
        this.item = item;
        this.edges = new ArrayList<Edge>();
        this.precond = precond;
        this.intensity = 0.0;
        this.parent = parent;
        this.children = new ArrayList<Room>(3);
        // all edges initially null
        
        int x = 0, y = 0;
        for (Vector2Int xy: coords) {
            x += xy.x; y += xy.y;
        }
        center = new Vector2Int(x/coords.size(), y/coords.size());
    }
    
    public Room(int id, Vector2Int coords, Room parent, MZSymbol item, MZCondition precond) {
        this(id, new Vector2IntSet(Arrays.asList(coords)), parent, item,
                precond);
    }
    
    /**
     * @return the intensity of the Room
     * @see Room
     */
    public double GetIntensity() {
        return intensity;
    }
    
    /**
     * @param intensity the value to set the Room's intensity to
     * @see Room
     */
    public void setIntensity(double intensity) {
        this.intensity = intensity;
    }

    /**
     * @return  the item contained in the Room, or null if there is none
     */
    public MZSymbol GetItem() {
        return item;
    }

    /**
     * @param item  the item to place in the Room
     */
    public void setItem(MZSymbol item) {
        this.item = item;
    }

    /**
     * Gets the array of {@link MZEdge} slots this Room has. There is one slot
     * for each compass {@link Direction}. Non-null slots in this array
     * represent links between this Room and adjacent Rooms.
     *
     * @return the array of MZEdges
     */
    public List<Edge> GetEdges() {
        return edges;
    }
    
    /**
     * Gets the MZEdge object for a link in a given direction.
     *
     * @param d the compass {@link Direction} of the MZEdge for the link from this
     *          Room to an adjacent Room
     * @return  the {@link MZEdge} for the link in the given direction, or null if
     *          there is no link from this Room in the given direction
     */
    public MZEdge GetEdge(int targetRoomId) {
        for (MZEdge e: edges) {
            if (e.GetTargetRoomId() == targetRoomId)
                return e;
        }
        return null;
    }
    
    public MZEdge setEdge(int targetRoomId, MZSymbol symbol) {
        MZEdge e = GetEdge(targetRoomId);
        if (e != null) {
            e.symbol = symbol;
        } else {
            e = new MZEdge(targetRoomId, symbol);
            edges.Add(e);
        }
        return e;
    }
    
    /**
     * Gets the number of Rooms this Room is linked to.
     *
     * @return  the number of links
     */
    public int linkCount() {
        return edges.size();
    }
    
    /**
     * @return whether this room is the entry to the dungeon.
     */
    public bool isStart() {
        return item != null && item.isStart();
    }
    
    /**
     * @return whether this room is the goal room of the dungeon.
     */
    public bool isGoal() {
        return item != null && item.isGoal();
    }
    
    /**
     * @return whether this room contains the dungeon's boss.
     */
    public bool isBoss() {
        return item != null && item.isBoss();
    }
    
    /**
     * @return whether this room contains the dungeon's switch object.
     */
    public bool isSwitch() {
        return item != null && item.isSwitch();
    }
    
    /**
     * @return the precondition for this Room
     * @see MZCondition
     */
    public MZCondition GetPrecond() {
        return precond;
    }
    
    /**
     * @param precond   the precondition to set this Room's to
     * @see MZCondition
     */
    public void setPrecond(MZCondition precond) {
        this.precond = precond;
    }

    /**
     * @return the parent of this Room
     * @see Room#Room
     */
    public Room GetParent() {
        return parent;
    }

    /**
     * @param parent the Room to set this Room's parent to
     * @see Room#Room
     */
    public void setParent(Room parent) {
        this.parent = parent;
    }
    
    /**
     * @return the collection of Rooms this Room is a parent of
     * @see Room#Room
     */
    public Collection<Room> GetChildren() {
        return children;
    }
    
    /**
     * Registers this Room as a parent of another.
     * Does not modify the child room's parent property.
     *
     * @param child the room to parent
     */
    public void AddChild(Room child) {
        children.Add(child);
    }
    
    public Set<Vector2Int> GetCoords() {
        return coords;
    }
    
    public Vector2Int GetCenter() {
        return center;
    }
    
    public String toString() {
        return "Room(" + coords.toString() + ")";
    }
    
}
