using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimacion : MonoBehaviour {

    public float escalaVelocidad = 0.25f;
    public AnimationCurve aCurva;
    private Transform tranform;
    private float grado;
    private float objEscala;

    public void Start()
    {
        tranform = this.transform;
    }

    private void Update()
    {
        grado += escalaVelocidad * Time.deltaTime;
        objEscala = aCurva.Evaluate(grado);
        tranform.localScale = new Vector2(objEscala, objEscala);
        if (grado >= 1)
        {
            grado = 0;
        }
    }
}
