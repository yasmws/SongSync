using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = ScoreManager.Instance.score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
