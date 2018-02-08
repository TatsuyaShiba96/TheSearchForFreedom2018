using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class ControlesGame : MonoBehaviour {


    public string nombreScena = "";

    public void Empezar()
    {
        
        SceneManager.LoadScene(nombreScena);
        try {
            GameManager.instancia.LevelReset();
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("myLight was not set in the inspector");
        }
        
    }
}
