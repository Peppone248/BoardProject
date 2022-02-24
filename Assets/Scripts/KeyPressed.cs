using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPressed : MonoBehaviour
{
    bool tipIsSpawned;
    bool arrowSpawned = false;
    public float delayTime = 2f;
    public GameObject firstEnemy;
    public GameObject secondEnemy;
    public GameObject thirdEnemy;
    public Image firstTip;
    public Image interactionObjTip;
    public Image blueEnemyDescription;
    public Image orangeEnemyDescription;
    public Image greenEnemyDescription;
    public Image scoreDescription;
    public GameObject arrow;
    public GameObject arrow2;
    public Canvas tutorial;
    public GameObject intro;

    PlayerManager player;
    GameManager gm;

    private void Awake()
    {
        player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        gm = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.activeInHierarchy)
        {
            DelaySpawn(delayTime);
            tipIsSpawned = true;
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if(player.countTurn == 0 && !arrowSpawned && !intro.activeInHierarchy)
        {
            tipIsSpawned = false;
            arrowSpawned = true;
            firstTip.gameObject.SetActive(true);
            Instantiate(arrow, new Vector3(6f, 3f, 0f), Quaternion.identity);
            iTween.RotateBy(GameObject.Find("Pointer(Clone)"), iTween.Hash(
                "y", 360f,
                "looptype", iTween.LoopType.loop,
                "speed", 65f,
                "time", 2f,
                "easetype", iTween.EaseType.linear));
        }

        if (player.countTurn == 0 && intro.activeInHierarchy)
        {
            player.playerInput.InputEnabled = false;
        }

            try
        {

            if (player.countTurn == 1 && arrowSpawned)
            {
                arrowSpawned = false;
                Destroy(GameObject.Find("Pointer(Clone)"));
                firstTip.gameObject.SetActive(false);
                blueEnemyDescription.gameObject.SetActive(true);
                Instantiate(arrow2, new Vector3(2f, 3f, 2f), Quaternion.identity);
                iTween.RotateBy(GameObject.Find("Pointer 1(Clone)"), iTween.Hash(
                "y", 360f,
                "looptype", iTween.LoopType.loop,
                "speed", 65f,
                "time", 2f,
                "easetype", iTween.EaseType.linear));
            }
        }
        catch
        {
            return;
        }

        if (player.countTurn==2 && !arrowSpawned)
        {
            arrowSpawned = true;
            Destroy(GameObject.Find("Pointer 1(Clone)"));
            blueEnemyDescription.gameObject.SetActive(false);
            orangeEnemyDescription.gameObject.SetActive(true);
            Instantiate(arrow, new Vector3(0f, 3f, 2f), Quaternion.identity);
            iTween.RotateBy(GameObject.Find("Pointer(Clone)"), iTween.Hash(
                "y", 360f,
                "looptype", iTween.LoopType.loop,
                "speed", 65f,
                "time", 2f,
                "easetype", iTween.EaseType.linear));
        }

        if (player.countTurn == 3 && arrowSpawned)
        {
            arrowSpawned = false;
            Destroy(GameObject.Find("Pointer(Clone)"));
            orangeEnemyDescription.gameObject.SetActive(false);
            greenEnemyDescription.gameObject.SetActive(true);
            Instantiate(arrow2, new Vector3(-2f, 3f, 0f), Quaternion.identity);
            iTween.RotateBy(GameObject.Find("Pointer 1(Clone)"), iTween.Hash(
               "y", 360f,
               "looptype", iTween.LoopType.loop,
               "speed", 65f,
               "time", 2f,
               "easetype", iTween.EaseType.linear));

        }

        if (player.countTurn == 4 && !arrowSpawned)
        {
            arrowSpawned = true;
            Destroy(GameObject.Find("Pointer 1(Clone)"));
            greenEnemyDescription.gameObject.SetActive(false);
            interactionObjTip.gameObject.SetActive(true);
            Instantiate(arrow, new Vector3(0f, 2.8f, 5f), Quaternion.identity);
            iTween.RotateBy(GameObject.Find("Pointer(Clone)"), iTween.Hash(
               "y", 360f,
               "looptype", iTween.LoopType.loop,
               "speed", 65f,
               "time", 2f,
               "easetype", iTween.EaseType.linear));
            
        }

        if (player.countTurn == 5 && arrowSpawned)
        {
            arrowSpawned = false;
            Destroy(GameObject.Find("Pointer(Clone)"));
            Instantiate(arrow2, new Vector3(-4f, 2.5f, 2f), Quaternion.identity);
            iTween.RotateBy(GameObject.Find("Pointer 1(Clone)"), iTween.Hash(
               "y", 360f,
               "looptype", iTween.LoopType.loop,
               "speed", 65f,
               "time", 2f,
               "easetype", iTween.EaseType.linear));
            interactionObjTip.gameObject.SetActive(false);
            scoreDescription.gameObject.SetActive(true);
        }

        if (player.countTurn == 6 && !arrowSpawned)
        {
            arrowSpawned = true;
            scoreDescription.gameObject.SetActive(false);
            Destroy(GameObject.Find("Pointer 1(Clone)"));
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
    }
}
