using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Links two {@link Room}s.
 * <p>
 * The attached {@link MZSymbol} is a condition that must be satisfied for the
 * player to pass from one of the linked Rooms to the other via this MZEdge. It is
 * implemented as a {@link MZSymbol} rather than a {@link MZCondition} to simplify
 * the interface to clients of the library so that they don't have to handle the
 * case where multiple MZSymbols are required to pass through an MZEdge.
 * <p>
 * An unconditional MZEdge is one that may always be used to go from one of the
 * linked Rooms to the other.
 */
public class MZEdge {

    protected int targetRoomId;
    protected MZSymbol symbol;
   
    /**
     * Creates an unconditional MZEdge.
     */
    public MZEdge(int targetRoomId) : this(targetRoomId, null) {}
    
    /**
     * Creates an MZEdge that requires a particular MZSymbol to be collected before
     * it may be used by the player to travel between the Rooms.
     * 
     * @param symbol    the symbol that must be obtained
     */
    public MZEdge(int targetRoomId, MZSymbol symbol) {
        this.targetRoomId = targetRoomId;
        this.symbol = symbol;
    }
    
    /**
     * @return  whether the MZEdge is conditional
     */
    public bool HasSymbol() {
        return symbol != null;
    }
    
    /**
     * @return  the symbol that must be obtained to pass along this edge or null
     *          if there are no required symbols
     */
    public MZSymbol GetSymbol() {
        return symbol;
    }
    
    public void SetSymbol(MZSymbol symbol) {
        this.symbol = symbol;
    }
    
    public int GetTargetRoomId() {
        return targetRoomId;
    }
    
    public override bool Equals(object other) {
        if (other.GetType() == typeof(MZEdge))
        {
            MZEdge o = (MZEdge)other;
            return targetRoomId == o.targetRoomId &&
                    (symbol == o.symbol || symbol.Equals(o.symbol));
        } else {
            return base.Equals(other);
        }
    }
    
}
