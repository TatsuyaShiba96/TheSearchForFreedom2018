using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour 
{
    public float VelocidadMovimiento = 10f; // Qué tan rápido debe moverse la cámara del punto A al B.
    public float DistanciaRapida = 0.25f; // Qué tan lejos de la nueva posición deberíamos estar antes de ajustarla.
    public Transform EjePrincipal; // Eje que mueve la cámara
    public Transform EjeTemblor; // Eje que hace temblar la cámara

    // Para mover la cámara
    public bool HayMovimiento { get; private set; }
    private Vector3 nuevaPosicion;
    private float velocidadActual;

    // Para hacer temblar la camara
    private bool hayTemblor = false;
    private int contarTemblores;
    private float intensidadTemblor, velocidadTemblor, _baseX, _baseY;
    private Vector3 siguienteTemblorPosicion;


	void Start () 
    {
        enabled = false;

        // Configura las posiciones de base, estas se usan para agitar para determinar a dónde volver después de un batido.
        _baseX = EjeTemblor.localPosition.x;
        _baseY = EjeTemblor.localPosition.y;
	}
	
	
	void Update () 
    {
        // Comprueva si nos estamos moviendo
        if (HayMovimiento)
        {
            // Movernos hacia la nueva posición
            EjePrincipal.position = Vector3.MoveTowards(EjePrincipal.position, nuevaPosicion, Time.deltaTime * velocidadActual);

            // Determinar si estamos allí o no (a una distancia rápida)
            if (Vector2.Distance(EjePrincipal.position, nuevaPosicion) < DistanciaRapida)
            {
                EjePrincipal.position = nuevaPosicion;
                HayMovimiento = false;
                if(!hayTemblor) enabled = false;
            }
        }
        // Comprueva si estamos temblando o (Podría ser ambos)
        if (hayTemblor)
        {
            // Avanzar hacia la siguiente posición de batido previamente determinada
            EjeTemblor.localPosition = Vector3.MoveTowards(EjeTemblor.localPosition, siguienteTemblorPosicion, Time.deltaTime * velocidadTemblor);

            // Determine if we are there or not
            if (Vector2.Distance(EjeTemblor.localPosition, siguienteTemblorPosicion) < intensidadTemblor / 5f)
            {
                // Determinar si estamos allí o no
                contarTemblores--;

                // Si terminamos de temblar, apáguelo si ya no nos movemos
                if (contarTemblores <= 0)
                {
                    hayTemblor = false;
                    EjeTemblor.localPosition = new Vector3(_baseX, _baseY, EjeTemblor.localPosition.z);
                    if (!HayMovimiento) enabled = false;
                }
                // Si solo queda 1 temblor, regrese a la base o estado inicial
                else if (contarTemblores <= 1)
                {
                    siguienteTemblorPosicion = new Vector3(_baseX, _baseY, EjeTemblor.localPosition.z);
                }
                // Si no hemos terminado o no estamos cerca, determine la siguiente posición para viajar a
                else
                {
                    DetermineNextShakePosition();
                }
            }
        }
	}
         // Mueva la cámara en cierta dirección en una cierta distancia
         // Distancia a lo largo del eje x para mover
         // Distancia a lo largo del eje y para mover
         // Qué tan rápido moverse en la dirección especificada
    public void Move(float x, float y, float speed = 0)
    {
        // Si se pasa una velocidad, úsalo. De lo contrario, use el valor predeterminado.
        if (speed > 0) velocidadActual = speed;
        else velocidadActual = VelocidadMovimiento;

        // Configurarnos para movernos
        nuevaPosicion = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
        HayMovimiento = true;
        enabled = true;
    }


    // Establece inmediatamente la posición de la cámara

    public void SetPosition(Vector2 position)
    {
        Vector3 nuevaPosicion = new Vector3(position.x, position.y, EjePrincipal.position.z);
        EjePrincipal.position = nuevaPosicion;
    }




     // Sacude la cámara. Esencialmente coloca algunos puntos aleatorios alrededor de la cámara y se los da a ellos
     // Distancia máxima desde el punto central que recorrerá la cámara
     // Número total de puntos aleatorios a los que la cámara viajará
     // Qué tan rápido se mueve la cámara de un punto a otro
    public void Shake(float intensity, int shakes, float speed)
    {
        enabled = true;
        hayTemblor = true;
        contarTemblores = shakes;
        intensidadTemblor = intensity;
        velocidadTemblor = speed;

        DetermineNextShakePosition();
    }


    private void DetermineNextShakePosition()
    {
        siguienteTemblorPosicion = new Vector3(Random.Range(-intensidadTemblor, intensidadTemblor),
            Random.Range(-intensidadTemblor, intensidadTemblor),
            EjeTemblor.localPosition.z);
    }
}