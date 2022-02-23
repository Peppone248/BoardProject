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
    public GameObject pauseBtn;
    public GameObject cluePadlock;

    public Light[] pointLight;
    TerminalDetector t;

    bool keySpawned = false;
    bool jacketSpawned = false;
    bool doorOpen = false;
    float drawGoalTime = 1f;
    float drawGoalDelay = 0.1f;
    public iTween.EaseType drawGoalEaseType = iTween.EaseType.easeOutExpo;
    PlayerMover m_player;
    PlayerManager playerManager;
    PlayerInput playInput;
    GameManager game;

    List<EnemyManager> enemies;
    Scene currentScene;
    string nameCurrentScene;

    public List<Transform> capturePosition;
    public int currentCapturedPosition = 0;
    public int CurrentCapturedPosition { get { return currentCapturedPosition; } set { currentCapturedPosition = value; } }

    public float capturePosIconSize = 0.4f;
    public Color capturePosIconColor = Color.red;

    public Canvas insertPsw;
    public Canvas scanScreen;
    public Canvas terminalCanvas;
    public Canvas spoofed;

    public Button buttonCrackDoor;

    public AudioSource errorSource;
    public AudioClip errorEffect;

    public Text passwordText;
    public InputField passwordTyped;
    public InputField usernameTyped;
    public InputField emailTyped;
    public InputField surnameTyped;
    public string pswFromField;
    public string usernameFromField;
    public string emailFromField;
    private string[] password3 = {"1243", "1423", "2143", "2413", "4123", "4213"};
    private string[] password1 = { "4231", "4321", "3421", "3241", "2341", "2431" };
    private string[] password2 = { "1342", "1432", "3142", "3412", "4312", "4132" };
    private string[] password4 = { "1234", "3124", "1324", "2134", "2314", "3214" };
    int n;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        nameCurrentScene = currentScene.name;
        n = Random.Range(0, 5);
    }
    void Awake()
    {
        m_player = Object.FindObjectOfType<PlayerMover>().GetComponent<PlayerMover>();
        playInput = Object.FindObjectOfType<PlayerInput>().GetComponent<PlayerInput>();
        playerManager = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        game = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
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
            ChangeCameraOnNodeLvl1();
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

    public void DrawPrefabs()
    {
        Vector3 centerDoor = new Vector3(-0.5f, 0f, 0f);
        Vector3 computerPosition = new Vector3(0f, 0.7f, 0f);
        Vector3 terminalPos = new Vector3(0.15f, 1.2f, 0f);
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
            if (FindNodeAt(m_player.transform.position).isDoorNode && !doorOpen && nameCurrentScene.Equals("Level1"))
            {
                playerManager.Die();
                game.LoseLevel();
            }

            if ((FindNodeAt(m_player.transform.position + spacingZ).isDoorNode || FindNodeAt(m_player.transform.position + spacingX).isDoorNode) && m_player.isMoving == false)
            {
                pauseBtn.SetActive(false);
                // Debug.Log(n.ToString());
                insertPsw.gameObject.SetActive(true);
                //Debug.Log("HAI DAVANTI UNA PORTA!");
                pswFromField = passwordText.text;
                if (pswFromField.Length == 4)
                {
                    if (Object.FindObjectOfType<DetectCollision>().GetComponent<DetectCollision>().RandomClueIndex == 0)
                    {
                        if (pswFromField == password1[n])
                        {
                            pauseBtn.SetActive(true);
                            doorOpen = true;
                            insertPsw.gameObject.SetActive(false);
                            return true;
                            //doorPrefab.gameObject.transform.rotation = new Quaternion(0f, -90f, 0f, 0f);
                        }
                        else
                        {
                            errorSource.PlayOneShot(errorEffect);
                            passwordText.text = "";
                            return false;
                        }
                    }

                    if (Object.FindObjectOfType<DetectCollision>().GetComponent<DetectCollision>().RandomClueIndex == 1)
                    {
                        if (pswFromField == password2[n])
                        {
                            pauseBtn.SetActive(true);
                            doorOpen = true;
                            insertPsw.gameObject.SetActive(false);
                            return true;
                            //doorPrefab.gameObject.transform.rotation = new Quaternion(0f, -90f, 0f, 0f);
                        }
                        else
                        {
                            errorSource.PlayOneShot(errorEffect);
                            passwordText.text = "";
                            return false;
                        }
                    }
                    
                    if (Object.FindObjectOfType<DetectCollision>().GetComponent<DetectCollision>().RandomClueIndex == 2)
                    {
                        if (pswFromField == password3[n])
                        {
                            pauseBtn.SetActive(true);
                            doorOpen = true;
                            insertPsw.gameObject.SetActive(false);
                            return true;
                            //doorPrefab.gameObject.transform.rotation = new Quaternion(0f, -90f, 0f, 0f);
                        }
                        else
                        {
                            errorSource.PlayOneShot(errorEffect);
                            passwordText.text = "";
                            return false;
                        }
                    }

                    if (Object.FindObjectOfType<DetectCollision>().GetComponent<DetectCollision>().RandomClueIndex == 3)
                    {
                        if (pswFromField == password4[n])
                        {
                            pauseBtn.SetActive(true);
                            doorOpen = true;
                            insertPsw.gameObject.SetActive(false);
                            return true;
                            //doorPrefab.gameObject.transform.rotation = new Quaternion(0f, -90f, 0f, 0f);
                        }
                        else
                        {
                            errorSource.PlayOneShot(errorEffect);
                            passwordText.text = "";
                            return false;
                        }
                    }
                }
                return false;
            }
            else
            {
                pauseBtn.SetActive(true);
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

    public bool ShowInteractionWitchPC()
    {
        Vector3 oneSpaceX = new Vector3(1f, 0f, 0f);

        try
        {
            // Check if the player is in front of the ComputerNode
            if (FindNodeAt(m_player.transform.position + oneSpaceX).isComputerNode && m_player.isMoving == false)
            {
                pauseBtn.SetActive(false);
                // If it happens, InsertCredentials Canvas spawn
                insertPsw.gameObject.SetActive(true);
                return true;
            }
            else
            {
                pauseBtn.SetActive(true);
                insertPsw.gameObject.SetActive(false);
                return false;
            }
        }
        catch
        {
            return false;
        }

    }

    public bool StopPlayerOnTerminal()
    {
        Vector3 distanceFromNode = new Vector3(1f, 0f, 0f);

        try
        {
            // Check if the player is in front of the TerminalNode
            if (FindNodeAt(m_player.transform.position - distanceFromNode).isTerminalNode && m_player.isMoving == false)
            {
                t = Object.FindObjectOfType<TerminalDetector>().GetComponent<TerminalDetector>();
                if (!t.IsSpoofed)
                {
                    pauseBtn.SetActive(false);
                }
                terminalCanvas.gameObject.SetActive(true);
                return true;
            }
            else
            {
                pauseBtn.SetActive(true);
                terminalCanvas.gameObject.SetActive(false);
                return false;
            }
            
        }
        catch
        {
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

    public void ChangeCameraOnNodeLvl1()
    {
        if(m_playerNode == doorNode)
        {
            mainCamera.SetActive(false);
            retroCamera.SetActive(true);
        } 
        else if(m_playerNode.transform.position == new Vector3(-2f, 0f, 0f))
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

        if (m_playerNode.transform.position == new Vector3(3f, 0f, 4f))
        {
            retroCamera.SetActive(false);
            lateralCam.SetActive(true);
        } else if(m_playerNode.transform.position == new Vector3(3f, 0f, 2f) && lateralCam.activeInHierarchy)
        {
            retroCamera.SetActive(true);
            lateralCam.SetActive(false);
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


