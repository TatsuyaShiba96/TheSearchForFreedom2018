using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGame : MonoBehaviour {

    public string nombreScena = "";

    public void Empezar()
    {
        SceneManager.LoadScene(nombreScena);
        //Application.LoadLevel(nombreScena);
    }

    public void Salir()
    {
        Debug.Log("Salir");
        Application.Quit();
    }

    
}
