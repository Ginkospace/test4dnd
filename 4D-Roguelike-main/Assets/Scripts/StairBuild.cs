using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairBuild : MonoBehaviour
{   public static StairBuild Instance;

    public int upStairNum, downStairNum;
    public GameObject[] StairPrefabs;

    public List<Stair> upStairs = new List<Stair>();
    public List<Stair> downStairs = new List<Stair>();
    public void Start(){Instance = this;}
    public bool TouchUpStair(Vector4 point) { 
        foreach (var eachStairs in upStairs) { if (eachStairs.position == point) return true; } return false; }
    public bool TouchDownStair(Vector4 point) {
        foreach (var eachStairs in downStairs) { if (eachStairs.position == point) return true; } return false; }

    //public Stair GetStair(Vector4 point)

    public void SpawnUpStair(hCube room) {
        Stair newStair = Instantiate(StairPrefabs[0], transform).GetComponent<Stair>(); upStairs.Add(newStair);
        newStair.position = room.randomPos();
    }
    public void SpawnDownStair(hCube room) {
        Stair newStair = Instantiate(StairPrefabs[1], transform).GetComponent<Stair>(); downStairs.Add(newStair);
        newStair.position = room.randomPos();
    }
}