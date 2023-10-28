using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    public Vector4 position;
    public SpriteRenderer sprite;
    public FourDPlayer plr;
    MapDrawer gridSize;
    void Start() { gridSize = FindObjectOfType<MapDrawer>(); plr = FindObjectOfType<FourDPlayer>(); }

    void Update()
    {
        transform.position = new Vector3(position.x / gridSize.gridSize + position.w, position.y / gridSize.gridSize + position.z, 0);
        sprite.color = gridSize.Within5(position, plr.position) ? Color.white : new Color(255, 255, 255, 0);
    }

}
