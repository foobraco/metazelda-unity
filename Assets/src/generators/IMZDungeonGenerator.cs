using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Interface for classes that provide methods to procedurally Generate new
 * {@link MZIDungeon}s.
 */
public interface IMZDungeonGenerator {

    /**
     * Generates a new {@link MZIDungeon}.
     */
    void Generate();
    
    /**
     * Gets the most recently Generated {@link MZIDungeon}.
     * 
     * @return the most recently Generated MZIDungeon
     */
    MZIDungeon GetDungeon();
    
}
