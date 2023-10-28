using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class 地窨層 : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0) { text.text = "地窨" + SceneManager.GetActiveScene().buildIndex + "層"; }
    }
}
