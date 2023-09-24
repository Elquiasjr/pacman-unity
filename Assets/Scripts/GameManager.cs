using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;

    public int currentScene;

    public int ghostMultiplier { get; private set; } = 1;

    public int score { get; private set; }

    public int lives { get; private set; }

    public void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (this.lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        SetLives(3);
        SetScore(0);
        NewRound();
    }

    private void NewRound()
    {
        scoreText.text = score.ToString();
        foreach (Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        ResetGhostMultiplier();

        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();
        }

        pacman.ResetState();
    }

    private void GameOver()
    {
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }

        this.pacman.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = $"Lives: {lives}";
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * this.ghostMultiplier;
        SetScore(this.score + points);
        this.ghostMultiplier++;
    }

    public void PacmanEaten()
    {
        this.pacman.gameObject.SetActive(false);

        SetLives(this.lives - 1);

        if (this.lives > 0)
        {
            Invoke(nameof(ResetState), 1.3f);
            return;
        }
        GameOver();
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(score + pellet.points);


        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3.0f);
            if (currentScene == 2)
            {
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene(currentScene + 1, LoadSceneMode.Single);
            }
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        foreach (Ghost ghost in ghosts)
        {
            if (ghost.gameObject.activeSelf)
            {
                ghost.frightened.Enable(pellet.duration);
            }
        }

        PelletEaten(pellet);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);

    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}

