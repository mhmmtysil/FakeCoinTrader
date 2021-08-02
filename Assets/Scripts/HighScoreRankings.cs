using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HighScoreRankings : MonoBehaviour
{
    public TextMeshProUGUI[] rankingNumber;
    public TextMeshProUGUI[] rankingUsername;
    public TextMeshProUGUI[] rankingHighScore;
    public TextMeshProUGUI usernameText;
    public Color myRankingColor;
    public Color white;

    public void ChangeColor()
    {
        for (int i = 0; i < 10; i++)
        {
            if (rankingUsername[i].text == usernameText.text)
            {
                rankingNumber[i].color = myRankingColor;
                rankingUsername[i].color = myRankingColor;
                rankingHighScore[i].color = myRankingColor;
            }
            else
            {
                rankingNumber[i].color = white;
                rankingUsername[i].color = white;
                rankingHighScore[i].color = white;
            }
        }
    }
}
