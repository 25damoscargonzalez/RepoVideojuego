using UnityEngine;

public class PalancaInteractiva : MonoBehaviour
{
    public GameObject palancaIzq;          
    public GameObject palancaDer;          
    public GameObject sueloQueDesaparece;  

    bool encendida = false;

    void Start()
    {
        ActualizarEstadoPalanca(); 
    }

    public void AlternarPalanca()
    {
        encendida = !encendida;
        ActualizarEstadoPalanca();
    }

    void ActualizarEstadoPalanca()
    {
        if (palancaIzq != null)
        {
            palancaIzq.SetActive(!encendida);
        }

        if (palancaDer != null)
        {
            palancaDer.SetActive(encendida);
        }

        // Si está encendida (derecha), el suelo desaparece ///////////// si está apagada (izquierda), vuelve a aparecer
        if (sueloQueDesaparece != null)
        {
            sueloQueDesaparece.SetActive(!encendida);
        }
    }
}
