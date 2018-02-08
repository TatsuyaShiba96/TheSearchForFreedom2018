using UnityEngine;
using System.Collections;

public class Enemigo : MovingObject
{
    public int playerDamage;
    private bool skipMove; //De forma que los enemigos se muevan un turno si un turno no (turno del enemigo)

    private Animator animator; //Para activar las animaciones
    private Transform objetivo; //Va a referencia al tranforms del jugador, para saber donde esta el jugador y asi moverse a su dirección
    public AudioClip enemyAttack1; //Audio del enemigo 1 cuando ataca
    public AudioClip enemyAttack2; //Audio 2 del enemigo 2 cuando ataca

    protected override void Start() //Tambien lo tenemos que soobrescribir ya que tambien existe en la clase base heredada
    {
        //Para poder añadir a nuestra lista enemigos 
        GameManager.instancia.AddEnemigoToList(this); //(This) hace referencia a toda la clase de Enemigo
        animator = GetComponent<Animator>();
        objetivo = GameObject.FindGameObjectWithTag("Jugador").transform; //Le paso el tag del player
        base.Start();
    }
    
    protected override void IntentoMover<T>(int xDir, int yDir)
    {
        //Antes tenemos que decirle que turno hace el enemigo
        if (skipMove) //Si hay que saltarse el movimiento
        {
            skipMove = false; //Desactive la variable para que en el siguiente turno no entre
            return; //Para que lo de abajo base... no se ejecute
        }
        base.IntentoMover<T>(xDir, yDir); //codigo que hace el movimiento

        //Una vez que se haga movimiento
        skipMove = true;  //Lo salte
    }

    //Quien va a mover los enemigos? Se va a encargar el gameManager
    public void MovimientoEnemigo()
    {
        //Para mover al enemigo tenemos que saber a que dirección se va a mover
        int xDir = 0;
        int yDir = 0;

        //Ahora necesitamos saber calcular en que direccións se va amover en base de donde esta el jugador haciendo referencia a su transform
        //Lo que vamos hacer esque el enemigo se mueva arriva y abajo cuando el jugador se encuentre en la misma columna en el que esta el enemigo
        //Primero vamos a comprovar si el jugador esta en la misma columna  en la que esta el enemigo que tiene este script
        if (Mathf.Abs(objetivo.position.x - transform.position.x) < float.Epsilon) //Posicion del jugador en x(horizontal) le voy a resta mi posicion en x que será del enemigo, conque es valor si es muy pequeño ponemo el float.Epsilon que significará que están en la misma columna
        //El Math.Abs(Valor absoluto) sirve para que si la resta anterior da negativo pues esto lo va a devolver a positivo
        {
            //Se va mover en vertical el enemigo hacia donde esta el jugador, necesitamos saber si ell jugador esta encima o debajo del enemigo
            yDir = objetivo.position.y > transform.position.y ? 1 : -1; //Si el jugador está por escima del enemigo, el enemigo tendra que mover hacia arriba hacia el player por lo tanto tenemos que darle un valor positivo (1) y sino pues darle un valor negativo (-1)
        }
        else //No está en la misma columna el player con el enemigo
        {
            //Necesitamos saber si el jugador está a la izquierda o a la derecha
            xDir = objetivo.position.x > transform.position.x ? 1 : -1; 
        }
        IntentoMover<Jugador>(xDir, yDir);
    }

    //En caso de que el enemigo no pueda moverse por un obstaculo, pues se le llamara al enemigo el metodo onCanMove y se le pasara el objeto que no le deja moverse
    protected override void OnCantMove<T>(T component)
    {
        Jugador hitPlayer = component as Jugador;
        hitPlayer.PerdidaVida(playerDamage);

        //Activar el trigger de ataque del enemigo al player
        animator.SetTrigger("enemyAttack");
        SoundManager.instancia.RandomizeSfx(enemyAttack1, enemyAttack2); //Realiza el sonido cuandoa ataca
    }
}