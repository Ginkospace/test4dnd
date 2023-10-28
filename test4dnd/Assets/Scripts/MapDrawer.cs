using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
//using static UnityEditor.Progress;
using Random = UnityEngine.Random;

public class hCube
{
    public Vector4 start;
    public Vector4 end;
    public Vector4 size;
    public Vector4 centre;
    //public float high = 0; later work

    public hCube(Vector4 position, Vector4 size)
    {
        start = position;
        end = position + size;

        this.size = size;
        centre = new Vector4((int)(position.x + size.x / 2), (int)(position.y + size.y / 2),
            (int)(position.z + size.z / 2), (int)(position.w + size.w / 2));
    }

    public Vector4 randomPos()
    {
        List<Vector4> list = MapDrawer.hCubePositions(this);
        return list[Random.Range(0, list.Count)];
    }
}

public class MapDrawer : MonoBehaviour
{
    public static MapDrawer Instance;

    public int gridSize;

    public Tilemap tilemap;
    public Tile floorTile;
    public Tilemap walls;
    public TileBase wallTile;

    public Vector4 mapSize;
    public Vector4 maxRoomSize;
    public float maxRooms;
    public bool haveIntersectsRooms;

    public bool doEasyRoomsRandom;
    public int easyRoomsLowType, easyRoomsHighType; // Easy rooms random (2D rooms = 0-5, 3D r8oms = 6-9, 4D r88ms = 10)
    public int roomRate, r8omRate, r88mRate; // 2D,3D,4D room rate

    HashSet<Vector4> floorTiles = new HashSet<Vector4>();
    List<hCube> rooms = new List<hCube>();
    FourDPlayer plr;
    Stair endStair;

    void Start()
    {
        print("MapDrawerStart");
        plr = FindObjectOfType<FourDPlayer>();

        Instance = this;

        makeMap();
        UpdateScreen();
    }

    void Update()
    {
        tilemap.layoutGrid.cellSize = new Vector3(1f / gridSize, 1f / gridSize, 0);
    }

    public void UpdateScreen()
    {
        tilemap.ClearAllTiles();
        walls.ClearAllTiles();

        foreach (var tile in floorTiles)
        {
            var pos = FourToTwo(tile);
            pos = new Vector3Int(pos.x, pos.y, 0);

            if (Within5(tile, plr.position))
            { tilemap.SetTile(pos, floorTile); }
        }

        foreach (var tile in floorTiles)
        {
            if (Within7(tile, plr.position))
            {
                foreach (var item in GetNeighbours(tile))
                {
                    if (Within5(item, plr.position))
                    {
                        var posW = FourToTwo(item);
                        posW = new Vector3Int(posW.x, posW.y, 0);

                        if (tilemap.GetTile(posW) == null)
                            walls.SetTile(posW, wallTile);
                    }
                }
            }
        }
    }


    public Vector3Int FourToTwo(Vector4 v4)
    { return new Vector3Int((int)v4.x + (int)v4.w * gridSize, (int)v4.y + (int)v4.z * gridSize, 0); }

    public bool Within5(Vector4 pos, Vector4 plr)
    {
        int a = Mathf.FloorToInt(gridSize / 2) + 1;

        return (pos.x < plr.x + a && pos.x > plr.x - a &&
            pos.y < plr.y + a && pos.y > plr.y - a &&
            pos.z < plr.z + a && pos.z > plr.z - a &&
            pos.w < plr.w + a && pos.w > plr.w - a);
    }

    bool Within7(Vector4 pos, Vector4 plr)
    {
        int a = Mathf.FloorToInt(gridSize / 2) + 3;

        return (pos.x < plr.x + a && pos.x > plr.x - a &&
            pos.y < plr.y + a && pos.y > plr.y - a &&
            pos.z < plr.z + a && pos.z > plr.z - a &&
            pos.w < plr.w + a && pos.w > plr.w - a);
    }

    public static List<Vector4> hCubePositions(hCube cube)
    {
        List<Vector4> positions = new List<Vector4>();

        for (int x = (int)cube.start.x; x < cube.end.x; x++)
        {
            for (int y = (int)cube.start.y; y < cube.end.y; y++)
            {
                for (int z = (int)cube.start.z; z < cube.end.z; z++)
                {
                    for (int w = (int)cube.start.w; w < cube.end.w; w++)
                    {
                        positions.Add(new Vector4(x, y, z, w));
                    }
                }
            }
        }

        return positions;
    }

    public hCube makeMapMini()
    {

        float rX = Random.Range(2, maxRoomSize.x);
        float rY = Random.Range(2, maxRoomSize.y);
        float rZ = Random.Range(2, maxRoomSize.z);
        float rW = Random.Range(2, maxRoomSize.w);
        float rO = 0;// Random.Range(1, 2); // dimension that don't need to

        // 2 dimensions rooms (normal 2D roguelike rooms)
        Vector4 XYroomSize = new Vector4(rX, rY, rO, rO);
        Vector4 XZroomSize = new Vector4(rX, rO, rZ, rO);
        Vector4 XWroomSize = new Vector4(rX, rO, rO, rW);
        Vector4 YZroomSize = new Vector4(rO, rY, rZ, rO);
        Vector4 YWroomSize = new Vector4(rO, rY, rO, rW);
        Vector4 ZWroomSize = new Vector4(rO, rO, rZ, rW);

        // 3 dimensions roooms (WE CAN WALK IN 3D SPACE)
        Vector4 XYZroomSize = new Vector4(rX, rY, rZ, rO);
        Vector4 XYWroomSize = new Vector4(rX, rY, rO, rW);
        Vector4 XZWroomSize = new Vector4(rX, rO, rZ, rW);
        Vector4 YZWroomSize = new Vector4(rO, rY, rZ, rW);

        // 4 dimensions r88ms (yo beguilingggg 4D r88ms)
        Vector4 room4DSize = new Vector4(rX, rY, rZ, rW);


        // all the different rooms
        Vector4[] size = { XYroomSize, XZroomSize, XWroomSize, YZroomSize, YWroomSize, ZWroomSize, XYZroomSize, XYWroomSize, XZWroomSize, YZWroomSize, room4DSize };
        Vector4 posi = new Vector4(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y), Random.Range(0, mapSize.z), Random.Range(0, mapSize.w));


        // bring the number down
        int eRmsL = MapDrawer.Instance.easyRoomsLowType, eRmsH = MapDrawer.Instance.easyRoomsHighType;
        roomRate = MapDrawer.Instance.roomRate; r8omRate = MapDrawer.Instance.r8omRate; r88mRate = MapDrawer.Instance.r88mRate;


        // easy way (equal weighted random)
        // size[n]:  2D rooms = 0-5, 3D r8oms = 6-9, 4D r88ms = 10
        hCube room = new hCube(posi, size[Random.Range(eRmsL, eRmsH)]);
        // Different floors may have different styles (curse + fun)


        // detail way (weighted random)
        int Random3RoomSet = Random.Range(1, roomRate + r8omRate + r88mRate + 1);
        if (roomRate + r8omRate + r88mRate != 0)
        { // if 0, easy way random; if !0, this detail way of random
            if (Random3RoomSet <= roomRate) { return room = new hCube(posi, size[Random.Range(0, 5)]); }
            else if (Random3RoomSet - roomRate <= r8omRate) { return room = new hCube(posi, size[Random.Range(6, 9)]); }
            else { return new hCube(posi, room4DSize); }
        }

        return room;
    }

    public void makeMap()
    {
        print("maping"); int numRooms = 0;
        for (int i = 0; i < maxRooms; i++)
        {   
            hCube room = makeMapMini(); print("minied room.");

            bool Intersected = false;

            foreach (var item in rooms) {
                if (intersects(item, room)) { Intersected = true; break; }}

            if (Intersected && !haveIntersectsRooms) { print("no intersect!"); i--; }
            else { createRoom(room); print("made room: " + numRooms+1);

                if (numRooms == 0) { plr.position = new Vector4((int)room.centre.x, (int)room.centre.y, (int)room.centre.z, (int)room.centre.w); }
                else { joinRooms(rooms[rooms.Count - 2], rooms[rooms.Count - 1]); print(numRooms+" room linked"); }

            numRooms += 1; print("had made room: " + numRooms);
            }
        } print("all room maped");

        if (StairBuild.Instance.downStairNum > 0) {
            for (int i = 0; i < StairBuild.Instance.downStairNum; i++) {
                StairBuild.Instance.SpawnDownStair(rooms[Random.Range(0,rooms.Count)]);}}
        //else if { StairBuild.Instance.downStairNum == 0 }     Have A The END Stair?

        if (StairBuild.Instance.upStairNum > 0 && SceneManager.GetActiveScene().buildIndex > 0) {
            for (int i = 0; i < StairBuild.Instance.upStairNum; i++) {
                StairBuild.Instance.SpawnUpStair(rooms[Random.Range(0, rooms.Count)]); }}

        foreach (var item in rooms) {
            print("Spawn across these rooms");
            EnemySpawner.Instance.SpawnEnemies(item);
        } print("all Spawned");
    }

    public void Spawn2thEnemies() { EnemySpawner.Instance.SpawnEnemies(rooms[Random.Range(0, rooms.Count)]); print("Repawned"); }

    List<Vector4> GetNeighbours(Vector4 v)
    {
        return new List<Vector4>() { v + new Vector4(-1,1), v + new Vector4(0,1), v + new Vector4(1,1),
            v + new Vector4(-1,0), v + new Vector4(1,0),
            v + new Vector4(1,-1), v + new Vector4(0,-1), v + new Vector4(-1,-1),};
    }

    void createRoom(hCube room)
    {
        rooms.Add(room);
        floorTiles.UnionWith(hCubePositions(room));
    }

    void createRoom(Vector4 pos, Vector4 size)
    {
        var room = new hCube(pos, size);
        createRoom(room);
    }

    Vector4 start;
    hCube room1, room2;

    void joinRooms(hCube r1, hCube r2)
    {
        room1 = r1;
        room2 = r2;

        start = r1.centre;

        var list = new List<Action> { xt, yt, zt, wt };
        list = Shuffle(list);
        list.ForEach(method => method());
    }

    bool intersects(hCube h1, hCube h2)
    {
        return (h1.start.x <= h2.end.x + 1 && h1.end.x >= h2.start.x - 1) &&
            (h1.start.y <= h2.end.y + 1 && h1.end.y >= h2.start.y - 1) &&
            (h1.start.z <= h2.end.z + 1 && h1.end.z >= h2.start.z - 1) &&
            (h1.start.w <= h2.end.w + 1 && h1.end.w >= h2.start.w - 1);
    }

    void xt()
    {
        xTunnel((int)room1.centre.x, (int)room2.centre.x, (int)start.y, (int)start.z, (int)start.w);
        start.x = room2.centre.x;
    }

    void yt()
    {
        yTunnel((int)room1.centre.y, (int)room2.centre.y, (int)start.x, (int)start.z, (int)start.w);
        start.y = room2.centre.y;
    }

    void zt()
    {
        zTunnel((int)room1.centre.z, (int)room2.centre.z, (int)start.x, (int)start.y, (int)start.w);
        start.z = room2.centre.z;
    }

    void wt()
    {
        wTunnel((int)room1.centre.w, (int)room2.centre.w, (int)start.x, (int)start.y, (int)start.z);
        start.w = room2.centre.w;
    }

    void xTunnel(int x1, int x2, int y, int z, int w)
    {
        for (int i = Mathf.Min(x1, x2); i < Mathf.Max(x1, x2) + 1; i++)
        {
            floorTiles.Add(new Vector4(i, y, z, w));
        }
    }

    void yTunnel(int y1, int y2, int x, int z, int w)
    {
        for (int i = Mathf.Min(y1, y2); i < Mathf.Max(y1, y2) + 1; i++)
        {
            floorTiles.Add(new Vector4(x, i, z, w));
        }
    }

    void zTunnel(int z1, int z2, int x, int y, int w)
    {
        for (int i = Mathf.Min(z1, z2); i < Mathf.Max(z1, z2) + 1; i++)
        {
            floorTiles.Add(new Vector4(x, y, i, w));
        }
    }

    void wTunnel(int w1, int w2, int x, int y, int z)
    {
        for (int i = Mathf.Min(w1, w2); i < Mathf.Max(w1, w2) + 1; i++)
        {
            floorTiles.Add(new Vector4(x, y, z, i));
        }
    }

    public static System.Random Rnd = new System.Random();

    public List<T> Shuffle<T>(List<T> list)
    {
        if (list == null)
            throw new ArgumentNullException("list");
        for (int j = list.Count; j >= 1; j--)
        {
            int item = Rnd.Next(0, j);
            if (item < j - 1)
            {
                var t = list[item];
                list[item] = list[j - 1];
                list[j - 1] = t;
            }
        }
        return list;
    }
}


