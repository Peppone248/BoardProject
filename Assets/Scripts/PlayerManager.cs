using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerDeath))]
public class PlayerManager : TurnManager
{
    public PlayerMover playerMover;
    public PlayerInput playerInput;
    public float countTurn;
    private bool isDead;

    Board m_board;

    public UnityEvent deathEvent;

    public bool IsDead { get => isDead; set => isDead = value; }

    protected override void Awake()
    {
        base.Awake();
        playerMover = GetComponent<PlayerMover>();
        playerInput = GetComponent<PlayerInput>();
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        playerInput.InputEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMover.isMoving || m_gameManager.CurrentTurn != Turn.Player)
        {
            
            return;
        }

        playerInput.GetKeyInput();
        
        if(playerInput.V == 0 && playerInput.InputEnabled)
        {
            if (playerInput.H < 0 && playerInput.InputEnabled)
            {
                playerMover.MoveLeft();
            } else if(playerInput.H > 0 && playerInput.InputEnabled)
            {
                playerMover.MoveRight();
            }
        } 
        else if(playerInput.H == 0 && playerInput.InputEnabled)
        {
            if(playerInput.V < 0 && playerInput.InputEnabled)
            {
                playerMover.MoveBackward();
            } else if(playerInput.V > 0 && playerInput.InputEnabled)
            {
                playerMover.MoveForward();
            }
        }

    }

    public void Die()
    {
        if (deathEvent != null)
        {
            deathEvent.Invoke();
            isDead = true;
        }
        else
            isDead = false;
    }

    private void CaptureEnemies()
    {
        if (m_board != null)
        {
            List<EnemyManager> enemies = m_board.FindEnemiesAt(m_board.PlayerNode);

            if(enemies.Count != 0)
            {
                foreach (EnemyManager enemy in enemies)
                {
                    if(enemy != null)
                    {
                        enemy.Die();
                    }
                }
            }
        }
    }

    public override void FinishTurn()
    {
        countTurn++;
        CaptureEnemies();
        Debug.Log("Turn number: " + countTurn);
        base.FinishTurn();
    }

}
