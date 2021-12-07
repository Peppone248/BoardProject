using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    Node computerNode;
    Node keyNode;
    Color g = Color.green;
    public Node GoalNode { get { return m_goalNode; } }

    public GameObject goalPrefab;
    public GameObject doorPrefab;
    public GameObject computerPrefab;
    public GameObject metalDoorPrefab;
    //public GameObject keyPrefab;
    public GameObject mainCamera;
    public GameObject retroCamera;
    public Light[] pointLight;

    float drawGoalTime = 1f;
    float drawGoalDelay = 0.3f;
    public iTween.EaseType drawGoalEaseType = iTween.EaseType.easeOutExpo;
    PlayerMover m_player;
    PlayerInput playInput;

    public List<Transform> capturePosition;
    public int currentCapturedPosition = 0;
    public int CurrentCapturedPosition { get { return currentCapturedPosition; } set { currentCapturedPosition = value; } }

    public float capturePosIconSize = 0.4f;
    public Color capturePosIconColor = Color.red;
    public Canvas insertPsw;
    public Canvas scanScreen;
    public InputField passwordTyped;
    public InputField usernameTyped;
    public string pswFromField;
    public string usernameFromField;
    public string[] password = {"1234", "3142", "2413", "1243", "3214", "4321", "4132", "1432", "1324"};
    string pswSecurityCam = "admin";
    string usernameSecurityCam = "admin";


    void Awake()
    {
        m_player = Object.FindObjectOfType<PlayerMover>().GetComponent<PlayerMover>();
        playInput = Object.FindObjectOfType<PlayerInput>().GetComponent<PlayerInput>();
        GetNodeList();

        m_goalNode = FindGoalNode();
        doorNode = FindDoorNode();
        computerNode = FindComputerNode();
        keyNode = FindKeyNode();
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
        Vector3 keyPos = new Vector3(0f, 1.2f, 0f);
        
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

        if(computerPrefab != null && computerNode != null)
        {
            GameObject computerInstance = Instantiate(computerPrefab, computerNode.transform.position + computerPosition, Quaternion.Euler(0f,-90f, 0f));

            iTween.ScaleFrom(computerInstance, iTween.Hash(
               "scale", Vector3.zero,
               "delay", drawGoalDelay,
               "time", drawGoalTime));
        }

        if(metalDoorPrefab != null && doorNode != null)
        {
            GameObject metalDoorInstance = Instantiate(metalDoorPrefab, doorNode.transform.position, Quaternion.Euler(0f, -90f, 0f));
            iTween.ScaleFrom(metalDoorInstance, iTween.Hash(
                "scale", Vector3.zero,
                "delay", drawGoalDelay,
                "time", drawGoalTime));
        }

       /* if(keyPrefab != null && keyNode != null)
        {
            GameObject keyInstance = Instantiate(keyPrefab, keyNode.transform.position, Quaternion.identity);
            iTween.ScaleFrom(keyPrefab, iTween.Hash(
                "scale", Vector3.zero,
                "delay", drawGoalDelay,
                "time", drawGoalTime));
        } */
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
                int n = Random.Range(0, 9);
                insertPsw.gameObject.SetActive(true);
                //Debug.Log("HAI DAVANTI UNA PORTA!");
                pswFromField = passwordTyped.text;
                if (pswFromField==password[5])
                {
                    Debug.Log("psw correct");
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
            if (FindNodeAt(m_player.transform.position + oneSpaceX).isComputerNode && m_player.isMoving==false)
            {
                insertPsw.gameObject.SetActive(true);
                //Debug.Log("HAI DAVANTI UNA PORTA!");
                usernameFromField = usernameTyped.text;
                pswFromField = passwordTyped.text;
                if (pswFromField.Equals(pswSecurityCam) && usernameFromField.Equals(usernameSecurityCam))
                {
                    playInput.InputEnabled = false;
                    Debug.Log("psw correct");
                    insertPsw.gameObject.SetActive(false);
                    scanScreen.gameObject.SetActive(true);
                    if(pointLight[0].color == g && pointLight[1].color == g && pointLight[2].color == g)
                    {
                        playInput.InputEnabled = true;
                    }
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
    public void ChangeCameraOnNode()
    {
        if(m_playerNode == doorNode)
        {
            mainCamera.SetActive(false);
            retroCamera.SetActive(true);
        }
    }
}
