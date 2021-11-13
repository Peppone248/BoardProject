﻿using System.Collections;
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
    public Node GoalNode { get { return m_goalNode; } }

    public GameObject goalPrefab;
    public GameObject doorPrefab;
    public float drawGoalTime = 1.2f;
    public float drawGoalDelay = 1.2f;
    public iTween.EaseType drawGoalEaseType = iTween.EaseType.easeOutExpo;
    PlayerMover m_player;

    public List<Transform> capturePosition;
    public int currentCapturedPosition = 0;
    public int CurrentCapturedPosition { get { return currentCapturedPosition; } set { currentCapturedPosition = value; } }

    public float capturePosIconSize = 0.4f;
    public Color capturePosIconColor = Color.red;
    //public Canvas pressAKey;
    public Canvas insertPsw;
    public InputField passwordTyped;
    public string pswFromField;
    public string password = "1234";

    void Awake()
    {
        m_player = Object.FindObjectOfType<PlayerMover>().GetComponent<PlayerMover>();
        GetNodeList();

        m_goalNode = FindGoalNode();
        doorNode = FindDoorNode();
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
       //StopPlayerOnDoor();
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
        if(goalPrefab != null && m_goalNode != null)
        {
            GameObject goalInstance = Instantiate(goalPrefab, m_goalNode.transform.position, Quaternion.identity);
            GameObject doorInstance = Instantiate(doorPrefab, doorNode.transform.position + centerDoor, Quaternion.identity);
            
            iTween.ScaleFrom(goalInstance, iTween.Hash(
                "scale", Vector3.zero,
                "delay", drawGoalDelay,
                "time", drawGoalTime,
                "easetype", drawGoalEaseType));

            iTween.ScaleFrom(doorInstance, iTween.Hash(
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
        Vector3 spacingZ = new Vector3(0f,0f,2f);
        Vector3 spacingX = new Vector3(2f, 0f, 0f);
        if (FindNodeAt(m_player.transform.position + spacingZ).isDoorNode || FindNodeAt(m_player.transform.position + spacingX).isDoorNode)
        {
            insertPsw.gameObject.SetActive(true);
            Debug.Log("HAI DAVANTI UNA PORTA!");
            pswFromField = passwordTyped.text;
            if (pswFromField.Equals(password))
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
            Debug.Log("VIA LIBERA");
            return false;
        }
    }
}