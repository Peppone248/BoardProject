using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalDetector : MonoBehaviour
{
    Board m_board;

    public Canvas terminalCanvas;
    public InputField nameTyped;
    public InputField surnameTyped;
    public InputField emailTyped;
    public Button loginBtn;

    public GameObject[] enemiesPatrol;
    public GameObject[] enemiesSent;
    public GameObject[] hitmanEnemySent;
    public GameObject[] hitmanEnemyPatrol;
    public GameObject[] listOfUIelementsToDeactivate;
    public GameObject[] listOfUIelementsToActivate;
    public GameObject pauseBtn;

    public AudioSource alarmSource;
    public AudioClip alarmEffect;

    private string nameFromField;
    private string surnameFromField;
    private string emailFromField;
    private bool isSpoofed = false;

    public bool IsSpoofed { get => isSpoofed; set => isSpoofed = value; }

    private void OnEnable()
    {
        try
        {
            loginBtn.onClick.AddListener(() => buttonCallBack(loginBtn));
        }
        catch
        {
            return;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (m_board.StopPlayerOnTerminal())
        {

        }
    }


    private void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
    }


    public bool buttonCallBack(Button btnPressed)
    {
        if (btnPressed == loginBtn)
        {
            nameFromField = nameTyped.text;
            surnameFromField = surnameTyped.text;
            emailFromField = emailTyped.text;

            if (nameFromField.Length > 0 && surnameFromField.Length > 0 && emailFromField.Contains("@"))
            {
                pauseBtn.SetActive(true);
                isSpoofed = true;
                terminalCanvas.gameObject.SetActive(false);
                enemiesPatrol[0].GetComponent<Renderer>().enabled = false;
                enemiesPatrol[1].GetComponent<Renderer>().enabled = false;
                enemiesPatrol[2].GetComponent<Renderer>().enabled = false;
                enemiesSent[0].GetComponent<Renderer>().enabled = false;
                hitmanEnemySent[0].GetComponent<Renderer>().enabled = true;
                hitmanEnemyPatrol[0].GetComponent<Renderer>().enabled = true;
                hitmanEnemyPatrol[1].GetComponent<Renderer>().enabled = true;
                hitmanEnemyPatrol[2].GetComponent<Renderer>().enabled = true;

                for (int i = 0; i < listOfUIelementsToDeactivate.Length; i++)
                {
                    listOfUIelementsToDeactivate[i].SetActive(false);
                }

                for (int j = 0; j < listOfUIelementsToActivate.Length; j++)
                {
                    listOfUIelementsToActivate[j].SetActive(true);
                }

                alarmSource.volume = 0.09f;
                alarmSource.PlayOneShot(alarmEffect);
                
                return true;
            }
            else
                return false;
        }
        return false;
    }

}