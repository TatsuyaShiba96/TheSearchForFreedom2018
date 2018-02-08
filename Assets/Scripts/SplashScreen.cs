using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {


    //Varaibles para crear el scena
    public float timer = 0f; //Contador del tiempo
    public string loadScene = "MenuJuego"; //Nombre de la siguiente scena
    public float porcentaje;

    //Variables para printar por pantalla 
    public Text porcentajeText;
    public Text loadingText;
    public GameObject bar;

    void Update()
    {

        //Si el porcentaje es menor a 100
        if (porcentaje < 100)
        {
            porcentaje += Time.deltaTime * 15;
        }
        if (porcentaje >= 100)
        {
            porcentaje = 100;
            StartCoroutine("DisplayScene");
        }
        porcentajeText.text = "" + (int)porcentaje + "%";
        bar.transform.localScale = new Vector3(porcentaje/100, 1, 1); //x, y, z
    }

    IEnumerator DisplayScene()
    {
        yield return new WaitForSeconds(timer);

        SceneManager.LoadScene(loadScene);
        //Application.LoadLevel(loadScene);
    }
}
