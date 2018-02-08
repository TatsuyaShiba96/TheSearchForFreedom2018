using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource; // Fuente de sondio
    public AudioSource musicSource; // Que musica se reproducirá en ese lugar
    public static SoundManager instancia = null;
    public float lowPitchRange = 0.95f; //Para controlar el nivel de volumen (rango de tono bajo)
    public float highPitchRange = 1.05f; //Para controlar el nivel de volumen (rango de tono alto)

    void Awake()
    {
        if (instancia == null)
            instancia = this;
        else if (instancia != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }
    
    //Metodo para que la musica empieze a ejecutarse
    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }
    
    //Metodo que hara un random de los nivel de tono alto y bajo, de manera que, nos permitira en un futuro asignar a botones para que controlen el nivel de volumen
    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }
}