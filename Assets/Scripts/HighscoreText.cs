using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class HighscoreText : MonoBehaviour
{
    Text highscore;

    void OnEnable() { UpdateHighscore(); }

    void UpdateHighscore() { GetComponent<Text>().text = "Highscore: " + PlayerPrefs.GetInt("Highscore").ToString(); }
}
