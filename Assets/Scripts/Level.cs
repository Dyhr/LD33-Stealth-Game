using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
    public bool Generate;
    public int Rooms;
    public int Tries;
    public float WallThickness;
    public float WallHeight;
    public float DoorWidth;
    public float DoorHeight;
    public Material WallMaterial;
    public Material FloorMaterial;

    public Vector2 MinRoomSize;
    public Vector2 MaxRoomSize;

    public Door Door;
    public Guard Guard;
    public Terminal Terminal;
    public Transform Spawn;
    public Transform Goal;

    private void Start()
    {
        if (Generate) Remake();
    }
    private void OnDrawGizmos()
    {
        if (Generate) Remake();
    }

    private List<Room> GetMap()
    {
        var map = new List<Room>();

        map.Add(new Room(Vector2.zero, MaxRoomSize, Vector4.zero));

        for (int k = 0; k < Rooms; ++k)
        {
            var o = map[Random.Range(0, map.Count)];
            for (int i = 0; i < 4; ++i)
            {
                if (o.Doors[i] != 0) continue;
                for (int t = 0; t < Tries; ++t)
                {
                    var room = new Room(o.Position,
                        new Vector2(Random.Range(MinRoomSize.x, MaxRoomSize.x), Random.Range(MinRoomSize.y, MaxRoomSize.y)),
                        Vector4.zero);

                    var sx = Mathf.Min(o.Size.x - DoorWidth - WallThickness, room.Size.x - DoorWidth - WallThickness);
                    var sy = Mathf.Min(o.Size.y - DoorWidth - WallThickness, room.Size.y - DoorWidth - WallThickness);

                    float min, max;
                    switch (i)
                    {
                        case 0:
                            room.Position -= Vector2.right * (o.Size.x + room.Size.x) / 2;
                            room.Position += Vector2.up * Random.Range(-sy, sy);

                            min = (room.Position.y - o.Position.y - room.Size.y / 2 + o.Size.y / 2 +
                                   (WallThickness + DoorWidth / 2)) / o.Size.y;
                            max = (room.Position.y - o.Position.y + room.Size.y / 2 + o.Size.y / 2 -
                                   (WallThickness + DoorWidth / 2)) / o.Size.y;
                            min = Mathf.Clamp(min, (WallThickness + DoorWidth / 2) / o.Size.y,
                                1 - (WallThickness + DoorWidth / 2) / o.Size.y);
                            max = Mathf.Clamp(max, (WallThickness + DoorWidth / 2) / o.Size.y,
                                1 - (WallThickness + DoorWidth / 2) / o.Size.y);
                            if (!(min == max && (max == 0 || max == 1)))
                            {
                                o.Doors[0] = Random.Range(min, max);
                                room.Doors[1] = (o.Doors[0] * o.Size.y + o.Position.y - o.Size.y / 2 + room.Size.y / 2 -
                                                 room.Position.y) / room.Size.y;
                                o.Neighbors[0] = room;
                                room.Neighbors[1] = o;
                            }
                            break;
                        case 1:
                            room.Position += Vector2.right * (o.Size.x + room.Size.x) / 2;
                            room.Position += Vector2.up * Random.Range(-sy, sy);

                            min = (room.Position.y - o.Position.y - room.Size.y / 2 + o.Size.y / 2 +
                                   (WallThickness + DoorWidth / 2)) / o.Size.y;
                            max = (room.Position.y - o.Position.y + room.Size.y / 2 + o.Size.y / 2 -
                                   (WallThickness + DoorWidth / 2)) / o.Size.y;
                            min = Mathf.Clamp(min, (WallThickness + DoorWidth / 2) / o.Size.y,
                                1 - (WallThickness + DoorWidth / 2) / o.Size.y);
                            max = Mathf.Clamp(max, (WallThickness + DoorWidth / 2) / o.Size.y,
                                1 - (WallThickness + DoorWidth / 2) / o.Size.y);
                            if (!(min == max && (max == 0 || max == 1)))
                            {
                                o.Doors[1] = Random.Range(min, max);
                                room.Doors[0] = (o.Doors[1] * o.Size.y + o.Position.y - o.Size.y / 2 + room.Size.y / 2 -
                                                 room.Position.y) / room.Size.y;
                                o.Neighbors[1] = room;
                                room.Neighbors[0] = o;
                            }
                            break;
                        case 2:
                            room.Position -= Vector2.up * (o.Size.y + room.Size.y) / 2;
                            room.Position += Vector2.right * Random.Range(-sx, sx);

                            min = (room.Position.x - o.Position.x - room.Size.x / 2 + o.Size.x / 2 +
                                   (WallThickness + DoorWidth / 2)) / o.Size.x;
                            max = (room.Position.x - o.Position.x + room.Size.x / 2 + o.Size.x / 2 -
                                   (WallThickness + DoorWidth / 2)) / o.Size.x;
                            min = Mathf.Clamp(min, (WallThickness + DoorWidth / 2) / o.Size.x,
                                1 - (WallThickness + DoorWidth / 2) / o.Size.x);
                            max = Mathf.Clamp(max, (WallThickness + DoorWidth / 2) / o.Size.x,
                                1 - (WallThickness + DoorWidth / 2) / o.Size.x);
                            if (!(min == max && (max == 0 || max == 1)))
                            {
                                o.Doors[2] = Random.Range(min, max);
                                room.Doors[3] = (o.Doors[2] * o.Size.x + o.Position.x - o.Size.x / 2 + room.Size.x / 2 -
                                                 room.Position.x) / room.Size.x;
                                o.Neighbors[2] = room;
                                room.Neighbors[3] = o;
                            }
                            break;
                        case 3:
                            room.Position += Vector2.up * (o.Size.y + room.Size.y) / 2;
                            room.Position += Vector2.right * Random.Range(-sx, sx);

                            min = (room.Position.x - o.Position.x - room.Size.x / 2 + o.Size.x / 2 +
                                   (WallThickness + DoorWidth / 2)) / o.Size.x;
                            max = (room.Position.x - o.Position.x + room.Size.x / 2 + o.Size.x / 2 -
                                   (WallThickness + DoorWidth / 2)) / o.Size.x;
                            min = Mathf.Clamp(min, (WallThickness + DoorWidth / 2) / o.Size.x,
                                1 - (WallThickness + DoorWidth / 2) / o.Size.x);
                            max = Mathf.Clamp(max, (WallThickness + DoorWidth / 2) / o.Size.x,
                                1 - (WallThickness + DoorWidth / 2) / o.Size.x);
                            if (!(min == max && (max == 0 || max == 1)))
                            {
                                o.Doors[3] = Random.Range(min, max);
                                room.Doors[2] = (o.Doors[3] * o.Size.x + o.Position.x - o.Size.x / 2 + room.Size.x / 2 -
                                                 room.Position.x) / room.Size.x;
                                o.Neighbors[3] = room;
                                room.Neighbors[2] = o;
                            }
                            break;
                    }

                    var done = true;
                    for (int j = 0; j < map.Count; ++j)
                    {
                        var other = map[j];
                        var dp = room.Position - other.Position;
                        var ds = (room.Size + other.Size) / 2;
                        if (Mathf.Abs(dp.x) < ds.x && Mathf.Abs(dp.y) < ds.y)
                        {
                            done = false;
                            break;
                        }
                    }
                    if (done)
                    {
                        map.Add(room);
                        break;
                    }
                    o.Doors[i] = 0;
                    o.Neighbors[i] = null;
                }
            }
        }

        var end = Furthest(map[0]);
        var start = Furthest(end);

        start.Spawns.Add(Spawn.transform);
        start.Spawns.Add(Terminal.transform);
        end.Spawns.Add(Goal.transform);

        var path = Path(start, end);
        var branches = path.Select(room => room.Neighbors.Where(r => r != null && !path.Contains(r)).ToArray()).ToList();
        foreach (var branch in branches)
        {
            foreach (var room in branch)
            {
                if (room == null) continue;

                var p = room.Neighbors.Single(path.Contains);
                int i = 0;
                while (room.Neighbors[i] != p) i++;
                room.Neighbors[i] = null;

                var e = Furthest(room);
                e.Spawns.Add(Terminal.transform);

                room.Neighbors[i] = p;
            }
        }

        map[Random.Range(0, map.Count)].Spawns.Add(Guard.transform);
        map[Random.Range(0, map.Count)].Spawns.Add(Guard.transform);

        return map;
    }

    private List<Room> Path(Room a, Room b)
    {
        RLog.Clear();
        var result = new List<Room>();
        IterateP(a, b, result);
        return result;
    }

    private bool IterateP(Room room, Room end, List<Room> path)
    {
        if (room == null || room.Neighbors == null || RLog.Contains(room))
        {
            return false;
        }
        RLog.Add(room);

        path.Add(room);
        if (room == end) return true;

        for (int j = 0; j < room.Neighbors.Length; j++)
        {
            var neighbor = room.Neighbors[j];
            if (neighbor == null || RLog.Contains(neighbor)) continue;

            var v = IterateP(neighbor, end, path);
            if (v) return true;
        }
        path.Remove(room);
        return false;
    }

    private HashSet<Room> RLog = new HashSet<Room>(); 
    private Room Furthest(Room room)
    {
        RLog.Clear();
        Room result;
        IterateF(room, out result, 0);
        return result;
    }
    private int IterateF(Room room, out Room result, int i)
    {
        if (room == null || room.Neighbors == null || RLog.Contains(room))
        {
            result = room;
            return i;
        }
        RLog.Add(room);

        var max = 0;
        var mroom = room;

        for (int j = 0; j < room.Neighbors.Length; j++)
        {
            var neighbor = room.Neighbors[j];
            if (neighbor == null || RLog.Contains(neighbor)) continue;
            Room r;
            var v = IterateF(neighbor, out r, i + 1);
            if (v > max)
            {
                max = v;
                mroom = r;
            }
        }
        result = mroom;
        return Mathf.Max(max,i);
    }

    public void Remake()
    {
        Generate = false;
        while (transform.childCount > 0) DestroyImmediate(transform.GetChild(0).gameObject);

        var map = GetMap();

        foreach (var room in map)
        {
            var g = new GameObject("Room").transform;
            g.parent = transform;
            g.position = new Vector3(room.Position.x, 0, room.Position.y);
            g.tag = "Merge";

            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            floor.parent = g;
            floor.localScale = new Vector3(room.Size.x, 0.1f, room.Size.y);
            floor.localPosition = new Vector3(0, -0.05f, 0);
            floor.localRotation = Quaternion.identity;
            floor.GetComponent<MeshRenderer>().sharedMaterial = FloorMaterial;
            floor.gameObject.layer = LayerMask.NameToLayer("Ground");

            var patrol = new GameObject("Room").transform;
            patrol.parent = transform;
            patrol.position = new Vector3(room.Position.x, 0, room.Position.y);
            patrol.tag = "Patrol";

            for (int i = 0; i < room.Spawns.Count; i++)
            {
                var spawn = Instantiate(room.Spawns[i]);
                spawn.transform.position = g.position + new Vector3(
                        Random.Range(-room.Size.x/2 + WallThickness*2, room.Size.x/2 - WallThickness*2),
                        0.5f,
                        Random.Range(-room.Size.y/2 + WallThickness*2, room.Size.y/2 - WallThickness*2));
                spawn.parent = transform;
            }

            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            wall.parent = g;
            wall.localScale = new Vector3(WallThickness, WallHeight, room.Size.y * (1 - room.Doors[0]) - (room.Doors[0] != 0 ? DoorWidth / 2 : 0));
            wall.localPosition = new Vector3(-room.Size.x / 2 + WallThickness / 2, WallHeight / 2, room.Size.y * room.Doors[0] / 2 + (room.Doors[0] != 0 ? DoorWidth / 4 : 0));
            wall.localRotation = Quaternion.identity;
            wall.GetComponent<MeshRenderer>().sharedMaterial = FloorMaterial;
            wall.gameObject.layer = LayerMask.NameToLayer("Obstacles");
            wall = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            wall.parent = g;
            wall.localScale = new Vector3(WallThickness, WallHeight, room.Size.y * (1 - room.Doors[1]) - (room.Doors[1] != 0 ? DoorWidth / 2 : 0));
            wall.localPosition = new Vector3(room.Size.x / 2 - WallThickness / 2, WallHeight / 2, room.Size.y * room.Doors[1] / 2 + (room.Doors[1] != 0 ? DoorWidth / 4 : 0));
            wall.localRotation = Quaternion.identity;
            wall.GetComponent<MeshRenderer>().sharedMaterial = FloorMaterial;
            wall.gameObject.layer = LayerMask.NameToLayer("Obstacles");

            wall = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            wall.parent = g;
            wall.localScale = new Vector3(WallThickness, WallHeight, room.Size.y * room.Doors[0] - (room.Doors[0] != 0 ? DoorWidth / 2 : 0));
            wall.localPosition = new Vector3(-room.Size.x / 2 + WallThickness / 2, WallHeight / 2, -room.Size.y * (1 - room.Doors[0]) / 2 - (room.Doors[0] != 0 ? DoorWidth / 4 : 0));
            wall.localRotation = Quaternion.identity;
            wall.GetComponent<MeshRenderer>().sharedMaterial = FloorMaterial;
            wall.gameObject.layer = LayerMask.NameToLayer("Obstacles");
            wall = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            wall.parent = g;
            wall.localScale = new Vector3(WallThickness, WallHeight, room.Size.y * room.Doors[1] - (room.Doors[1] != 0 ? DoorWidth / 2 : 0));
            wall.localPosition = new Vector3(room.Size.x / 2 - WallThickness / 2, WallHeight / 2, -room.Size.y * (1 - room.Doors[1]) / 2 - (room.Doors[1] != 0 ? DoorWidth / 4 : 0));
            wall.localRotation = Quaternion.identity;
            wall.GetComponent<MeshRenderer>().sharedMaterial = FloorMaterial;
            wall.gameObject.layer = LayerMask.NameToLayer("Obstacles");


            wall = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            wall.parent = g;
            wall.localScale = new Vector3(room.Size.x * (1 - room.Doors[2]) - (room.Doors[2] != 0 ? DoorWidth / 2 : 0), WallHeight, WallThickness);
            wall.localPosition = new Vector3(room.Size.x * room.Doors[2] / 2 + (room.Doors[2] != 0 ? DoorWidth / 4 : 0), WallHeight / 2, -room.Size.y / 2 + WallThickness / 2);
            wall.localRotation = Quaternion.identity;
            wall.GetComponent<MeshRenderer>().sharedMaterial = FloorMaterial;
            wall.gameObject.layer = LayerMask.NameToLayer("Obstacles");
            wall = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            wall.parent = g;
            wall.localScale = new Vector3(room.Size.x * (1 - room.Doors[3]) - (room.Doors[3] != 0 ? DoorWidth / 2 : 0), WallHeight, WallThickness);
            wall.localPosition = new Vector3(room.Size.x * room.Doors[3] / 2 + (room.Doors[3] != 0 ? DoorWidth / 4 : 0), WallHeight / 2, room.Size.y / 2 - WallThickness / 2);
            wall.localRotation = Quaternion.identity;
            wall.GetComponent<MeshRenderer>().sharedMaterial = FloorMaterial;
            wall.gameObject.layer = LayerMask.NameToLayer("Obstacles");

            wall = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            wall.parent = g;
            wall.localScale = new Vector3(room.Size.x * room.Doors[2] - (room.Doors[2] != 0 ? DoorWidth / 2 : 0), WallHeight, WallThickness);
            wall.localPosition = new Vector3(-room.Size.x * (1 - room.Doors[2]) / 2 - (room.Doors[2] != 0 ? DoorWidth / 4 : 0), WallHeight / 2, -room.Size.y / 2 + WallThickness / 2);
            wall.localRotation = Quaternion.identity;
            wall.GetComponent<MeshRenderer>().sharedMaterial = FloorMaterial;
            wall.gameObject.layer = LayerMask.NameToLayer("Obstacles");
            wall = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            wall.parent = g;
            wall.localScale = new Vector3(room.Size.x * room.Doors[3] - (room.Doors[3] != 0 ? DoorWidth / 2 : 0), WallHeight, WallThickness);
            wall.localPosition = new Vector3(-room.Size.x * (1 - room.Doors[3]) / 2 - (room.Doors[3] != 0 ? DoorWidth / 4 : 0), WallHeight / 2, room.Size.y / 2 - WallThickness / 2);
            wall.localRotation = Quaternion.identity;
            wall.GetComponent<MeshRenderer>().sharedMaterial = FloorMaterial;
            wall.gameObject.layer = LayerMask.NameToLayer("Obstacles");

            Transform door;
            if (room.Doors[0] != 0)
            {
                var p = new Vector3(
                    -room.Size.x / 2,
                    DoorHeight / 2,
                    room.Size.y * (room.Doors[0]) - room.Size.y / 2);
                if (!FindObjectsOfType<Door>().Any(d => Vector3.Distance(p + floor.position, d.transform.position) < 1))
                {
                    door = Instantiate(Door).transform;
                    door.parent = g;
                    door.localScale = new Vector3(DoorWidth, DoorHeight, WallThickness);
                    door.localPosition = p;
                    door.Rotate(0,90,0);
                    door.parent = transform;
                }
            }
            if (room.Doors[1] != 0)
            {
                var p = new Vector3(
                    room.Size.x / 2,
                    DoorHeight / 2,
                    room.Size.y * (room.Doors[1]) - room.Size.y / 2);
                if (!FindObjectsOfType<Door>().Any(d => Vector3.Distance(p + floor.position, d.transform.position) < 1))
                {
                    door = Instantiate(Door).transform;
                    door.parent = g;
                    door.localScale = new Vector3(DoorWidth, DoorHeight, WallThickness);
                    door.localPosition = p;
                    door.Rotate(0, 90, 0);
                    door.parent = transform;
                }
            }
            if (room.Doors[2] != 0)
            {
                var p = new Vector3(
                    room.Size.x * (room.Doors[2]) - room.Size.x / 2,
                    DoorHeight / 2,
                    -room.Size.y / 2);
                if (!FindObjectsOfType<Door>().Any(d => Vector3.Distance(p + floor.position, d.transform.position) < 1))
                {
                    door = Instantiate(Door).transform;
                    door.parent = g;
                    door.localScale = new Vector3(DoorWidth, DoorHeight, WallThickness);
                    door.localPosition = p;
                    door.parent = transform;
                }
            }
            if (room.Doors[3] != 0)
            {
                var p = new Vector3(
                    room.Size.x * (room.Doors[3]) - room.Size.x / 2,
                    DoorHeight / 2,
                    room.Size.y / 2);
                if (!FindObjectsOfType<Door>().Any(d => Vector3.Distance(p + floor.position, d.transform.position) < 1))
                {
                    door = Instantiate(Door).transform;
                    door.parent = g;
                    door.localScale = new Vector3(DoorWidth, DoorHeight, WallThickness);
                    door.localPosition = p;
                    door.parent = transform;
                }
            }
        }

        var network = FindObjectsOfType<Networkable>();
        for (int i = 0; i < network.Length; ++i)
        {
            if (network[i].Neighbors.Count > 0) continue;
            var j = 0;
            do
            {
                j = Random.Range(0, network.Length);
            } while (j == i);
            network[i].Neighbors.Add(network[j]);
            network[j].Neighbors.Add(network[i]);
        }

        //var merger = new GameObject("Merger").AddComponent<MergeMesh>();
        //merger.transform.parent = transform;
        //merger.Merge(GameObject.FindGameObjectsWithTag("Merge").Select(g=>g.transform).ToArray());
    }
}

class Room
{
    public Vector2 Position;
    public Vector2 Size;
    public Vector4 Doors;
    public Room[] Neighbors;
    public List<Transform> Spawns;

    public Room(Vector2 Position, Vector2 Size, Vector4 Doors)
    {
        this.Position = Position;
        this.Size = Size;
        this.Doors = Doors;
        this.Neighbors = new Room[4];
        this.Spawns = new List<Transform>(6);
    }
}
