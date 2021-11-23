using UnityEngine;
using UnityEngine.UI;

enum GameState
{
    PLAY,
    PAUSE,
    WIN,
    LOOSE,
    FINISHED
}

public class GameEngine : MonoBehaviour
{

    public Text scoreLabel;
    public StaminaBar staminaBar;
    private int score = 0;
    private int missStaminaCost = 10;
    private int hitStaminaCost = 2;

    private bool isExhausted = false;

    public TimerController timer;
    private GameState gameState = GameState.PLAY;

    private void Start()
    {
        timer.InitWithValue(60);
        BugEvents.onDeath += this.updateScore;
    }

    private void FixedUpdate()
    {
        switch (gameState)
        {
            case GameState.PLAY:
                PlayLoop();
                break;
            case GameState.LOOSE:
                timer.ToggleTimer();
                gameState = GameState.FINISHED;
                break;
            case GameState.WIN:
                // TODO: - Display win overlay
                break;
            case GameState.PAUSE:
                // TODO: - Display pause overlay
                break;
            case GameState.FINISHED:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isExhausted)
            {
                staminaBar.UseStamina(0);
                return;
            }

            staminaBar.UseStamina(missStaminaCost);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 100.0f);
            Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
            
        }
    }

    public void updateScore(Bug bug)
    {
        if (scoreLabel != null)
        {
            score += bug.score;
            scoreLabel.text = "Score: " + score;
        }
    }
    
    public void updateTime(int newtime)
    {
        timer.IncreaseTime(newtime);
    }


    private void PlayLoop()
    {
        if (timer.GetRemainingTime() <= 0f)
        {
            gameState = GameState.LOOSE;
            return;
        }
    }

    public bool IsExhausted()
    {
        return isExhausted;
    }

    public void SetExaustion(bool value)
    {
        isExhausted = value;
    }

    public void TriggerPause()
    {
        gameState = Time.timeScale == 1 ? GameState.PAUSE : GameState.PLAY;
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
    }
}
