using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class master : MonoBehaviour {

    public GameObject box;
    public GameObject point;
    public GameObject savePoint;

    public GameObject UI;

    public TextMeshProUGUI timeUI;
    public TextMeshProUGUI failUI;

    List<GameObject> Colorbox = new List<GameObject>();

    public int CountTime = 15;
    public int boxSize = 20;

    public float Timer = 15;
    public int fail = 0;

    public int maxFail = 5;

    public int Stage = 0;
    public int BreakBox = 0;

    public GameObject WinUI;
    public GameObject LoseUI;
    public TextMeshProUGUI stageUI;
    public GameObject StageOBJ;

    bool gameEnd = false;

    public AudioClip DestorySound;
    public AudioClip FailSound;
    public AudioClip GameOverSound;

    public int mode  = 3;

    public bool Rush = false;


    public int[] RScoreBoard = new int[9];

    public int[] TScoreBoard = new int[9];


    //button
    public GameObject PauseButton;

    public Sprite Pause;
    public Sprite play;


    bool PClick = false;


    public enum Boxcolor{
        red,
        green,
        blue,
    }
    

    void PreLoad()
    {
        for (int i = 0; i < RScoreBoard.Length; i++)
        {
            RScoreBoard[i] = PlayerPrefs.GetInt("Rsocore" + i, RScoreBoard[i]);
        }
        for (int i = 0; i < TScoreBoard.Length; i++)
        {
            TScoreBoard[i] = PlayerPrefs.GetInt("Tsocore" + i, TScoreBoard[i]);
        }
    }

	void Start () {
        PreLoad();
        mode = PlayerPrefs.GetInt("mode", mode);
        Debug.Log("mode == " + mode);
        if (mode == 0)
        {
            Rush = true;
        }
        else
        {
            boxSize = 100;
            Timer = 30;
            Rush = false;
        }
        Create();
    }

    void Update()
    {
        if (Rush == true)
        {
            if (!gameEnd)
            {
                Timer -= Time.deltaTime;
                timeUI.text = "Time " + Timer.ToString("0.0");
                if (Timer <= 0)
                {
                    YouLose();
                }
                if (Colorbox.Count == 0)
                {
                    YouWin();
                }
            }
        }
        else // time Attack
        {
            if (!gameEnd)
            {
                Timer -= Time.deltaTime;
                timeUI.text = "Time \n" + Timer.ToString("0.0");
                if (Timer <= 0)
                {
                    YouLose();
                }
            }
        }
    }

    void Create()
    {
        for (int i = 0; i < boxSize; i++)
        {
            Vector3 V = new Vector3(0,  (i* 100), point.transform.position.z);
            GameObject B = Instantiate(box,V, Quaternion.identity);
            B.transform.SetParent(point.transform,false);
            B.transform.localScale = new Vector3(1, 1, 1);
            Colorbox.Add(B);
            switch (Randomcolor())
            {
                case 0: //red
                    //B.GetComponent<Image>().color = new Color32(255,95,95,255);
                    B.gameObject.GetComponent<Animator>().Play("red");
                    B.GetComponent<mycolor>().color = Boxcolor.red;
                    break;
                case 1: // green
                    B.gameObject.GetComponent<Animator>().Play("green");
                    B.GetComponent<mycolor>().color = Boxcolor.green;
                    break;
                case 2: // blue
                    B.GetComponent<Image>().color = new Color32(136, 136, 255,255);
                    B.gameObject.GetComponent<Animator>().Play("blue");
                    B.GetComponent<mycolor>().color = Boxcolor.blue;
                    break;

                default: // never
                    break;
            }
        }
    }

    public int Randomcolor()
    {
        int color = Random.Range(0, 3);
        return color;
    }

    public void Movedown()
    {
        foreach(GameObject G in Colorbox)
        {
            G.transform.localPosition = new Vector3(G.transform.localPosition.x, G.transform.localPosition.y-100, G.transform.localPosition.z);
        }
    }

    public void UpdateFail()
    {
        failUI.text = "Fail \n" + fail;
        if (Rush == true)
        {
            if (fail >= 5)
            {
                YouLose();
            }
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(FailSound, 0.7f);
        }
        else
        {
            if (fail >= 1)
            {
                YouLose();
            }
        }
    }
    public void UpdateStage()
    {
        StageOBJ.SetActive(true);
        stageUI.gameObject.SetActive(true);
        if (Rush == true)
        {
            stageUI.text = "Stage " + Stage;
        }
        else
        {
            stageUI.text = "BreakBox : " + BreakBox;
        }
    }

    public void closeButton(bool close)
    {
        List<GameObject> Button = new List<GameObject>(GameObject.FindGameObjectsWithTag("Button"));
        foreach (GameObject B in Button)
        {
            B.gameObject.GetComponent<Button>().interactable = close;
        }
    }

    public void YouWin()
    {
        gameEnd = true;
        WinUI.SetActive(true);
        UpdateStage();
        closeButton(false);
    }

    public void YouLose()
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(GameOverSound, 2.0f);
        gameEnd = true;
        LoseUI.SetActive(true);
        if (Rush == true)
        {
            SaveRecord(Stage);
        }
        else
        {
            SaveRecord(BreakBox);
            LoseUI.GetComponent<TextMeshProUGUI>().text = "Game End";
        }
        UpdateStage();
        closeButton(false);

        
    }

    /// <summary>
    /// save
    /// </summary>
    /// <param name="higherscore"></param>

    public void SaveRecord(int higherscore)
    {
        if (Rush == true)
        {
            for (int i = 0; i < RScoreBoard.Length; i++)
            {
                if (higherscore >= RScoreBoard[i])
                {
                    Debug.Log("move down");
                    MoveDownD(i, higherscore);
                    SaveData();
                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < TScoreBoard.Length; i++)
            {
                if (higherscore >= TScoreBoard[i])
                {
                    Debug.Log("move down");
                    MoveDownD(i, higherscore);
                    SaveData();
                    return;
                }
            }
        }
    }

    void SaveData()
    {
        if (Rush  == true)
        {
            for (int i = 0; i < RScoreBoard.Length; i++)
            {
                PlayerPrefs.SetInt("Rsocore" + i, RScoreBoard[i]);
                
            }
        }
        else
        {
            for (int i = 0; i < TScoreBoard.Length; i++)
            {
                PlayerPrefs.SetInt("Tsocore" + i, TScoreBoard[i]);
            }
        }
        PlayerPrefs.Save();
    }

    void MoveDownD(int numberofscore, int score)
    {
        if (Rush == true)
        {
            for (int i = RScoreBoard.Length - 1; i >= 1; i--)
            {
                RScoreBoard[i] = RScoreBoard[i - 1];
            }
            RScoreBoard[numberofscore] = score;
        }
        else
        {
            for (int i = TScoreBoard.Length - 1; i >= 1; i--)
            {
                TScoreBoard[i] = TScoreBoard[i - 1];
            }
            TScoreBoard[numberofscore] = score;
        }
    }
    /// <summary>
    /// save 
    /// </summary>
    /// <param name="G"></param>


    public void RemoveBox(GameObject G)
    {
        BreakBox += 1;
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(DestorySound, 0.7f);
        Destroy(G);
        Colorbox.Remove(G);
        Movedown();
    }

    public void RedClick()
    {
        if (Colorbox.Count != 0)
        {
            if (Colorbox[0].GetComponent<mycolor>().color == Boxcolor.red)
            {
                RemoveBox(Colorbox[0]);
            }
            else
            {
                fail += 1;
                UpdateFail();
            }
        }
       
    }

    public void GreenClick()
    {
        if (Colorbox.Count != 0)
        {
            if (Colorbox[0].GetComponent<mycolor>().color == Boxcolor.green)
            {
                RemoveBox(Colorbox[0]);
            }
            else
            {
                fail += 1;
                UpdateFail();
            }
        }
       
    }
    public void BlueClick()
    {

        if (Colorbox[0].GetComponent<mycolor>().color == Boxcolor.blue)
        {
            RemoveBox(Colorbox[0]);
        }
        else
        {
            fail += 1;
            UpdateFail();
        }
    }

    public void Nextstage()
    {

        fail = 0;
        Stage += 1;
        Timer += 8;
        boxSize += 5;
        gameEnd = false;
        WinUI.SetActive(false);
        StageOBJ.SetActive(false);
        stageUI.gameObject.SetActive(false);
        closeButton(true);
        point.transform.position = savePoint.transform.position;
        Colorbox.Clear();
        Create();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(0);
    }

    public void PauseClick()
    {
        if (PClick == true)
        {
            PClick = false;
            PauseButton.GetComponent<Image>().sprite = Pause;
            Time.timeScale = 1;
            closeButton(true);
        }
        else
        {
            PClick = true;
            PauseButton.GetComponent<Image>().sprite = play;
            Time.timeScale = 0;
            closeButton(false);
        }
    }
}
