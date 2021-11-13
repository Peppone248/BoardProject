﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum Turn
{
    Player,
    Enemy
}

public class GameManager : MonoBehaviour
{
    Board m_board;
    PlayerManager player;
    bool hasLevelStarted = false;
    bool isGamePlaying = false;
    bool isGameOver = false;
    bool hasLevelFinish= false;
    public float delay = 1f;
    public GameObject[] goalComplete;
    public GameObject[] passwordCanvas;

    List<EnemyManager> enemies;
    Turn currentTurn = Turn.Player;

    public UnityEvent setupEvent;
    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent endLevelEvent;
    public UnityEvent loseLevelEvent;

    public bool HasLevelStarted { get => hasLevelStarted; set => hasLevelStarted = value; }
    public bool IsGamePlaying { get => isGamePlaying; set => isGamePlaying = value; }
    public bool IsGameOver { get => isGameOver; set => isGameOver = value; }
    public bool HasLevelFinish { get => hasLevelFinish; set => hasLevelFinish = value; }
    public Turn CurrentTurn { get => currentTurn; }

    void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        player = GameObject.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        EnemyManager[] t_enemies = GameObject.FindObjectsOfType<EnemyManager>() as EnemyManager[];
        enemies = t_enemies.ToList();
      
    }
    // Start is called before the first frame update
    void Start()
    {
        if(player != null && m_board != null)
        {
            StartCoroutine(RunGameLoop());
        }
        else
        {
            Debug.LogWarning("GAMEMANAGER ERROR: NO PLAYER OR BOARD FOUND");
        }
    }

    IEnumerator RunGameLoop()
    {
        yield return StartCoroutine("StartLevelRoutine");
        yield return StartCoroutine("PlayLevelRoutine");
        yield return StartCoroutine("EndLevelRoutine");
    }

    IEnumerator StartLevelRoutine()
    {
        Debug.Log("SETUP LEVEL");
        if(setupEvent != null)
        {
            setupEvent.Invoke();
        }
        Debug.Log("START LEVEL");
        player.playerInput.InputEnabled = false;
       
        while (!hasLevelStarted)
        {
            //show start screen
            // press start btn
            // set bool true
            yield return null;
        }
        
        if(startLevelEvent != null)
        {
            startLevelEvent.Invoke();
        }
    }

    IEnumerator PlayLevelRoutine()
    {
        Debug.Log("PLAY LEVEL");
        isGamePlaying = true;
        yield return new WaitForSeconds(delay);
        player.playerInput.InputEnabled = true;
        
        if(playLevelEvent != null)
        {
            playLevelEvent.Invoke();
        }

        while (!isGameOver)
        {

            StopPlayer();
            // check for GameOVER CONDITIONS
            //win reach the end of the level

            // lose - player dies
            yield return null;
            isGameOver = IsWinner();
        }
        Debug.Log("WIN THE LEVEL");
    }

    public void LoseLevel()
    {
        StartCoroutine(LoseLevelRoutine());
    }

    IEnumerator LoseLevelRoutine()
    {
        isGameOver = true;

        yield return new WaitForSeconds(1f);

        if (loseLevelEvent != null)
        {
            loseLevelEvent.Invoke();
        }

        yield return new WaitForSeconds(3f);

        RestartLevel();
    }

    IEnumerator EndLevelRoutine()
    {
        Debug.Log("END LEVEL");
        player.playerInput.InputEnabled = false;

        if(endLevelEvent != null)
        {
            endLevelEvent.Invoke();
        }
        // end screen
        while (!hasLevelFinish)
        {
            // btn to continue
            // haslevelfinish = true
            yield return null;
        }
        RestartLevel();
    }

    void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void PlayLevel()
    {
        hasLevelStarted = true;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public bool IsWinner()
    {
        if(m_board.PlayerNode != null && m_board.PlayerNode == m_board.GoalNode)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StopPlayer()
    {
        if (isGameOver == true)
        {
            player.playerInput.InputEnabled = false;
        }
    }

    void PlayPlayerTurn()
    {
        //player.countTurn++;
        currentTurn = Turn.Player;
        player.IsTurnComplete = false;
    }

    void PlayEnemyTurn()
    {
        currentTurn = Turn.Enemy;
        foreach(EnemyManager enemy in enemies)
        {
            if (enemy != null && !enemy.IsDead)
            {
                enemy.IsTurnComplete = false;
                enemy.PlayTurn();
            }
        }
    }
    bool IsEnemyTurnComplete()
    {
        foreach (EnemyManager enemy in enemies)
        {
            if (enemy.IsDead)
            {
                continue;
            }
            if (!enemy.IsTurnComplete)
            {
                return false;
            }
        }
        return true;
    }

    bool AreEnemiesAllDead()
    {
        foreach(EnemyManager enemy in enemies)
        {
            if (!enemy.IsDead)
            {
                return false;
            }
        }
        return true;
    }

    public void UpdateTurn()
    {
        if(currentTurn == Turn.Player && player != null)
        {
            if (player.IsTurnComplete && !AreEnemiesAllDead())
            {
                //START ENEMIES TURN
                PlayEnemyTurn();
            }

        } else if (currentTurn == Turn.Enemy)
        {
            // if enermy turn is complete, start player turn
            if (IsEnemyTurnComplete())
            {
                PlayPlayerTurn();
            }

        }

    }

    public void GoalCompleted()
    {
        if (AreEnemiesAllDead())
        {
            goalComplete[1].SetActive(true);
        }
        else
            goalComplete[1].SetActive(false);

        if (player.countTurn <= 8)
        {
            goalComplete[0].SetActive(true);

        }
        else
            goalComplete[0].SetActive(false);
    }

    public void CloseCanvas()
    {
        for(int i=0; i<=passwordCanvas.Length; i++)
        {
            passwordCanvas[i].SetActive(false);
        }
    }

}
