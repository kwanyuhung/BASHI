using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class startUI : MonoBehaviour {


    public GameObject StartUI;


    public int IsSave = 0;
    public int[] RScoreBoard = new int[9];

    public int[] TScoreBoard = new int[9];

    public TextMeshProUGUI ScoreHeader;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI ScoreButtonText;

    bool ScoreSelectClick = true;

    public void UpdateScoreUI(bool rush)
    {
        ScoreText.text = "";
        if (rush)
        {
            ScoreHeader.text = "Rush";
            for(int i = 0; i< RScoreBoard.Length; i++)
            {
                ScoreText.text += (i + 1) + ")  Stage " + RScoreBoard[i] + "\n";
            }
            IsSave = 1;
        }
        else
        {
            ScoreHeader.text = "Time Attack";
            for (int i = 0; i < TScoreBoard.Length; i++)
            {
                ScoreText.text += (i + 1) + ")  Break " + TScoreBoard[i] + "\n";
            }
        }
        if(ScoreSelectClick == false)
        {
            ScoreButtonText.text = "Rush";
        }
        else
        {
            ScoreButtonText.text = "TimeAttack";
        }
    }


    public void Fristtimesave()
    {
        if (PlayerPrefs.GetInt("Save ed", IsSave) == 0)
        {
            Debug.Log("Not Save Yet");
            for (int i = 0; i < RScoreBoard.Length; i++)
            {
                RScoreBoard[i] = 0;
                PlayerPrefs.SetInt("Rsocore" + i, RScoreBoard[i]);
            }
            for (int i = 0; i < TScoreBoard.Length; i++)
            {
                TScoreBoard[i] = 0;
                PlayerPrefs.SetInt("Tsocore" + i, TScoreBoard[i]);
            }
            IsSave = 1;
            PlayerPrefs.SetInt("Save ed", IsSave);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("save ed");
            for (int i = 0; i < RScoreBoard.Length; i++)
            {
                RScoreBoard[i] = PlayerPrefs.GetInt("Rsocore" + i, RScoreBoard[i]);
            }
            for (int i = 0; i < TScoreBoard.Length; i++)
            {
                TScoreBoard[i] = PlayerPrefs.GetInt("Tsocore" + i, TScoreBoard[i]);
            }
        }
    }

    public void Start()
    {
        Fristtimesave();
    }




    void SelectRush() //scoreboard
    {
        UpdateScoreUI(true);
    }

    void SelectTA() //scoreboard
    {
        UpdateScoreUI(false);
    }





    /// <summary>
    /// below is button 
    /// </summary>
    ///

    public void GoToRush()
    {
        PlayerPrefs.SetInt("mode", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }

    public void GoToTimeAttack()
    {
        PlayerPrefs.SetInt("mode", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }

    public void ScoreChange()
    {

        ScoreSelectClick = !ScoreSelectClick;
        if (ScoreSelectClick == true)
        {
            SelectRush();
        }
        else
        {
            SelectTA();
        }
    }


    public void MoveLeft()  //option
    {
        StartUI.GetComponent<Animation>().Play("move left");
    }

    public void MoveRight() //socreBoard
    {
        ScoreChange();
        StartUI.GetComponent<Animation>().Play("move right");
    }

    public void LeftplayBack()
    {
        StartUI.GetComponent<Animation>().Play("playback left");
    }


    public void RightplayBack()
    {
        StartUI.GetComponent<Animation>().Play("playback right");
    }

    public void RushMode() //socreBoard
    {
        ScoreSelectClick = true;
        UpdateScoreUI(true);
    }

    public void TimeAttackMode() //socreBoard
    {
        ScoreSelectClick = false;
        UpdateScoreUI(false);
    }
}
