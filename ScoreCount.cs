using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCount : MonoBehaviour
{
    public delegate void GameOverDelegate(int totalScore);
    public static event GameOverDelegate GameOver;
    
    public Text ScoreText;
    public int NumberOfPig;
    public int NumberOfBird = 3;
    
    private int TotalScore = 0;


    private void OnEnable()
    {
        PigDamage.PigDied += PigDied;
        BlocksDamage.BrokeBlock += BrokenBlock;
        BirdsControl.BirdDied += BirdDied;
    }

    private void OnDisable()
    {
        BirdsControl.BirdDied -= BirdDied;
        BlocksDamage.BrokeBlock -= BrokenBlock;
        PigDamage.PigDied -= PigDied;
    }


    void PigDied(int pover_of_pig)
    {
        TotalScore += pover_of_pig*5;
        NumberOfPig -= 1;
        if (NumberOfPig == 0)
        {
            TotalScore += NumberOfBird * 10000;
            if (GameOver != null)
            {
                GameOver(TotalScore); 
            }
        }
    }

    void BirdDied()
    {
        NumberOfBird -= 1;
        if (NumberOfBird == 0)
        {
            GameOver(TotalScore);
        }
    }

    void BrokenBlock(int durabilit)
    {
        TotalScore += durabilit * 2;
    }

    private void Update()
    {
        ScoreText.text = TotalScore.ToString();
    }
}
