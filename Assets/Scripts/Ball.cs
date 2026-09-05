using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 5;
    public Transform paddleLeft;
    public Transform paddleRight;
    private Vector2 direction = Vector2.one;
    private bool isSpacePressed = false;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer paddleLeftRenderer;
    private SpriteRenderer paddleRightRenderer;

    private Camera mainCamera;

    private void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        paddleLeftRenderer = paddleLeft.GetComponent<SpriteRenderer>();
        paddleRightRenderer = paddleRight.GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }


    private void Update()
    {
        if (!isSpacePressed)
        {
            WaitForStart();
            return;
        }

        Move();
        BounceTopAndBottom();
        BounceWithPaddles();
    } 

    private async void ChangeColor()
    {
        spriteRenderer.color = new Color32(178, 34, 34, 255);
        paddleLeftRenderer.color = new Color32(178, 34, 34, 255);
        paddleRightRenderer.color = new Color32(178, 34, 34, 255);
        mainCamera.backgroundColor = new Color32(0, 0, 0, 255);

        await Task.Delay(300);
        spriteRenderer.color = new Color32(255, 255, 255, 255);
        paddleLeftRenderer.color = new Color32(255, 255, 255, 255);
        paddleRightRenderer.color = new Color32(255, 255, 255, 255);
        mainCamera.backgroundColor = new Color32(102, 102, 102, 255);
    }
    private void Move()
    {
        Vector3 movement = direction * speed * Time.deltaTime;
        transform.Translate(movement);
    }

    private void BounceTopAndBottom()
    {
    
        float screenTop = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        float screenBottom = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;

        Vector3 position = transform.position;

        if (direction.y > 0 && position.y >= (screenTop - 0.25f))
        {
            ChangeColor();

            direction.y = -1;
        }

        if (direction.y < 0 && position.y <= (screenBottom + 0.25f))
        {
            ChangeColor();
            direction.y = 1;
        }
    }

    private void BounceWithPaddles()
    {
        float paddleWidth = 0.5f;
        float paddleHeight = 2f;

        float ballSize = 0.5f;

        if(direction.x > 0)
        {
            if ((transform.position.x + ballSize / 2f) > (paddleRight.position.x - paddleWidth / 2f) && (transform.position.x + ballSize / 2f) < (paddleRight.position.x + paddleWidth/2f) && transform.position.y > (paddleRight.position.y - paddleHeight/2f) && transform.position.y < (paddleRight.position.y + paddleHeight/2f))
            {
                ChangeColor();
                direction.x = -1;
            }
        }
        else if(direction.x < 0)
        {
           if ((transform.position.x - ballSize / 2f) < (paddleLeft.position.x + paddleWidth / 2f) && (transform.position.x - ballSize / 2f) > (paddleLeft.position.x - paddleWidth/2f) && transform.position.y > (paddleLeft.position.y - paddleHeight/2f) && transform.position.y < (paddleLeft.position.y + paddleHeight/2f))
            {
                ChangeColor();
                direction.x = 1;
            } 
        }        
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    public void WaitForStart()
    {
       if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpacePressed = true;
        }
    }
}
