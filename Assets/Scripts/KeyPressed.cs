﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPressed : MonoBehaviour
{
    bool tipIsSpawned = true;
    bool arrowSpawned = false;
    public float delayTime = 2.2f;
    public GameObject firstEnemy;
    public GameObject secondEnemy;
    public GameObject thirdEnemy;
    public Image firstTip;
    public Image blueEnemyDescription;
    public Image orangeEnemyDescription;
    public Image greenEnemyDescription;
    public GameObject arrow;
    PlayerManager player;
    public Canvas tutorial;

    private void Awake()
    {
        player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.activeInHierarchy)
            DelaySpawn(delayTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(player.countTurn == 1 && tipIsSpawned)
        {
            tipIsSpawned = false;
            DelaySpawn(delayTime);
        }

        try
        {
            if (player.countTurn == 2 && !arrowSpawned)
            {
                arrowSpawned = true;
                firstTip.gameObject.SetActive(false);
                blueEnemyDescription.gameObject.SetActive(true);
                Instantiate(arrow, new Vector3(2f, 3f, 2f), Quaternion.Euler(-182.80f, -290.347f, 276.406f));
                iTween.MoveTo(GameObject.Find("Arrow5(Clone)"), iTween.Hash(
                "y", 3.5f,
                "time", 1f,
                "loopType", iTween.LoopType.pingPong,
                "easetype", iTween.EaseType.linear));
            }
        }
        catch
        {
            //Debug.Log("No porte in questo livello.");
            return;
        }

        if (player.countTurn==3)
        {
            blueEnemyDescription.gameObject.SetActive(false);
            orangeEnemyDescription.gameObject.SetActive(true);
            iTween.MoveTo(GameObject.Find("Arrow5(Clone)"), iTween.Hash(
                "x", 0f,
                "y", 3f,
                "z", 2f,
                "speed", 10f,
                "time", 2.5f,
                "easetype", iTween.EaseType.linear));
        }

        if (player.countTurn == 4)
        {
            orangeEnemyDescription.gameObject.SetActive(false);
            greenEnemyDescription.gameObject.SetActive(true);
            iTween.MoveTo(GameObject.Find("Arrow5(Clone)"), iTween.Hash(
                "x", -2f,
                "y", 3f,
                "z", 0f,
                "speed", 10f,
                "time", 2.5f,
                "easetype", iTween.EaseType.linear));
        }

        if(player.countTurn == 5)
        {
            Destroy(GameObject.Find("Arrow5(Clone)"));
            greenEnemyDescription.gameObject.SetActive(false);
        }
    }

    void DelaySpawn(float delayTime)
    {
        StartCoroutine(DelayAction(delayTime));
    }

    IEnumerator DelayAction(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        //Do the action after the delay time has finished.
        tutorial.gameObject.SetActive(true);
        firstTip.gameObject.SetActive(true);
    }
}
