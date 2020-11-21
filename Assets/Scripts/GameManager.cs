using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static LevelManager LEVEL;
    public static GameManager GAME;
    public static float soundVolume, musicVolume;
    [HideInInspector] public int levelLength, levelTwistyness;
    public AudioClip victorySound, defeatSound;
    public GameObject levelManager, mainmenuPanel, mainCamera, gameHudPanel, levelCompletePanel, gameOverPanel, pausePanel;
    public GameObject leftGoButton, leftLeftButton, leftRightButton, rightGoButton, rightLeftButton, rightRightButton;
    public Text menuHiScore, leftPoints, rightPoints, hudHiScore, lcScore, lcHiScore, goScore, goHiScore;
    public enum State { MENU, INIT, PLAY, LEVELCOMPLETE, GAMEOVER};
    [HideInInspector] public int leftScore, rightScore, savedHiScore, hiScore, gameScore;
    
    private float period = 0f, bgRot; private GameObject _ball1, _ball2;

    
    private State _state;

    public State GameState
    {
        get { return _state; }
        set { _state = value; }
    }

    private float _levelSpeed = 2.25f;
    public float LevelSpeed
    {   
        get { return _levelSpeed; }
        set { _levelSpeed = value; }
    }
    private bool _paused;
    public bool Paused
    {
        get { return _paused; }
        set { _paused = value; }
    }
    private bool _leftPlaying;
    public bool LeftPlaying
    {
        get { return _leftPlaying; }
        set
        {
            _leftPlaying = value;
            if(value == false)
            {
                leftLeftButton.SetActive(false);
                leftRightButton.SetActive(false);
            }
        }
    }
    private bool _rightPlaying;
    public bool RightPlaying
    {
        get { return _rightPlaying; }
        set
        {
            _rightPlaying = value;
            if(value == false)
            { 
            rightGoButton.SetActive(false);
            rightLeftButton.SetActive(false);
            rightRightButton.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        GetVolumeSettings();
    }


    void Start()
    {
        LEVEL = levelManager.GetComponent<LevelManager>();
        GAME = this;
        SwitchState(State.INIT);       
    }

    public void SwitchState(State newState)
    {
        EndState();
        BeginState(newState);
    }

    void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                CalulateHighScore();
                menuHiScore.text = "High Score: " + hiScore;
                mainmenuPanel.SetActive(true);
                gameHudPanel.SetActive(false);
                levelCompletePanel.SetActive(false);
                gameOverPanel.SetActive(false);
                pausePanel.SetActive(false);
                _state = State.MENU;
                break;
            case State.INIT:
                //SCORE
                leftScore = 0;
                rightScore = 0;
                gameScore = 0;
                hiScore = 0;
                CalulateHighScore();
                levelLength = 50; levelTwistyness = 15;
                LEVEL.BuildLevel(0, 0, 1, levelLength, levelTwistyness);
                LEVEL.BuildLevel(100, 0, 2, levelLength, levelTwistyness);
                _leftPlaying = true;
                _rightPlaying = true;
                _paused = true;
                _state = State.INIT;
                break;
            case State.PLAY:
                bgRot = Random.Range(1.0f, 5.0f);
                mainmenuPanel.SetActive(false);
                gameHudPanel.SetActive(true);
                levelCompletePanel.SetActive(false);
                gameOverPanel.SetActive(false);
                pausePanel.SetActive(false);
                rightGoButton.SetActive(true);
                rightLeftButton.SetActive(false);
                rightRightButton.SetActive(false);
                leftGoButton.SetActive(true);
                leftLeftButton.SetActive(false);
                leftRightButton.SetActive(false);
                _state = State.PLAY;
                _ball1 = GameObject.FindGameObjectWithTag("Ball1");
                _ball2 = GameObject.FindGameObjectWithTag("Ball2");
                break;
            case State.LEVELCOMPLETE:
                mainmenuPanel.SetActive(false);
                gameHudPanel.SetActive(false);
                levelCompletePanel.SetActive(true);
                gameOverPanel.SetActive(false);
                pausePanel.SetActive(false);
                lcScore.text = "Reft Ball Points: " + leftScore + " + Blue Ball Points: " + rightScore + " + Running Total Score: " + gameScore + " = " + (leftScore + rightScore + gameScore).ToString();
                gameScore = leftScore + rightScore + gameScore;
                CalulateHighScore();
                lcHiScore.text = "High Score: " + hiScore;
                _state = State.LEVELCOMPLETE;
                break;
            case State.GAMEOVER:
                mainmenuPanel.SetActive(false);
                gameHudPanel.SetActive(false);
                levelCompletePanel.SetActive(false);
                gameOverPanel.SetActive(true);
                pausePanel.SetActive(false);
                goScore.text = "Reft Ball Points: " + leftScore + " + Blue Ball Points: " + rightScore + " + Running Total Score: " + gameScore + " = " + (leftScore + rightScore + gameScore).ToString();
                gameScore = leftScore + rightScore + gameScore;
                CalulateHighScore();
                goHiScore.text = "High Score: " + hiScore;
                _state = State.GAMEOVER;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * bgRot);
        
        switch (_state)
        {
            case State.MENU:

                break;
            case State.INIT:
                SwitchState(State.MENU);
                break;
            case State.PLAY:
                if(period > 0.1f)
                {
                    
                    if (_leftPlaying) leftScore++;
                    if (_rightPlaying) rightScore++;
                    if (!_leftPlaying) leftScore--;
                    if (!_rightPlaying) rightScore--;
                    
                    hudHiScore.text = "High Score: " + hiScore;
                    if (_leftPlaying) leftPoints.text = "<color=white>Red Ball: " + leftScore + "</color>";
                    if (!_leftPlaying) leftPoints.text = "<color=red>Red Ball: " + leftScore + "</color>";
                    if (_rightPlaying) rightPoints.text = "<color=white>Blue Ball: " + rightScore + "</color>";
                    if (!_rightPlaying) rightPoints.text = "<color=red>Blue Ball: " + rightScore + "</color>";
                    if (!_leftPlaying && !_rightPlaying) SwitchState(State.GAMEOVER);
                    period = 0f;
                }
                if(!_paused) period += Time.deltaTime;
                break;
            case State.LEVELCOMPLETE:
                break;
            case State.GAMEOVER:
                break;
        }
    }

    void EndState()
    {
        switch (_state)
        {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                CalulateHighScore();
                GameObject[] go = GameObject.FindGameObjectsWithTag("Track");
                foreach (GameObject track in go) Destroy(track);
                if (_ball1 != null) Destroy(_ball1);
                if (_ball2 != null) Destroy(_ball2);
                break;
            case State.LEVELCOMPLETE:
                levelLength += 10; if (levelLength > 1000) levelLength = 1000;
                levelTwistyness += 5; if (levelTwistyness > 90) levelTwistyness = 90;
                LevelSpeed += .25f; if (LevelSpeed > 20) LevelSpeed = 20;
                LEVEL.BuildLevel(0, 0, 1, levelLength, levelTwistyness);
                LEVEL.BuildLevel(100, 0, 2, levelLength, levelTwistyness);
                //_leftScore = 0;
                //_rightScore = 0;
                _leftPlaying = true;
                _rightPlaying = true;
                _paused = true;
                break;
            case State.GAMEOVER:
                break;
        }
    }

    public void PlayGame()
    {
        SwitchState(State.PLAY);
    }
    public void ResetGame()
    {
        SwitchState(State.INIT);
    }
    public void PauseGame()
    {
        if (!leftGoButton.activeSelf && !rightGoButton.activeSelf)
        {
            Paused = !Paused;
            pausePanel.SetActive(Paused);
        }
    }
    public void GoBAll()
    {
        rightGoButton.SetActive(false);
        rightLeftButton.SetActive(false);
        rightRightButton.SetActive(true);
        leftGoButton.SetActive(false);
        leftLeftButton.SetActive(false);
        leftRightButton.SetActive(true);
        _paused = false;
    }
    public void TurnBall1()
    {
        if (_ball1.transform.rotation.y == 0)
        {
            _ball1.GetComponent<Ball>()._targetRotation = Quaternion.Euler(0, 90, 0);
            rightGoButton.SetActive(false);
            rightLeftButton.SetActive(true);
            rightRightButton.SetActive(false);
        }
        else
        {
            _ball1.GetComponent<Ball>()._targetRotation = Quaternion.identity;
            rightGoButton.SetActive(false);
            rightLeftButton.SetActive(false);
            rightRightButton.SetActive(true);
        }
    }
    public void TurnBall2()
    {
        if (_ball2.transform.rotation.y == 0)
        {
            _ball2.GetComponent<Ball>()._targetRotation = Quaternion.Euler(0, 90, 0);
            leftGoButton.SetActive(false);
            leftLeftButton.SetActive(true);
            leftRightButton.SetActive(false);
        }
        else
        {
            _ball2.GetComponent<Ball>()._targetRotation = Quaternion.identity;
            leftGoButton.SetActive(false);
            leftLeftButton.SetActive(false);
            leftRightButton.SetActive(true);
        }
    }
    public void BlowUpBall()
    {
        GetComponent<AudioSource>().PlayOneShot(defeatSound);
    }
    public void CalulateHighScore()
    {        
        savedHiScore = PlayerPrefs.GetInt("highscore");
        if (savedHiScore > hiScore) hiScore = savedHiScore;
        if (gameScore > hiScore) hiScore = gameScore;
        PlayerPrefs.SetInt("highscore", hiScore);
    }
    public void GetVolumeSettings()
    {
        soundVolume = PlayerPrefs.GetFloat("soundvolume");
        musicVolume = PlayerPrefs.GetFloat("musicvolume");
    }
    public void SetVolumeSettings()
    {
        PlayerPrefs.SetFloat("soundvolume", soundVolume);
        PlayerPrefs.SetFloat("musicvolume", musicVolume);
    }
    public void ClearHiScore()
    {
        PlayerPrefs.DeleteKey("highscore");
    }
}
