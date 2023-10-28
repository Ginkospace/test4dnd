using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{   public static EnemySpawner Instance;

    public bool spawned1st = false; // true = enemy can't respawn
    public int maxEnemiesPerRoom;
    public int minEnemiesPerRoom;

    public string MonstersIncrement = "poise"; //changeable

    public GameObject[] enemyPrefabs;
    public int[] weights;

    public List<Enemy> enemies = new List<Enemy>();

    public void Start(){Instance = this;}

    public void MoveEnemies(){ foreach (var enemy in enemies){enemy.Move();}}

    public bool EnemyAtPoint(Vector4 point)
    {   foreach (var enemy in enemies) { if (enemy.position == point) return true;} return false; }

    public Enemy GetEnemy(Vector4 point)
    {   foreach (var enemy in enemies) {if (enemy.position == point) return enemy;} return null;}

    public void KillAllEnemies()
    { foreach (var enemy in enemies) { enemy.combat.TakeDamage(100); } }

    public void SpawnEnemies(hCube room)
    {   int numMonsters = 1; //unassigned local variable must = something

        if (!spawned1st){print("now starting"); spawned1st = true;
            numMonsters = Random.Range(minEnemiesPerRoom, maxEnemiesPerRoom);}
        else switch (MonstersIncrement)
            {   case "decay": numMonsters = Random.Range(0, 1); print("decay spawned"); break;
                case "poise": numMonsters = 1; print("poise respawned"); break;
                case "extra": numMonsters = Random.Range(1, 2); print("extra monsters+"); break;
                case "swell": numMonsters = 2; print("Monsters are going to swell you"); break;
                case "OCEAN": numMonsters = Random.Range(2, 5); print("OCEAN Monsters+++++"); break;
                default: print("Monster die forever");break;
            }

        for (int i = 0; i < numMonsters; i++)
        {   Vector4 pos = room.randomPos();

            int random = Random.Range(1, 100);
            int countWeightedSpawn = 0;
            int behindC = 0;

            for (int j = 0; j < enemyPrefabs.Length; j++)
            {   countWeightedSpawn += weights[j]; print("countWeightedSpawn"+countWeightedSpawn);
                if (random > behindC && random < countWeightedSpawn && !EnemyAtPoint(pos))
                {   Enemy objec = Instantiate(enemyPrefabs[j], transform).GetComponent<Enemy>();
                    objec.gameObject.name = enemyPrefabs[j].name; enemies.Add(objec);
                    objec.position = pos;
                }
                behindC = countWeightedSpawn;
            }
        }
        print("enemies numbers:"+enemies.Count);
    }
}
