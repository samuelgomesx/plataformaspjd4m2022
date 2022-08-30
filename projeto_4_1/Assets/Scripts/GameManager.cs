using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe usada para gerenciar o jogo
/// </summary>

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // instancia do singleton
    
    
    [SerializeField] 
    private string guiName; // nome da fase de interface;

    [SerializeField]
    private string levelName; // nome da fase de jogo;

    [SerializeField] 
    private GameObject playerAndCameraPrefab; // referencia pro prefab do jogador + camera

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

        } ; 
        
        
        
       

        // 3 - começar a partida
    }

    private void StartGameFromInitialization()
    {
        SceneManager.LoadScene("Splash");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
