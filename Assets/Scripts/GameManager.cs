using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
   public int scorePlayer1 = 0;
   public int scorePlayer2 = 0;

   public Transform ball;
   public Ball ballScript;
   public Text score;
   public Text winner;

   private float currentSpeed = 5;

   private void Update()
   {
        float screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x; 
        float screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        if (ball.position.x + 0.25f < screenLeft)
        {
            AddScore(2);
            ResetBallPositionAndDirection(2);
        }
        else if (ball.position.x - 0.25f > screenRight)
        {
            AddScore(1);
            ResetBallPositionAndDirection(1);
        }

        if(scorePlayer1 >= 10 || scorePlayer2 >= 10)
        {
            ResetGame(scorePlayer1 > scorePlayer2 ? 1 : 2);
        }
   }

   public void AddScore(int player)
    {
        if (player == 1)
        {
            scorePlayer1++;
        }
        else if (player == 2)
        {
            scorePlayer2++;
        }

        score.text = $"{scorePlayer1} x {scorePlayer2}";
    }

    public async void ResetBallPositionAndDirection(int player)
    {
        currentSpeed = ballScript.speed;
        ball.position = Vector3.zero;
        ballScript.speed = 0;
        
        await Task.Delay(2000);

        float xDir = (player == 1) ? -1f : 1f;

        Vector2 newDirection = new Vector2(xDir, UnityEngine.Random.Range(-1f, 1f)).normalized;
        ballScript.SetDirection(newDirection);
        BallSpeed();
    }

    public void BallSpeed()
    {
        if (scorePlayer1 + scorePlayer2 >= 5)
        {
            ballScript.speed = currentSpeed + 1;
        
        }else
        {
            ballScript.speed = 5;
        }
    }

    public async void ResetGame(int winnerPlayer)
    {
        Vector2 winnerTextPosition = winner.rectTransform.anchoredPosition;
        winnerTextPosition.x = winnerPlayer == 1 ? -300 : 300;
        winner.rectTransform.anchoredPosition = winnerTextPosition;

        winner.text = $"Player {winnerPlayer} venceu!";

        await Task.Delay(2000);
        winner.text = "";
        scorePlayer1 = 0;
        scorePlayer2 = 0;
        score.text = $"{scorePlayer1} x {scorePlayer2}";
        ballScript.SetDirection(Vector2.one.normalized);
        ballScript.speed = 5;
    }
}
