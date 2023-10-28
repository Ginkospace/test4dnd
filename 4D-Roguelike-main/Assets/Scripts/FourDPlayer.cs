using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public class KeyAxis{public KeyCode n, p;}

//public class KeyAxis2{public KeyCode nn, np, pn, pp;}
//public class UniMoveWay {public KeyCode nnnn, nnnp, nnpn, nnpp, npnn, npnp, nppn, nppp, pnnn, pnnp, pnpn, pnpp, ppnn, ppnp, pppn, pppp;}

/*[System.Serializable]
public class TiltedAxis { public KeyCode n, p; }*/

[System.Serializable]
public class FourDkeys
{   public KeyAxis X, Y, Z, W; //straight 4D moving axis
    /*public KeyAxis2 XY, XZ, XW, YZ, YW, ZW; //tilted 4D moving axis
    public KeyAxis3 XYZ, XYW, XZW, YZW; //tilted 4D moving axis
    public KeyAxis4 XYZW; //tilted 4D moving axis √4 = 2 */
}

public class FourDPlayer : MonoBehaviour
{
    public Combat combat;
    public CameraFollow follow;

    [Header("Movement")]
    public Vector4 position;
    public FourDkeys moveKeys, moveKeysPlus;//additional moving keys
    bool bPlus, bEQCZ, bQZEC, bIJKL;//check what type of move
    public FourDkeys EQCZ_moveSet;//EQCZ/Left Hunch moving keys
    public FourDkeys QZEC_moveSet;//QZEC/Left Rhaskia moving keys
    public FourDkeys IJKL_moveSet;//IJKL/Igor Galochkin moving keys
    public Tilemap tilemap;
    public Tile wallTile;

    public GameObject OpenChat;
    public GameObject[] colorSquareXY_ZW;
    public int playColorSquare;
    
    public Vector4 x1, y1, z1, w1; //orthodoxy 4D moving 1 square

    MapDrawer gridSize;

    [Header("Sprites")]
    public SpriteRenderer sprite;
    public Sprite deathSprite;
    bool dead;
    public GameObject deadRed;

    void Start()
    {   gridSize = FindObjectOfType<MapDrawer>();
        moveKeys = EQCZ_moveSet; bPlus = bEQCZ = true; bQZEC = bIJKL = false;
        Audio.instance.playBGM(Random.Range(1, Audio.instance.BGM.Length+1));
    }

    void Update()
    {
        ChatManager();
        if (OpenChat.active) return;
        GobleInputManager();
        if (deadRed.active) return;
        if (dead) { deadRed.SetActive(true); sprite.sprite = deathSprite; }
        InputManager();
        transform.position = new Vector3(position.x / gridSize.gridSize + position.w, position.y / gridSize.gridSize + position.z, 0);
    }

    public void InputManager()
    {   if (!Input.GetKey(KeyCode.LeftShift))
        {   if (Input.GetKeyDown(moveKeys.X.p)) { Move(x1); }
            if (Input.GetKeyDown(moveKeys.X.n)) { Move(-x1); }

            if (Input.GetKeyDown(moveKeys.Y.p)) { Move(y1); }
            if (Input.GetKeyDown(moveKeys.Y.n)) { Move(-y1); }

            if (Input.GetKeyDown(moveKeys.Z.p)) { Move(z1); }
            if (Input.GetKeyDown(moveKeys.Z.n)) { Move(-z1); }

            if (Input.GetKeyDown(moveKeys.W.p)) { Move(w1); }
            if (Input.GetKeyDown(moveKeys.W.n)) { Move(-w1); }

            if (Input.GetKeyDown(moveKeysPlus.X.p)) { Move(x1); }
            if (Input.GetKeyDown(moveKeysPlus.X.n)) { Move(-x1); }

            if (Input.GetKeyDown(moveKeysPlus.Y.p)) { Move(y1); }
            if (Input.GetKeyDown(moveKeysPlus.Y.n)) { Move(-y1); }

            if (Input.GetKeyDown(moveKeysPlus.Z.p)) { Move(z1); }
            if (Input.GetKeyDown(moveKeysPlus.Z.n)) { Move(-z1); }

            if (Input.GetKeyDown(moveKeysPlus.W.p)) { Move(w1); }
            if (Input.GetKeyDown(moveKeysPlus.W.n)) { Move(-w1); }
        }
/*        else{
            if (Input.GetKeyDown(moveKeys.X.p)) { follow.LookFarXr(); }
            if (Input.GetKeyDown(moveKeys.X.n)) { follow.LookFarXl(); }

            if (Input.GetKeyDown(moveKeys.Y.p)) { follow.LookFarYr(); }
            if (Input.GetKeyDown(moveKeys.Y.n)) { follow.LookFarYl(); }

            if (Input.GetKeyDown(moveKeys.Z.p)) { follow.LookFarZr(); }
            if (Input.GetKeyDown(moveKeys.Z.n)) { follow.LookFarZl(); }

            if (Input.GetKeyDown(moveKeys.W.p)) { follow.LookFarWr(); }
            if (Input.GetKeyDown(moveKeys.W.n)) { follow.LookFarWl(); }
        }

        if (Input.GetKeyDown(KeyCode.Alpha6)) { CameraFollow.Instance.LookFarXr(); }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { follow.LookFarXl(); }

        if (Input.GetKeyUp(KeyCode.LeftShift)) { follow.LookBack(); }*/

        if (Input.GetKeyDown(KeyCode.X)) { EnemySpawner.Instance.MoveEnemies(); Rest1(); }
        if (Input.GetKeyDown(KeyCode.Y)) { combat.AllGood(); }

        if (Input.GetKeyDown(KeyCode.Alpha4)) { combat.healing = 4; }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { combat.maxHP = 50; }
    }

    public void GobleInputManager()
    {   if (Input.GetKeyDown(KeyCode.G)) { RestartGame0(); Audio.instance.StopBGM(); }
        
        if (Input.GetKeyDown(KeyCode.Minus)) { Audio.instance.playBGM(-1); }
        if (Input.GetKeyDown(KeyCode.Equals)) { Audio.instance.playBGM(1); }
        if (Input.GetKeyDown(KeyCode.Backspace)) { Audio.instance.StopBGM(); }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            moveKeys = EQCZ_moveSet;
            if (bIJKL) { moveKeysPlus = new FourDkeys(); }
            bEQCZ = true; bPlus = bQZEC = bIJKL = false;
            木.AddLine("以單手<color=#22FF22>WASD+EQCZ</color>移動！");
            Log.AddLine("Left Hunch moving keys: <color=#22FF22>WASD+EQCZ</color>");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            moveKeys = QZEC_moveSet;
            if (bIJKL) { moveKeysPlus = new FourDkeys(); }
            bQZEC = true; bPlus = bEQCZ = bIJKL = false;
            木.AddLine("以單手<color=#22FF22>WASD+QZEC</color>移動！");
            Log.AddLine("Left Hunch moving keys: <color=#22FF22>WASD+QZEC</color>");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            moveKeys = IJKL_moveSet;
            if (bIJKL) { moveKeysPlus = new FourDkeys(); }
            bIJKL = true; bPlus = bEQCZ = bQZEC = false;
            木.AddLine("以<color=#22FF22>WASD+IJKL</color>移動！");
            Log.AddLine("Igor Galochkin moving keys: <color=#22FF22>WASD+IJKL</color>");
        }

        if (Input.GetKeyDown(KeyCode.M)) { MapDrawer.Instance.Spawn2thEnemies(); }
        
        if (Input.GetKeyDown(KeyCode.Alpha8)) { EnemySpawner.Instance.KillAllEnemies(); }
        if (Input.GetKeyDown(KeyCode.Alpha9)) { combat.AddFightdetail(); }
        if (Input.GetKeyDown(KeyCode.Alpha0)) { combat.Add詳鬥(); }

        if (Input.GetKeyDown(KeyCode.LeftBracket)) { if (colorSquareXY_ZW[0].active) { colorSquareXY_ZW[0].SetActive(false); } else { colorSquareXY_ZW[0].SetActive(true); } playColorSquare = 0; }
        if (Input.GetKeyDown(KeyCode.RightBracket)) { if (colorSquareXY_ZW[1].active) { colorSquareXY_ZW[1].SetActive(false); } else { colorSquareXY_ZW[1].SetActive(true); } playColorSquare = 1; }
        if (Input.GetKeyDown(KeyCode.Semicolon)) { if (colorSquareXY_ZW[2].active) { colorSquareXY_ZW[2].SetActive(false); } else { colorSquareXY_ZW[2].SetActive(true); } playColorSquare = 2; }
        if (Input.GetKeyDown(KeyCode.Backslash)) { if (colorSquareXY_ZW[3].active) { colorSquareXY_ZW[3].SetActive(false); } else { colorSquareXY_ZW[3].SetActive(true); } playColorSquare = 3; }
        if (Input.GetKeyDown(KeyCode.Comma)) { if (colorSquareXY_ZW[playColorSquare * 2 + 4].active) { colorSquareXY_ZW[playColorSquare * 2 + 4].SetActive(false); } else { colorSquareXY_ZW[playColorSquare * 2 + 4].SetActive(true); } }
        if (Input.GetKeyDown(KeyCode.Period)) { if (colorSquareXY_ZW[playColorSquare * 2 + 5].active) { colorSquareXY_ZW[playColorSquare * 2 + 5].SetActive(false); } else { colorSquareXY_ZW[playColorSquare * 2 + 5].SetActive(true); } }
        if (Input.GetKeyDown(KeyCode.Slash)) { if (colorSquareXY_ZW[playColorSquare].active) { colorSquareXY_ZW[playColorSquare].SetActive(false); } else { colorSquareXY_ZW[playColorSquare].SetActive(true); } }}
    public void ChatManager(){if (Input.GetKeyDown(KeyCode.V)) { if (OpenChat.active) { OpenChat.SetActive(false); } else { OpenChat.SetActive(true); } }}
    void Move(Vector4 newPosition)
    {   if (EnemySpawner.Instance.EnemyAtPoint(position + newPosition)) {
            combat.Attack(EnemySpawner.Instance.GetEnemy(position + newPosition).combat); }
        else if (StairBuild.Instance.TouchDownStair(position + newPosition)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); Audio.instance.Play("stair"); print("down level");
            Log.AddLine("you need key to go down"); 木.AddLine("你要用鑰匙才能下去。");}
        else if (StairBuild.Instance.TouchUpStair(position + newPosition) && SceneManager.GetActiveScene().buildIndex != 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1); Audio.instance.Play("stair"); print("up level");}
        else if (tilemap.GetTile(FourToTwo(position + newPosition)) != null) {
            position += newPosition; gridSize.UpdateScreen(); combat.WalkHealing(); }
        EnemySpawner.Instance.MoveEnemies();}
    public void RestartGame() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); print("RestartGame"); }
    public void RestartGame0() { SceneManager.LoadScene(0); print("RestartGame0"); }
    public Vector3Int FourToTwo(Vector4 v4){ return new Vector3Int((int)v4.x + (int)v4.w * gridSize.gridSize, (int)v4.y + (int)v4.z * gridSize.gridSize, 0);}
    void Rest1() { combat.Rest(); }
    public void Death()
    {   dead = true;
        sprite.sprite = deathSprite;

        Log.AddLine("You Died...");
    }//unused

    

    
}
