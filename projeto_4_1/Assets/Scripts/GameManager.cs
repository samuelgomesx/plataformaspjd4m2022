using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum GameState
{
    Initialization,
    Running,
    Victory,
    GameOver
}

/// <summary>
/// Classe usada para gerenciar o jogo
/// </summary>

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // instancia do singleton

    public GameState GameState
    {
        get => _gameState;
        set
        {
            if (value == _gameState) return;
            _gameState = value;
            OnGameStateChanged();
        }
    }

    public int CoinsTowin;
    public float TimeToLose;
    
    [SerializeField] 
    private string guiName; // nome da fase de interface;

    [SerializeField]
    private string levelName; // nome da fase de jogo;

    [SerializeField] 
    private GameObject playerAndCameraPrefab; // referencia pro prefab do jogador + camera

    private GameState _gameState; // variavel que guarda o estado atual do game manager
    private float _currentTime;
    private void OnEnable()
    {
        PlayerObserveManager.OnCoinsChanged += PlayerCoinsUpdate;
    }

   

    private void OnDisable()
    {
        PlayerObserveManager.OnCoinsChanged -= PlayerCoinsUpdate;
    }


    private void Awake()
    {
        // condição de criação do singleton
        if (Instance == null)
        {
            Instance = this;
            // Imoede que o objeto indicado entre parenteses seja destruido
            DontDestroyOnLoad(this.gameObject);// referenxia  pro objeto que contem o gameManager
        }
        else Destroy(this.gameObject);
    }

    void Start()
    {
        // se estiver iniciando a cena a partir de Initialization, carregue o jogo
        // do jeito de antes 
        if(SceneManager.GetActiveScene().name == "Initialization")
            StartGameFromInitialization();
        else // caso contrario, esta iniciando a partir do level, carregue o jogo do modo apropriado 
            StartGameFromLevel();
        
    

    }

    private void StartGameFromLevel()
    {
        // 1 - carrega a cena de interface do modo adtivo para nao apagar a cena do level da memoria ram
        SceneManager.LoadScene(guiName, LoadSceneMode.Additive);
        // 2 - precisa instanciar o jogador na cena 
        // começa proucurar o objeto playerStart na cena do level
        Vector3 playerStartPosition = GameObject.Find("PlayerStart").transform.position;
           
        // instancia o prefab do jogador na posiçao do player start com rotação zerada
        Instantiate(playerAndCameraPrefab, playerStartPosition, Quaternion.identity);
        
        GameState = GameState.Running;
    }

    public void StartGame()
    {
        // impede que o objeto GameManager entre parenteses seja destruido
        DontDestroyOnLoad(this.gameObject); // referencia pro objeto que contem o GameManager
        // 1 - carregar a cena de interface e do jogo
        SceneManager.LoadScene(guiName);
        // SceneManager.LoadScene(levelName, LoadSceneMode.Additive); // additive carrega uma nova cena

        SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive).completed += operation =>
        {
            // inicializa a variavel para guardar a cena do level com o valor padrao (default)
            Scene levelScene = default;
                   
                   
            // encontrar a cena de level que estar carregando
            // for que itera no array as cenas abertas
            for (int i = 0; 1 < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == levelName)
                {
                    levelScene = SceneManager.GetSceneAt(i);
                    break;
                }
            }
            // se  variavel tivere um valor diferente do padão, significa que ela foi alterada
            // e a cena do level foi encontrada no array, entao faça ela ser a 
            // nova cena ativa
            if(levelScene != default) SceneManager.SetActiveScene(levelScene);
           
            // 2 - precisa instanciar o jogador na cena 
            Vector3 playerStartPosition = GameObject.Find("PlayerStart").transform.position;
           
            // instancia o prefab do jogador na posiçao do player start com rotação zerada
            Instantiate(playerAndCameraPrefab, playerStartPosition, Quaternion.identity);
            // 3 - começar a partida
            GameState = GameState.Running;
        } ; 
        
        
        
       

        // 3 - começar a partida
      
    }

    private void StartGameFromInitialization()
    {
        GameState = GameState.Initialization;
        SceneManager.LoadScene("Splash");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
    private void PlayerCoinsUpdate(int obj)
    {
        if (obj >=CoinsTowin) GameState= GameState.Victory;
    }

    private void OnGameStateChanged()
    {
        switch (_gameState)
        {
            case GameState.Running:
                ResetTime();
                ResumeGame();
                break;
            case GameState.Victory:
                PauseGame();
                LoasVictoryScene();
                break;
            case GameState.GameOver:
                PauseGame();
                LoadGameOverScene();
                break;
            
                
        }
    }

    private void LoasVictoryScene()
    {
        SceneManager.LoadScene("Victory", LoadSceneMode.Additive);
    }

    private void ResetTime()
    {
        _currentTime = TimeToLose;
    }

    private void Update()
    {
        if (GameState == GameState.Running)
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime <= 0)
            {
                GameState = GameState.GameOver;
            }
        }

        if (GameState == GameState.Victory)
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
               StartGame(); 
            }
        }
        
        if (GameState == GameState.GameOver)
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                GameState = GameState.Initialization;
                ResumeGame();
                LoadMainMenu();
            }
        }
    }

    
    private void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
    }








}


