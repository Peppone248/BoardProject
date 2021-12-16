using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Board : MonoBehaviour
{

    public static float spacing = 2f;
    public static readonly Vector2[] directions =
    {
        new Vector2(spacing, 0f),
        new Vector2(-spacing, 0f),
        new Vector2(0f, spacing),
        new Vector2(0f, -spacing)
    };

    List<Node> m_allNodes = new List<Node>();
    public List<Node> AllNodes { get { return m_allNodes; } }

    Node m_playerNode;
    public Node PlayerNode { get { return m_playerNode; } }
    Node m_goalNode;
    Node doorNode;
    Node doubleDoorNode;
    Node computerNode;
    Node keyNode;
    Node terminalNode;
    Node jacketNode;
    public Node GoalNode { get { return m_goalNode; } }

    public GameObject goalPrefab;
    public GameObject doorPrefab;
    public GameObject doubleDoorPrefab;
    public GameObject computerPrefab;
    public GameObject metalDoorPrefab;
    public GameObject keyLockPrefab;
    public GameObject terminalPrefab;
    public GameObject enemyJacketPrefab;
    public GameObject[] enemiesSent;
    public GameObject[] enemiesPatrol;
    public GameObject hitmanEnemyStat;
    public GameObject[] hitmanEnemySent;
    public GameObject[] hitmanEnemyPatrol;
    public GameObject mainCamera;
    public GameObject retroCamera;
    public GameObject lateralCam;
    public GameObject cameraFirstRoom;
    public GameObject cameraSecondRoom;
    public Light[] pointLight;

    public float countAttemptsCredentials = 0;
    bool keySpawned = false;
    bool jacketSpawned = false;
    bool close = false;
    float drawGoalTime = 1f;
    float drawGoalDelay = 0.3f;
    public iTween.EaseType drawGoalEaseType = iTween.EaseType.easeOutExpo;
    PlayerMover m_player;
    PlayerInput playInput;

    Color g = Color.green;

    List<EnemyManager> enemies;
    Scene currentScene;
    string nameCurrentScene;

    public List<Transform> capturePosition;
    public int currentCapturedPosition = 0;
    public int CurrentCapturedPosition { get { return currentCapturedPosition; } set { currentCapturedPosition = value; } }

    public float capturePosIconSize = 0.4f;
    public Color capturePosIconColor = Color.red;

    //public Button btnClick;

    public Canvas coverCanvasPsw;
    public Canvas coverCanvasWiFi;
    public Canvas insertPsw;
    public Canvas scanScreen;
    public Canvas terminalCanvas;
    public Canvas spoofed;

    public InputField passwordTyped;
    public InputField usernameTyped;
    public InputField emailTyped;
    public InputField surnameTyped;
    public string pswFromField;
    public string usernameFromField;
    public string emailFromField;
    public string[] password = {"1234", "3142", "2413", "1243", "3214", "4321", "4132", "1432", "1324"};
    int n;
    string pswSecurityCam = "admin";
    string usernameSecurityCam = "admin";

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        nameCurrentScene = currentScene.name;
        n = Random.Range(0, 8);
        //btnClick.onClick.AddListener(() => GetInputOnClickHandler());
    }
    void Awake()
    {
        m_player = Object.FindObjectOfType<PlayerMover>().GetComponent<PlayerMover>();
        playInput = Object.FindObjectOfType<PlayerInput>().GetComponent<PlayerInput>();
        GetNodeList();

        m_goalNode = FindGoalNode();
        doorNode = FindDoorNode();
        computerNode = FindComputerNode();
        keyNode = FindKeyNode();
        terminalNode = FindTerminalNode();
        doubleDoorNode = FindDoubleDoorNode();
        jacketNode = FindJacketNode();
    }

    void Update()
    {
        if (currentScene.name == "Level1")
        {
            ChangeCameraOnNodeLvl2();
        }

        if (currentScene.name == "Level4")
        {
            ChangeCameraOnNodeLvl4();
        }

        if(currentScene.name == "Level3")
        {
            ChangeCameraOnNodeLvl3();
        }
       
        DrawKey();
        DrawJacket();
    }

    public void GetNodeList()
    {
        Node[] nList = GameObject.FindObjectsOfType<Node>();
        m_allNodes = new List<Node>(nList);
    }

    public Node FindNodeAt(Vector3 pos)
    {
        Vector2 boardCoord = Utility.Vector2Round(new Vector2 (pos.x, pos.z));
        return m_allNodes.Find(n => n.Coordinate == boardCoord);
    }

    private Node FindGoalNode()
    {
        return m_allNodes.Find(n => n.isLevelGoal);
    }

    private Node FindDoorNode()
    {
        return m_allNodes.Find(n => n.isDoorNode);
    }

    private Node FindComputerNode()
    {
        return m_allNodes.Find(n => n.isComputerNode);
    }

    private Node FindKeyNode()
    {
        return m_allNodes.Find(n => n.isKeyNode);
    }

    private Node FindTerminalNode()
    {
        return m_allNodes.Find(n => n.isTerminalNode);
    }

    private Node FindDoubleDoorNode()
    {
        return m_allNodes.Find(n => n.isDoubleDoorNode);
    }

    private Node FindJacketNode()
    {
        return m_allNodes.Find(n => n.isJacketNode);
    }

    public Node FindPlayerNode()
    {
        if(m_player != null && !m_player.isMoving)
        {
            Debug.Log("Player Node Found");
            return FindNodeAt(m_player.transform.position);
        }
        return null;
    }

    public List<EnemyManager> FindEnemiesAt(Node node)
    {
        List<EnemyManager> foundEnemies = new List<EnemyManager>();
        EnemyManager[] enemies = Object.FindObjectsOfType<EnemyManager>() as EnemyManager[];
        foreach (EnemyManager enemy in enemies)
        {
            EnemyMover mover = enemy.GetComponent<EnemyMover>();
            if(mover.CurrentNode == node)
            {
                foundEnemies.Add(enemy);
            }
        }
        return foundEnemies;
    }

    public void UpdatePlayerNode()
    {
        m_playerNode = FindPlayerNode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
        if(m_playerNode != null)
        {
            Gizmos.DrawSphere(m_playerNode.transform.position, 0.2f);
        }

        Gizmos.color = capturePosIconColor;
        foreach(Transform capturePos in capturePosition)
        {
            Gizmos.DrawCube(capturePos.position, Vector3.one * capturePosIconSize);
        }
    }

    public void DrawGoal()
    {
        Vector3 centerDoor = new Vector3(-0.5f, 0f, 0f);
        Vector3 computerPosition = new Vector3(0f, 0.7f, 0f);
        Vector3 terminalPos = new Vector3(0.15f, 1.3f, 0f);
        Vector3 metalDoorPos = new Vector3(0.003f, 0f, 0.38f);
        Vector3 doubleDoorPos = new Vector3(0f, 1.45f, 0f);
        
        if(doorPrefab != null && doorNode != null)
        {
            GameObject doorInstance = Instantiate(doorPrefab, doorNode.transform.position + centerDoor, Quaternion.identity);
            
            iTween.ScaleFrom(doorInstance, iTween.Hash(
               "scale", Vector3.zero,
               "delay", drawGoalDelay,
               "time", drawGoalTime));
        }
        
        if(goalPrefab != null && m_goalNode != null)
        {
            GameObject goalInstance = Instantiate(goalPrefab, m_goalNode.transform.position, Quaternion.identity);
          
            iTween.ScaleFrom(goalInstance, iTween.Hash(
                "scale", Vector3.zero,
                "delay", drawGoalDelay,
                "time", drawGoalTime,
                "easetype", drawGoalEaseType));
           
        }

        if(computerPrefab != null && computerNode != null && nameCurrentScene.Equals("Level2"))
        {
            GameObject computerInstance = Instantiate(computerPrefab, computerNode.transform.position + computerPosition, Quaternion.Euler(0f,-90f, 0f));

            iTween.ScaleFrom(computerInstance, iTween.Hash(
               "scale", Vector3.zero,
               "delay", drawGoalDelay,
               "time", drawGoalTime));
        } else if(computerPrefab != null && computerNode != null && nameCurrentScene.Equals("Level4"))
        {
            GameObject computerInstance = Instantiate(computerPrefab, computerNode.transform.position + new Vector3(0f, 0.89f, 0f), Quaternion.Euler(0f, 90f, 0f));

            iTween.ScaleFrom(computerInstance, iTween.Hash(
               "scale", Vector3.zero,
               "delay", drawGoalDelay,
               "time", drawGoalTime));
        }

        if(metalDoorPrefab != null && doorNode != null)
        {
            GameObject metalDoorInstance = Instantiate(metalDoorPrefab, doorNode.transform.position + metalDoorPos, Quaternion.identity);
            iTween.ScaleFrom(metalDoorInstance, iTween.Hash(
                "scale", Vector3.zero,
                "delay", drawGoalDelay,
                "time", drawGoalTime));
        }

        if (terminalPrefab != null && terminalNode != null)
        {
            GameObject terminalInstance = Instantiate(terminalPrefab, terminalNode.transform.position + terminalPos, Quaternion.Euler(0f, 180f, 0f));
            iTween.ScaleFrom(terminalInstance, iTween.Hash(
                "scale", Vector3.zero,
                "delay", drawGoalDelay,
                "time", drawGoalTime));
        }

        if(doubleDoorPrefab != null && doorNode != null)
        {
            GameObject doubleDoorInstance = Instantiate(doubleDoorPrefab, doorNode.transform.position + doubleDoorPos, Quaternion.Euler(0f, 90f, 0f));
            iTween.ScaleFrom(doubleDoorInstance, iTween.Hash(
                "scale", Vector3.zero,
                "delay", drawGoalDelay,
                "time", drawGoalTime));
        }

        if(doubleDoorPrefab != null && doubleDoorNode != null)
        {
            GameObject doubleDoorInstance1 = Instantiate(doubleDoorPrefab, doubleDoorNode.transform.position + doubleDoorPos, Quaternion.Euler(0f, 90f, 0f));
            
            iTween.ScaleFrom(doubleDoorInstance1, iTween.Hash(
                "scale", Vector3.zero,
                "delay", drawGoalDelay,
                "time", drawGoalTime));
        }
    }

    public void InitBoard()
    {
        if(m_playerNode != null)
        {
            Debug.Log("playerNode non esiste");
            m_playerNode.InitNode();
        }
    }

    public bool StopPlayerOnDoor()
    {
        Vector3 spacingZ = new Vector3(0f, 0f, 2f);
        Vector3 spacingX = new Vector3(2f, 0f, 0f);

        try
        {
            if ((FindNodeAt(m_player.transform.position + spacingZ).isDoorNode || FindNodeAt(m_player.transform.position + spacingX).isDoorNode) && m_player.isMoving == false)
            {
                Debug.Log(n.ToString());
                insertPsw.gameObject.SetActive(true);
                //Debug.Log("HAI DAVANTI UNA PORTA!");
                pswFromField = passwordTyped.text;
                if (pswFromField==password[n])
                { 
                    insertPsw.gameObject.SetActive(false);
                    return true;
                    //doorPrefab.gameObject.transform.rotation = new Quaternion(0f, -90f, 0f, 0f);
                }
                else
                    return false;
            }
            else
            {
                insertPsw.gameObject.SetActive(false);
                //Debug.Log("VIA LIBERA");
                return false;
            }
        }
        catch
        {
            //Debug.Log("No porte in questo livello.");
            return false;
        }
    }

    public bool StopPlayerOnPC()
    {
        Vector3 oneSpaceX = new Vector3(1f, 0f, 0f);

        try
        {
            // Check if the player is in front of the ComputerNode
            if (FindNodeAt(m_player.transform.position + oneSpaceX).isComputerNode && m_player.isMoving==false)
            {

                // If it happens, InsertCredentials Canvas spawn

                coverCanvasPsw.gameObject.SetActive(true);
                
                // Take the credentials typed inside the InputField and check if the credentials are correct 
                
                usernameFromField = usernameTyped.text;
                pswFromField = passwordTyped.text;
                if (pswFromField.Equals(pswSecurityCam) && usernameFromField.Equals(usernameSecurityCam))
                {
                 // The player now can't move because is using the PC and must disable the securityCams, switching of Canvas
                  playInput.InputEnabled = false;
                  coverCanvasPsw.gameObject.SetActive(false);
                  coverCanvasWiFi.gameObject.SetActive(true);
                 
                  if (!scanScreen.isActiveAndEnabled)
                  {
                        playInput.InputEnabled = true;
                        coverCanvasPsw.gameObject.SetActive(true);
                  }
                        return true;
                  }
                  else
                  {
                    countAttemptsCredentials++;
                    return false;
                  }
                }
            else
            {
                coverCanvasPsw.gameObject.SetActive(false);
                insertPsw.gameObject.SetActive(true);
                //Debug.Log("VIA LIBERA");
                return false;
            }
        }
        catch
        {
            //Debug.Log("No porte in questo livello.");
            return false;
        }
    }

    /*
    public void GetInputOnClickHandler()
    {
        Debug.Log("premuto");
    }*/

    public bool StopPlayerOnTerminal()
    {
        Vector3 distanceFromNode = new Vector3(2f, 0f, 0f);

        try
        {
            if (FindNodeAt(m_player.transform.position - distanceFromNode).isTerminalNode && m_player.isMoving == false)
            {
                terminalCanvas.gameObject.SetActive(true);
                playInput.InputEnabled = false;
                //Debug.Log("HAI DAVANTI UNA PORTA!");
                usernameFromField = usernameTyped.text;
                pswFromField = surnameTyped.text;
                emailFromField = emailTyped.text;
                if (usernameFromField.Length>=4 && emailFromField.Contains('@') && pswFromField.Length>=7)
                {
                    //Debug.Log("form correct");
                    terminalCanvas.gameObject.SetActive(false);
                    playInput.InputEnabled = true;
                    enemiesPatrol[0].GetComponent<Renderer>().enabled = false;
                    enemiesPatrol[1].GetComponent<Renderer>().enabled = false;
                    enemiesPatrol[2].GetComponent<Renderer>().enabled = false;
                    enemiesSent[0].GetComponent<Renderer>().enabled = false;
                    hitmanEnemySent[0].GetComponent<Renderer>().enabled = true;
                    hitmanEnemyPatrol[0].GetComponent<Renderer>().enabled = true;
                    hitmanEnemyPatrol[1].GetComponent<Renderer>().enabled = true;
                    hitmanEnemyPatrol[2].GetComponent<Renderer>().enabled = true;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                insertPsw.gameObject.SetActive(false);
                //Debug.Log("VIA LIBERA");
                return false;
            }
        }
        catch
        {
            //Debug.Log("No porte in questo livello.");
            return false;
        }
    }

    // Change camera of the level when the player open the door
    public void ChangeCameraOnNodeLvl3()
    {
        if(m_playerNode == doorNode)
        {
            mainCamera.SetActive(false);
            retroCamera.SetActive(true);
        }
        else if (m_playerNode.transform.position == new Vector3(7f, 0f, 0f) && retroCamera.activeInHierarchy)
        {
            mainCamera.SetActive(true);
            retroCamera.SetActive(false);
        } 
    }

    public void ChangeCameraOnNodeLvl2()
    {
        if(m_playerNode == doorNode)
        {
            mainCamera.SetActive(false);
            retroCamera.SetActive(true);
        } 
        else if(m_playerNode.transform.position == new Vector3(-2f, 0f, 0f) && retroCamera.activeInHierarchy)
        {
            mainCamera.SetActive(true);
            retroCamera.SetActive(false);
        }
    }

    public void ChangeCameraOnNodeLvl4()
    {
        
        if (m_playerNode.transform.position == new Vector3(-1f, 0f, 2f) && retroCamera.activeInHierarchy)
        {
            cameraFirstRoom.SetActive(true);
            retroCamera.SetActive(false);
        }
        else if (m_playerNode.transform.position == new Vector3(1f, 0f, 2f))
        {
            cameraFirstRoom.SetActive(false);
            retroCamera.SetActive(true);
        }

        if (m_playerNode.transform.position == new Vector3(1f, 0f, -2f) || m_playerNode.transform.position == new Vector3(1f, 0f, -4f))
        {
            mainCamera.SetActive(false);
            retroCamera.SetActive(true);
        } 

        if (m_playerNode.transform.position == new Vector3(5f, 0f, 8f))
        {
            lateralCam.SetActive(false);
            cameraSecondRoom.SetActive(true);
        }

        if (m_playerNode.transform.position == new Vector3(3f, 0f, 4f))
        {
            retroCamera.SetActive(false);
            lateralCam.SetActive(true);
        }
    }

    public void DrawKey()
    {
        Vector3 keyPos = new Vector3(0f, 0.5f, 0f);

        if (keyLockPrefab != null && keyNode != null && enemiesSent[0].GetComponent<EnemyManager>().IsDead && keySpawned == false)
        {
            GameObject keyInstance = Instantiate(keyLockPrefab, keyNode.transform.position + keyPos, Quaternion.Euler(0f, 45f, 0f));
            keySpawned = true;
            iTween.ScaleFrom(keyInstance, iTween.Hash(
                "scale", Vector3.zero,
                "delay", drawGoalDelay,
                "time", drawGoalTime));
        }
    }

    public void DrawJacket()
    {
        if (enemyJacketPrefab != null && (enemiesPatrol[0].GetComponent<EnemyManager>().IsDead || enemiesPatrol[1].GetComponent<EnemyManager>().IsDead
            || enemiesPatrol[2].GetComponent<EnemyManager>().IsDead) && jacketSpawned == false)
        {
            jacketSpawned = true;
            if (enemiesPatrol[0].GetComponent<EnemyManager>().IsDead)
            {
                GameObject jacketInstance = Instantiate(enemyJacketPrefab, (jacketNode.transform.position + new Vector3(0f, -0.7f, 0f)), Quaternion.identity);
                iTween.ScaleFrom(jacketInstance, iTween.Hash(
                "scale", Vector3.zero,
                "delay", drawGoalDelay,
                "time", drawGoalTime));

            } else if (enemiesPatrol[1].GetComponent<EnemyManager>().IsDead)
            {
                GameObject jacketInstance = Instantiate(enemyJacketPrefab, (jacketNode.transform.position + new Vector3(0f, -0.7f, 0f)), Quaternion.identity);
                iTween.ScaleFrom(jacketInstance, iTween.Hash(
                "scale", Vector3.zero,
                "delay", drawGoalDelay,
                "time", drawGoalTime));

            } else if (enemiesPatrol[2].GetComponent<EnemyManager>().IsDead)
            {
                GameObject jacketInstance = Instantiate(enemyJacketPrefab, (jacketNode.transform.position + new Vector3(0f, -0.7f, 0f)), Quaternion.identity);
                iTween.ScaleFrom(jacketInstance, iTween.Hash(
                "scale", Vector3.zero,
                "delay", drawGoalDelay,
                "time", drawGoalTime));

            }
        }   
    }

    public void DeleteCredIfPressed()
    {
        passwordTyped.text = "";
        usernameTyped.text = "";
    }
}


