using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * If you just want to use the Metazelda library, this is the only file you need to modify / replace.
 */

public class MZUnityBehaviourExample : MonoBehaviour {
    public MZDungeonGenerator generator;
    public GameObject normalRoom;
    public GameObject entranceRoom;
    public GameObject bossRoom;
    public GameObject goalRoom;
    public GameObject openDoor;
    public GameObject lockedDoor;
    public GameObject blockedDoor;
    public GameObject key;
    public GameObject roomSwitch;
    public GameObject roomSwitchOn;
    public GameObject roomSwitchOff;
    public GameObject[] tileMapObjects;
    private Tilemap tileMap;

    private readonly Color[] keyColors = { Color.blue, Color.yellow, Color.magenta, Color.cyan, Color.red, Color.green };

    void Start () {
        float roomRatio = 0.6875f; // 256x176


        // Use CountConstraints to make a truly random map.
        //CountConstraints constraints = new CountConstraints(50, 3, 3);


        // Use SpaceConstraints to make a map fitting to a shape.
        MZSpaceMap spaceMap = new MZSpaceMap();
        tileMap = tileMapObjects[Random.Range(0, tileMapObjects.Length)].GetComponentInChildren<Tilemap>();
        foreach (Vector3Int posWithZ in tileMap.cellBounds.allPositionsWithin.GetEnumerator())
        {
            if (tileMap.HasTile(posWithZ))
            {
                Vector2Int pos = new Vector2Int(posWithZ.x, posWithZ.y);
                spaceMap.Set(pos, true);
            }
        }
        SpaceConstraints constraints = new SpaceConstraints(spaceMap);


        generator = new MZDungeonGenerator(Random.Range(0, int.MaxValue), constraints);
        generator.Generate();
        IMZDungeon dungeon = generator.GetMZDungeon();
        foreach (MZRoom room in dungeon.GetRooms())
        {
            MZSymbol item = room.GetItem();
            GameObject toInstantiate = normalRoom;
            Color roomColor = new Color((float) room.GetIntensity(), 1.0f - (float) room.GetIntensity(), 0.5f - (float)room.GetIntensity()/2);
            if (item != null)
            {
                switch (item.GetValue())
                {
                    case (int)MZSymbol.MZSymbolValue.Start:
                        toInstantiate = entranceRoom;
                        roomColor = Color.white;
                        break;

                    case (int)MZSymbol.MZSymbolValue.Boss:
                        toInstantiate = bossRoom;
                        break;

                    case (int)MZSymbol.MZSymbolValue.Goal:
                        toInstantiate = goalRoom;
                        roomColor = Color.white;
                        break;

                    default:
                        break;
                }

                if (item.GetValue() >= 0)
                {
                    GameObject keyObjectInstance = Instantiate(key, new Vector3(room.GetCoords()[0].x, room.GetCoords()[0].y * roomRatio, 0), Quaternion.identity, transform);
                    keyObjectInstance.GetComponent<SpriteRenderer>().color = keyColors[item.GetValue()];
                    keyObjectInstance.transform.localScale += new Vector3(2, 2, 2);
                }
                else if (item.GetValue() == (int) MZSymbol.MZSymbolValue.Switch)
                {
                    GameObject keyObjectInstance = Instantiate(roomSwitch, new Vector3(room.GetCoords()[0].x, room.GetCoords()[0].y * roomRatio, 0), Quaternion.identity, transform);
                    keyObjectInstance.transform.localScale += new Vector3(2, 2, 2);
                }
            }

            GameObject roomObject = Instantiate(toInstantiate, new Vector3(room.GetCoords()[0].x, room.GetCoords()[0].y * roomRatio, 0), Quaternion.identity, transform);
            roomObject.GetComponent<SpriteRenderer>().color = roomColor;

            foreach (MZEdge edge in room.GetEdges())
            {
                MZRoom targetRoom = dungeon.Get(edge.GetTargetRoomId());
                Vector2Int edgeDir = targetRoom.GetCoords()[0] - room.GetCoords()[0];

                toInstantiate = openDoor;
                GameObject keyObject = null;
                Color keyColor = Color.white;
                if (edge.GetSymbol() != null)
                {
                    switch (edge.GetSymbol().GetValue())
                    {
                        case (int)MZSymbol.MZSymbolValue.SwitchOn:
                            toInstantiate = blockedDoor;
                            keyObject = roomSwitchOn;
                            break;

                        case (int)MZSymbol.MZSymbolValue.SwitchOff:
                            toInstantiate = blockedDoor;
                            keyObject = roomSwitchOff;
                            break;

                        default:
                            break;
                    }

                    if (edge.GetSymbol().GetValue() >= 0)
                    {
                        toInstantiate = lockedDoor;
                        keyObject = key;
                        keyColor = keyColors[edge.GetSymbol().GetValue()];
                    }
                }

                // This only works for identically sized rooms on a grid.
                Vector3 pos = Vector3.zero;
                if (edgeDir == Vector2Int.right)
                {
                    pos = new Vector3(room.GetCoords()[0].x + 0.5f, room.GetCoords()[0].y * roomRatio, 0);
                    GameObject doorObject = Instantiate(toInstantiate, pos, Quaternion.identity, transform);
                    if (keyObject != null)
                    {
                        GameObject keyObjectInstance = Instantiate(keyObject, pos, Quaternion.identity, transform);
                        keyObjectInstance.GetComponent<SpriteRenderer>().color = keyColor;
                        keyObjectInstance.transform.localScale += new Vector3(1, 1, 1);
                    }
                }
                else if (edgeDir == Vector2Int.down)
                {
                    pos = new Vector3(room.GetCoords()[0].x, room.GetCoords()[0].y * roomRatio - (roomRatio / 2), 0);
                    var relativePos = Vector2Int.zero + Vector2Int.up;
                    var angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
                    var rotation = Quaternion.AngleAxis(angle, Vector3.forward); // 90 degrees
                    GameObject doorObject = Instantiate(toInstantiate, pos, rotation, transform);
                    if (keyObject != null)
                    {
                        GameObject keyObjectInstance = Instantiate(keyObject, pos, Quaternion.identity, transform);
                        keyObjectInstance.GetComponent<SpriteRenderer>().color = keyColor;
                        keyObjectInstance.transform.localScale += new Vector3(1, 1, 1);
                    }
                }
            }
        }
	}
	
	void Update () {
		
	}
}
