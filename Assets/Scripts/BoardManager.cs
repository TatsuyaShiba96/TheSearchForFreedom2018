using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        //Variables que ajustaremos el nivel de Random range de la comida y columnas que aparecerán en el tablero.
        public int minimo=0; 
        public int maximo=0;

        public Count(int min, int max)
        {
            minimo = min;
            maximo = max;
        }
    }

    //##################################### VARIABLES PARA CREAR EL TABLERO ######################################################################
    public int columnas = 8;
    public int filas = 8;
    public Count columnasCount = new Count(5, 9);
    public Count comidaCount = new Count(1, 5);
    public GameObject salida;
    public GameObject[] sueloTiles;
    public GameObject[] columnaTiles;
    public GameObject[] comidaTiles;
    public GameObject[] enemigoTiles;
    public GameObject[] muroExteriorTiles;

    private Transform boardHolder;
    private List<Vector3> gridPosiciones = new List<Vector3>();

    //Este metodo permite crear el mapa 
    void InitialiseList()
    {
        gridPosiciones.Clear();

        for (int x = 1; x < columnas - 1; x++)
        {
            for (int y = 1; y < filas - 1; y++)
            {
                gridPosiciones.Add(new Vector3(x, y, 0f));
            }
        }
    }

    //Metodo que sea el que cree el escenaria incial
    void BoardSetup()
    {
        boardHolder = new GameObject("Tablero").transform; //Crea un GameObject para que meta todo lo que se genera en Unity en un canvas aparte.

        //Necesitaremos un bucle que recorra las x (horizontal) y dentro otro bucle que recorra la y (vertical)
        //Si el area en el que se mueve el player es 10x10, quiero tambien incluir en esa area el borde
        for (int x = -1; x < columnas + 1; x++)
        {
            for (int y = -1; y < filas + 1; y++)
            {
                //Referencia al objecto que vamos a instanciar uno de los 'prefabs' de forma aleatoria
                GameObject toInstanciar = sueloTiles[Random.Range(0, sueloTiles.Length)];

                //Vamos a comprovar si las posicion x,y es un posicion que pertenece al borde
                //Como sabemos que estamos el borde? Cuando la x vale -1, la y vale -1
                if (x == -1 || x == columnas || y == -1 || y == filas)
                {
                    //vamos a cambiar la instancia para que haga referencia los muros exteriores
                    toInstanciar = muroExteriorTiles[Random.Range(0, muroExteriorTiles.Length)];
                }
                //Tenemos que instanciar el objecto que tenemos referencia a intanciarCelda, donde en la posición (x,y) y rotación (ninguno)
                GameObject instancia = Instantiate(toInstanciar, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Una vez que lo tenemos instanciado necesitaresmo guardar su referencia
                instancia.transform.SetParent(boardHolder);
            }
        }
    }

    //Metodo que va a devolver un posición aleatoria
    Vector3 RandomPosicion()
    {
        //Va a obtener un numero aleatorio entre 0 y el total de elementos que hay en la lista GridPosition
        int randomIndex = Random.Range(0, gridPosiciones.Count);
        Vector3 randomPosicion = gridPosiciones[randomIndex];

        //Ahora tenemos que eliminar de la lista el elemento que esta en randomIndex porque sino nos aparecerá un elemento encima de otro
        gridPosiciones.RemoveAt(randomIndex);
        return randomPosicion;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimo, int maximo)
    {
        int contarObjeto = Random.Range(minimo, maximo + 1);//Para que cojar por ejemplo entre 3 minimo y 5 maximo para cojer el 5 sumamos 1

        for (int i = 0; i < contarObjeto; i++)
        {
            Vector3 randomPosicion = RandomPosicion();
            GameObject eleccionTile = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(eleccionTile, randomPosicion, Quaternion.identity);
        }
    }

    public void SetupScene(int nivel)
    {
        //Aqui pondremos el codigo que se encargará de sprintar el mapa
        BoardSetup(); //Crear suelo y muro alrededor
        InitialiseList(); //Inicializamos la lista que borra y mete de nuevo todas las posiciones donde se pueden generar elementos
        LayoutObjectAtRandom(columnaTiles, columnasCount.minimo, columnasCount.maximo);
        LayoutObjectAtRandom(comidaTiles, comidaCount.minimo, comidaCount.maximo);
        int contadorEnemigo = (int)Mathf.Log(nivel, 2f); //Numero de enemigos en los diferente levels la mitad o de forma logaritmica (int)Mathf.Log(level, 2)
        LayoutObjectAtRandom(enemigoTiles, contadorEnemigo, contadorEnemigo);
        Instantiate(salida, new Vector3(columnas - 1, filas - 1, 0F), Quaternion.identity);
    }

}