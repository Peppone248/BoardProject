using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerDetector : MonoBehaviour
{
    Board m_board;
    public Button loginBtn;
    public Canvas wifiCanvas;

    public InputField passwordTyped;
    public InputField usernameTyped;
    public string pswFromField;
    public string usernameFromField;
    string pswSecurityCam = "admin";
    string usernameSecurityCam = "admin";
    public GameObject[] listOfUIelementsToDeactivate;
    public GameObject[] listOfUIelementsToActivate;

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

    private void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_board.StopPlayerOnPC())
        {
           
        }
    }

   public bool buttonCallBack(Button btnPressed)
    {
        if (btnPressed == loginBtn)
        {
            usernameFromField = usernameTyped.text;
            pswFromField = passwordTyped.text;
            if (pswFromField.Equals(pswSecurityCam) && usernameFromField.Equals(usernameSecurityCam))
            {

                for (int i = 0; i < listOfUIelementsToDeactivate.Length; i++)
                {
                    Debug.Log(i.ToString());
                    listOfUIelementsToDeactivate[i].SetActive(false);
                }

                for(int j = 0; j < listOfUIelementsToActivate.Length; j++)
                {
                    Debug.Log(j.ToString());
                    listOfUIelementsToActivate[j].SetActive(true);
                }

                return true;
            }
            else
                return false;
        }

        return false;
        
    }
}
