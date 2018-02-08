using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour
{
    public float tiempoMovimiento = 0.1f;//Tiempo en que va a tarda de moverse entre casillas 0.1f decima de segundo
    private float tiempoMovimientoInverso; //Velocidad de movimiento, tarda 0.1f en pasar a otra casilla

    public LayerMask blockingLayer; //Tipo layerMask para ser referencia a blockingLayer
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;

    //Obtenemos la referencia del boxCollaider
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        tiempoMovimientoInverso = 1f / tiempoMovimiento;
    }

    //Metodo para interntar mover al jugador o al enemigo
    //Tipo protected para que no herede de la clase MovingObject
    //Si se a movido sera true y sino un false
    protected bool Move(int xDir, int yDir, out RaycastHit2D golpear)
    {
        Vector2 inicio = transform.position; //Donde está colocado el objecto
        Vector2 fin = inicio + new Vector2(xDir, yDir); //Cuando (0,0) + (1,0) = (1,0) suma de vectores

        boxCollider.enabled = false; //Para envitar que nuestro personaje se encuentre con su propio boxCollaider
        golpear = Physics2D.Linecast(inicio, fin, blockingLayer); //Para toparse con los boxCollaider que van de un incio a un fin
        boxCollider.enabled = true; //Volmemos a activar nuestro boxCollaider del player

        if (golpear.transform == null) //Si nos da null significa que no ha encontrato un boxCollarider objecto que pertenece al blockingLayer
        {
            //Aqui tendremos que realizar el movimiento
            //Corrutinas: nos permite ejecutar metodos y dentro de esos metodos decirle que queremos que se detenga por el momento para continuar en el siguiente fotograma
            StartCoroutine(MovimientoNormal(fin)); //Se encarga de hacer la animación de movimiento
            return true;
        }

        return false; //No podemos movernos
    }

    protected IEnumerator MovimientoNormal(Vector3 fin) //Si quiero que sea accesible la corrutina desde cualquier clase y no sea accesible desde otras clases que no tenga nada que ver
    {
        float distanciaRestante = (transform.position - fin).sqrMagnitude; //Distancia que falta

        while (distanciaRestante > float.Epsilon) //Nos estamos moviendo mientras la distancia que queda siga siendo mayor a 0 "avanzamos un poco (pero si nos queda muy poco los valroes serán muy pequeños, por lo tanto usaremos la variable 'float.Epsilon')" 
        {
            //Vamos a moverlo y tenemos que calcular a donde lo vamos a mover
            Vector3 nuevaPosicion = Vector3.MoveTowards(rb2D.position, fin, tiempoMovimientoInverso * Time.deltaTime);

            //Ahora lo movemos
            rb2D.MovePosition(nuevaPosicion);

            //Ahora actualizar el valor de 'distanciaRestante' (recalcular la distancia restante)
            distanciaRestante = (transform.position - fin).sqrMagnitude;

            //Ahora decirle a la corrutina que se detenga para que continue su ejecución en el siguiente fotograma
            yield return null;
        }
    }

    protected virtual void IntentoMover<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D golpear;
        bool puedoMover = Move(xDir, yDir, out golpear);

        //Ahora necesitamos si se a podido mover o no
        //*Modificación, Si no se puede mover
        if (golpear.transform == null)
        {
            return;
        }

        T golpearComponente = golpear.transform.GetComponent<T>();

        if (!puedoMover && golpearComponente != null)
        {

            //Con que es diferente para el player y el enemigo tendremos que hacer un metodo abstracto
            //Porque el player lo que le interesa es si se a encontrado un bloque y pordelo destruir
            //En cambio para el enemigo lo que le intersa es si se a topado con el player para poderlo dañar
            OnCantMove(golpearComponente);
        }
    }

    //Metodo que no permitica que el canMove devuelva un true
    protected abstract void OnCantMove<T>(T component)
        where T : Component;
}