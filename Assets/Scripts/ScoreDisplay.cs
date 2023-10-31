using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    private TMP_Text text;
    private int score;
    public FlowerController flowerRed;
    public FlowerController flowerBlue;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flowerRed.score > flowerBlue.score)
            score = flowerRed.score;
        else
            score = flowerBlue.score;
        text.text = "Score: " + score.ToString();
    }
}
