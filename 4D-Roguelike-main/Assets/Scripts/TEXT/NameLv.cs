using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NameLv : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0) { text.text = "LEVEL " + SceneManager.GetActiveScene().buildIndex + "\n4Dimensions&Dungeons"; }
    }
}
