using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Log : MonoBehaviour
{
    public static Log Instance;
    public string text;
    public TextMeshProUGUI front;

    void Start()
    {
        Instance = this;
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            text = "ground floor\n\n use <color=#22FF22>wasd+eqcz</color> to move in this four dimensions dungeon!\n\n additionally can also using the <color=#22FF22>arrow keys+ijkl</color> to move!\n\n press <color=#22FF22>x</color> for rest\n press <color=#22FF22>g</color> new game\n press <color=#22FF22>m</color> more mobs\n\n more useless keys are in itch.io description";
        } else { text = "you came to b" + SceneManager.GetActiveScene().buildIndex+ "floor \n\n press <color=#22FF22>x</color> for rest\n press <color=#22FF22>g</color> new game\n press <color=#22FF22>m</color> more mobs\n\n more keys are in itch.io description"; }
    }
    void Update(){front.text = text;}
    public static void AddLine(string line){Instance.text = line.ToLower() + "\n\n" + Instance.text;}
}
