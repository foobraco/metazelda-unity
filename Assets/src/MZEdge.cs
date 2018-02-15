using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Links two {@link MZRoom}s.
 * <p>
 * The attached {@link MZSymbol} is a condition that must be satisfied for the
 * player to pass from one of the linked MZRooms to the other via this MZEdge. It is
 * implemented as a {@link MZSymbol} rather than a {@link MZCondition} to simplify
 * the interface to clients of the library so that they don't have to handle the
 * case where multiple MZSymbols are required to pass through an MZEdge.
 * <p>
 * An unconditional MZEdge is one that may always be used to go from one of the
 * linked MZRooms to the other.
 */
public class MZEdge {

    protected int tarGetRoomId;
    public MZSymbol Symbol { get; set; }
   
    /**
     * Creates an unconditional MZEdge.
     */
    public MZEdge(int tarGetRoomId) : this(tarGetRoomId, null) {}
    
    /**
     * Creates an MZEdge that requires a particular MZSymbol to be collected before
     * it may be used by the player to travel between the MZRooms.
     * 
     * @param symbol    the symbol that must be obtained
     */
    public MZEdge(int tarGetRoomId, MZSymbol symbol) {
        this.tarGetRoomId = tarGetRoomId;
        this.Symbol = symbol;
    }
    
    /**
     * @return  whether the MZEdge is conditional
     */
    public bool HasSymbol() {
        return Symbol != null;
    }
    
    /**
     * @return  the symbol that must be obtained to pass along this edge or null
     *          if there are no required symbols
     */
    public MZSymbol GetSymbol() {
        return Symbol;
    }
    
    public void SetSymbol(MZSymbol symbol) {
        this.Symbol = symbol;
    }
    
    public int GetTarGetRoomId() {
        return tarGetRoomId;
    }
    
    public override bool Equals(object other) {
        if (other.GetType() == typeof(MZEdge))
        {
            MZEdge o = (MZEdge)other;
            return tarGetRoomId == o.tarGetRoomId &&
                    (Symbol == o.Symbol || Symbol.Equals(o.Symbol));
        } else {
            return base.Equals(other);
        }
    }
    
}
