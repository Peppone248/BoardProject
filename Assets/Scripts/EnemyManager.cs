using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
[RequireComponent(typeof(EnemyAttack))]
public class EnemyManager : TurnManager
{
    EnemyMover enemyMover;
    EnemySensor enemySensor;
    EnemyAttack enemyAttack;
    Board m_board;

    bool isDead = false;

    public UnityEvent deathEvent;

    public bool IsDead { get => isDead; }

    protected override void Awake()
    {
        base.Awake();
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        enemySensor = GetComponent<EnemySensor>();
        enemyMover = GetComponent<EnemyMover>();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    public void PlayTurn()
    {
        if (isDead)
        {
            FinishTurn();
            return;
        }
        StartCoroutine(PlayTurnRoutine());
    }

    IEnumerator PlayTurnRoutine()
    {
        if (gameManager != null && !gameManager.IsGameOver)
        {
            // detect player
            enemySensor.UpdateSensor(enemyMover.CurrentNode);

            // wait
            yield return new WaitForSeconds(0f);

            if (enemySensor.PlayerFound)
            {
                gameManager.LoseLevel();

                Vector3 playerPosition = new Vector3(m_board.PlayerNode.Coordinate.x, 0f, m_board.PlayerNode.Coordinate.y);
                enemyMover.Move(playerPosition, 0f);

                while (enemyMover.isMoving)
                {
                    yield return null;
                }
                // attack
                enemyAttack.Attack();        
            }
            else
            {
                enemyMover.MoveOneTurn();
            }
        }
    }

    public void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;

        if(deathEvent != null)
        {
            deathEvent.Invoke();
        }
    }

    public void RotatePatrolEnemy()
    {
        gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
    }
}
