using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    //##################################### VARIABLES NECESARIAS PARA EL JUEGO ############################################################
    public float nivelStartDelay = 2f; //Va indicar cuanto tiempo va estar la pantalla "day 1.." en funcionamiento
    public float turnoDelay = 0.1f; //Decima de segundo//Que va a representar la espera que tendrá que hacer el GamManager entre el movimiento y movimiento, es decir, lo que dura entre turnos
    public static GameManager instancia = null;
    public BoardManager boardScript;
    public int puntosVida = 100; //Variable para mantener el nivel de vida, entre escena y escena
    [HideInInspector] public bool turnoJugador = true; //Variable indica el turno del jugador
    private int nivel = 0; //Nivel del juego
    private List<Enemigo> enemigos; //Lista de los enemigos que llamamos a la clases Enemigo
    private bool movimientoEnemigos; 
    private bool doingSetup; 

    //##################################### REFERNCIAS A LOS LAYOUTS ######################################################################
    private GameObject quitbutton;
    private Text nivelTexto; 
    private GameObject nivelImagen; 
    private Text vidaTexto;
    private GameObject btnReinicio;
    private GameObject btnRegresarMenu; //
    public bool destruir = false;
    private GameObject musicaOnOff;
    private Text textoMusica;

    //##################################### HIGHSCORE ######################################################################
    private int highscore;
    private bool nuevoHighscore;
    private Text highscoreTexto;

    //Al iniciar la pantalla de juego mostrara la imagen donde aparece el nivel y depues de 1 desativará el canvas y al momento lo activará, esto me permitirá ver la pantalla del nivel
    IEnumerator Start()
    {
        nivelImagen.SetActive(true);
        yield return new WaitForSeconds(1);
        GameObject canvas = GameObject.Find("Canvas");
        canvas.SetActive(false);
        canvas.SetActive(true);;
        destruir = true;
    }

    //Resetear el nivel cuando le damos al boton de reiniciar
    public void LevelReset()
    {
        Jugador.FoodReset();

        enabled = true;
        nivel = 0;
        vidaTexto = GameObject.Find("VidaTexto").GetComponent<Text>();
        vidaTexto.text = "Vida: " + puntosVida;
        SceneManager.LoadScene("Juego");
    }

    void Awake()
    {
        if (instancia == null)
            instancia = this;
        else if (instancia != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        enemigos = new List<Enemigo>();

        highscore = PlayerPrefs.HasKey("Highscore") ? PlayerPrefs.GetInt("Highscore") : nivel;
        boardScript = GetComponent<BoardManager>();
        //InitGame(); //Lo comento para poder ejecutar el juego correctamente cuando lo incio desde otra escena (controles) y si lo ejecuto es misma escena(Juego) lo descomento para que no crashee
    }

    void OnLevelWasLoaded(int index)
    {
        nivel++;
        if (nivel > highscore)
        {
            highscore = nivel;
            nuevoHighscore = true;
        }
        InitGame();
    }

    void InitGame()
    {
        doingSetup = true;

        try
        {
            quitbutton = GameObject.Find("BtnSalir");
            btnReinicio = GameObject.Find("BtnReiniciar");
            btnRegresarMenu = GameObject.Find("BtnMenu");
            nivelImagen = GameObject.Find("NivelImagen");
            nivelTexto = GameObject.Find("NivelTexto").GetComponent<Text>();
            highscoreTexto = GameObject.Find("HighscoreText").GetComponent<Text>();


        nivelTexto.text = "Día " + nivel + " ¡¡Ha por la libertat!!";

        if (nuevoHighscore)
        {
            highscoreTexto.text = "¡¡¡Nuevo Record!!!";
        }
        else
        {
            highscoreTexto.text = "Score: " + highscore;
        }
        nivelImagen.SetActive(true);
        Invoke("EsconderNivelImagen", nivelStartDelay);

        enemigos.Clear();
        boardScript.SetupScene(nivel);
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Not set in the inspector");
        }
    }

    private void EsconderNivelImagen()
    {
        try
        {
            nivelImagen.SetActive(false);
            doingSetup = false;
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Not set in the inspector");
        }
    }

    public void GameOver()
    {
        nivelTexto.text = "Después " + nivel + " días, tendré que volver a empezar.";
        quitbutton.SetActive(true);
        btnReinicio.SetActive(true);
        btnRegresarMenu.SetActive(true);
        nivelImagen.SetActive(true);
        enabled = false;
    }

    void Update()
    {
        //
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            float volume = SoundManager.instancia.musicSource.volume;
            SoundManager.instancia.musicSource.volume = (float)System.Math.Round((volume - 0.1f), 1);
            volume = SoundManager.instancia.efxSource.volume;
            SoundManager.instancia.efxSource.volume = (float)System.Math.Round((volume - 0.1f), 1);

        }
        else if (Input.GetKeyUp(KeyCode.KeypadPlus))
        {
            float volume = SoundManager.instancia.musicSource.volume;
            SoundManager.instancia.musicSource.volume = (float)System.Math.Round((volume + 0.1f), 1);
            volume = SoundManager.instancia.efxSource.volume;
            SoundManager.instancia.efxSource.volume = (float)System.Math.Round((volume + 0.1f), 1);

        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            musicaOnOff = GameObject.Find("Musiconoff");
            textoMusica = musicaOnOff.transform.Find("Text").GetComponent<Text>();
            if (textoMusica.text == "Música On")
            {
                textoMusica.text = "Música Off";
                SoundManager.instancia.musicSource.Stop();
                SoundManager.instancia.efxSource.volume=0;

            }
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            musicaOnOff = GameObject.Find("Musiconoff");
            textoMusica = musicaOnOff.transform.Find("Text").GetComponent<Text>();
            if (textoMusica.text == "Música Off")
            {
                textoMusica.text = "Música On";
                SoundManager.instancia.musicSource.Play();
                SoundManager.instancia.efxSource.volume = 1;
            }
        }

        try
        {
            Text volumeText = GameObject.Find("VolumeText").GetComponent<Text>();
            volumeText.text = "Volumen: " + SoundManager.instancia.musicSource.volume * 100;
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Not set in the inspector");
        }
        
        

        if (turnoJugador || movimientoEnemigos || doingSetup)
            return;

        StartCoroutine(MoverEnemigos());
    }

    //Añade enemigos a la lista, llama a la clase de enemigo
    public void AddEnemigoToList(Enemigo script)
    {
        enemigos.Add(script);
    }

    //Esto nos permitirá que cuando el jugador mueva 2 turnos el enemigo empieza a mover-se
    IEnumerator MoverEnemigos()
    {
        movimientoEnemigos = true;
        yield return new WaitForSeconds(turnoDelay);
        if (enemigos.Count == 0)
        {
            yield return new WaitForSeconds(turnoDelay);
        }

        for (int i = 0; i < enemigos.Count; i++)
        {
            enemigos[i].MovimientoEnemigo();
            yield return new WaitForSeconds(enemigos[i].tiempoMovimiento);
        }

        turnoJugador = true;
        movimientoEnemigos = false;
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("Highscore", highscore);
    }
}