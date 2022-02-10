using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonManagement : MonoBehaviour
{

    public Button Btn1, Btn2, Btn3, Btn4, Btn5, Btn6, Btn7, Btn8, Btn9, Btn0, Btn10, Btn11;
    public Text passwordText;
    public AudioSource buttonSource;
    public AudioClip buttonPressedEffect;


    // Start is called before the first frame update
    void Start()
    {
        Btn1.onClick.AddListener(delegate { ButtonClicked("1"); });
        Btn2.onClick.AddListener(delegate { ButtonClicked("2"); });
        Btn3.onClick.AddListener(delegate { ButtonClicked("3"); });
        Btn4.onClick.AddListener(delegate { ButtonClicked("4"); });
        Btn5.onClick.AddListener(delegate { ButtonClicked("5"); });
        Btn6.onClick.AddListener(delegate { ButtonClicked("6"); });
        Btn7.onClick.AddListener(delegate { ButtonClicked("7"); });
        Btn8.onClick.AddListener(delegate { ButtonClicked("8"); });
        Btn9.onClick.AddListener(delegate { ButtonClicked("9"); });
        Btn0.onClick.AddListener(delegate { ButtonClicked("0"); });
        Btn10.onClick.AddListener(delegate { ButtonClicked("*"); });
        Btn11.onClick.AddListener(delegate { ButtonClicked("#"); });

    }

    void ButtonClicked(string buttonNo)
    {
        buttonSource.PlayOneShot(buttonPressedEffect);
        passwordText.text += buttonNo;
    }
}
