using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Jugador : MovingObject
{

    public float reiniciarNivelDelay = 1f; //Tiempo que tarda en pasar de nivel a otro
    public int puntosVidaComida = 10; //Numero de vida que incrementara cuando el jugador pase por encima de el scrip de comida
    public int puntosVidaPocion = 20; //Numero de vida que incrementara cuando el jugador pase por encima de el scrip de poción
    public int damageColuma = 1; //Daño que inflinge el jugado al scrip de columa
    private static int vida; //Que va a llevar la cuenta de la vida

    public Text vidaTexto; 
    public AudioClip movimientoSonido1;
    public AudioClip movimientoSonido2;
    public AudioClip comidaSonido1;
    public AudioClip comidaSonido2;
    public AudioClip pocionSonido1;
    public AudioClip pocionSonido2;
    public AudioClip sonidoGameOver;
    private Animator animator;

    public static void FoodReset()
    {
        vida = 100;
        SoundManager.instancia.musicSource.Play();
    }

    protected override void Start()
    {
        //Lo que vamos hacer es cargar el valor que tenga 'puntoDeVidaJugador' dentro de nuestra variable local 'vida' antes de empezar la escena
        animator = GetComponent<Animator>();

        //Lo que vamos hacer es cargar el valor que tenga 'playerFoodPoints' dentro de nuestra variable local 'food' antes de empezar la escena
        vida = GameManager.instancia.puntosVida;

        //Mostrar el total de la vida
        vidaTexto = GameObject.Find("VidaTexto").GetComponent<Text>();
        vidaTexto.text = "Vida: " + vida;
        base.Start();
    }
    //Cuando el jugador se desactive, significa que estamos cargando la misma escena pero con un nivel distinto
    private void OnDisable()
    {
        GameManager.instancia.puntosVida = vida; //Lleva la cuenta de la variable vida
    }

    //Movimiento del jugador, lo haremos cuando sea el turno del jugador
    private void Update()
    {
        if (!GameManager.instancia.turnoJugador)
        {
            return; //Salimos
        }
        else //Si el el turno del jugador
        {
            int horizontal = 0;
            int vertical = 0;

            //GetAxisRaw va a devorler 'ceros' si no estamos pulsando ni la izquierda ni la derecha y 'uno' si estamos clickando a la derecha y '-uno' si estamos clickeando a la izquierda
            horizontal = (int)(Input.GetAxisRaw("Horizontal"));

            //GetAxisRaw va a devorler 'ceros' si no estamos pulsando ni arriva ni abajo y 'uno' si estamos clickando a arriva y '-uno' si estamos clickeando a abajo
            vertical = (int)(Input.GetAxisRaw("Vertical"));

            //Ahora tendremos que evitar que el jugador se mueva en diagonal
            if (horizontal != 0)
            {
                vertical = 0; //Que nos de igual si hemos pulsado arriva o abajo
            }

            //Comprovar si hay que mover
            if (horizontal != 0 || vertical != 0)
            {
                //########################## Codigo a modificar para mover en todas las direcciones ######################
                animator.SetFloat("velocidadX", horizontal);
                animator.SetFloat("velocidadY", vertical);
                animator.SetBool("movimiento", true);
                IntentoMover<Columna>(horizontal, vertical);
            }
        }
    }
    

    protected override void IntentoMover<T>(int xDir, int yDir) //Cuando el jugador intenta moverse
    {
        vida--; //Desicrementaremos la vida

        //Mostrar el desecrimentar de la vida
        vidaTexto = GameObject.Find("VidaTexto").GetComponent<Text>();
        vidaTexto.text = "Vida: " + vida;

        //Movimiento
        //*Al metodo base le pasaremos el bool
        base.IntentoMover<T>(xDir, yDir);
        RaycastHit2D golpear;
        if (Move (xDir, yDir, out golpear))
        {
            SoundManager.instancia.RandomizeSfx(movimientoSonido1, movimientoSonido2);
        }

        //Tenemos que comprovar al metodo de checkifgameover()
        VerificarGameOver();

        //Tenemos que terminar el turno del jugador
        GameManager.instancia.turnoJugador = false;
    }

    //Resivimos la referencia al gameObject que esta al lugar que no hemos podido movernos
    protected override void OnCantMove<T>(T component)
    {
        Columna golpearMuro = component as Columna;
        golpearMuro.DamageMuro(damageColuma);
        animator.SetTrigger("jugadorGolpe");
    }

    //Ahora haremos un metodo que comprueva que el jugador esta en la casilla de food, soda o exit (sprites)
    private void OnTriggerEnter2D(Collider2D otro) //Lo comprovaremos con el Tag
    {
        if (otro.tag == "Salida") //Si other tiene el tag 'Salida'
        {
            //Ha llegado a la salida, lo que quiero es reiniciar el nivel
            //No lo quiero llamar de inmediato si no con algo de delay
            Invoke("Restart", reiniciarNivelDelay);

            //Desactivar el componente jugador, para que el usuario no pueda moverse más
            enabled = false;
        }
        else if (otro.tag == "Comida")
        {            
            vida += puntosVidaComida;

            //Mostrar el incremento de la comida
            vidaTexto = GameObject.Find("VidaTexto").GetComponent<Text>();
            vidaTexto.text = "Vida: " + vida + " " + "+" + puntosVidaComida;
            SoundManager.instancia.RandomizeSfx(comidaSonido1, comidaSonido2);

            //Vamos a desactivar el objeto que esta en esa casilla
            otro.gameObject.SetActive(false);//No se destruye sino se desactiva para evitar que el juego pierda rendimiento
        }
        
        else if (otro.tag == "Pocion")
        {
            vida += puntosVidaPocion;

            //Mostrar el incremento de la poción
            vidaTexto = GameObject.Find("VidaTexto").GetComponent<Text>();
            vidaTexto.text ="Vida: " + vida + " " + "+" + puntosVidaPocion;
            SoundManager.instancia.RandomizeSfx(pocionSonido1, pocionSonido2);
            otro.gameObject.SetActive(false);
        }
    }

    //Metodo para reiniciar la escena
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Recargamos la escena
        //Application.LoadLevel(Application.loadedLevel);
    }

    //Jugador que pierda vida cuando el enemigo lo golpee
    public void PerdidaVida(int perdida)
    {
        Camera.main.GetComponent<CameraControl>().Shake(0.5f, 20, 50);

        //Ahora tenemos que hacer referencia al trigger del animator que es golpeado
        animator.SetTrigger("jugadorGolpeado");
        vida -= perdida;

        //Mostrar cuando se pierde comida por el enemigo
        vidaTexto = GameObject.Find("VidaTexto").GetComponent<Text>();
        vidaTexto.text = "Vida: " + vida + " " + "-" + perdida;

        //Siempre que decrementamos la vida
        VerificarGameOver(); //En caso de haber perdido toda la vida
    }

    //Que nos permita comprovar si hemos llegado a GameOver
    private void VerificarGameOver()
    {
        if (vida <= 0)
        {
            SoundManager.instancia.PlaySingle(sonidoGameOver);
            SoundManager.instancia.musicSource.Stop();

            //Le pondremos sonido a gameOver
            GameManager.instancia.GameOver(); //Si la vida a llegado a 0
        }
    }
}