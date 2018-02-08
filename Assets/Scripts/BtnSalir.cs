using UnityEngine;
using System.Collections;

public class BtnSalir : MonoBehaviour {
    private GameObject btn;
    
	void Start ()
    {
        btn = GameObject.Find("BtnSalir");
        btn.SetActive(false);
	}

    public void Salir()
    {
        Debug.Log("Salir");
        Application.Quit();
    }
}
