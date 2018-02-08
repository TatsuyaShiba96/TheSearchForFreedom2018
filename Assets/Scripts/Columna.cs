using UnityEngine;
using System.Collections;

public class Columna : MonoBehaviour
{
    public Sprite dmgSprite;
    public Sprite dmgSprite2;
    public int hp = 12;
    public AudioClip sonidoGolpe1;
    public AudioClip sonidoGolpe2;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        //Aqui podemos al referencia del sprite ubicacion donde se cambia
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //Metodo que llamara el jugador cuando se quiera dar golpes al muro
    public void DamageMuro(int perdida) //Le pasamos cuanto daño resivira el muro
    {
        //Sonido al picar el muro
        SoundManager.instancia.RandomizeSfx(sonidoGolpe1, sonidoGolpe2);
        hp -= perdida; //Descincrementamos el hp a partir los puntos de daños perdidos
        if (hp == 8) //Cuando llege la vida a 8 cambia a otro script el muro actual
        {
            spriteRenderer.sprite = dmgSprite;
        }
        else if (hp == 4)
        {
            spriteRenderer.sprite = dmgSprite2;
        }
        else if (hp <= 0) //Columna destruida
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}