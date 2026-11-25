using UnityEngine;

public class BotonPresion : MonoBehaviour
{
    public GameObject sueloQueDesaparece;
    public GameObject objetoBotonNormal;
    public GameObject objetoBotonPulsado;

    int objetosEncima = 0;

    void Start()
    {
        ActualizarEstadoBoton();
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Player") || otro.CompareTag("Caja"))
        {
            objetosEncima++;
            ActualizarEstadoBoton();
        }
    }

    void OnTriggerExit2D(Collider2D otro)
    {
        if (otro.CompareTag("Player") || otro.CompareTag("Caja"))
        {
            objetosEncima--;
            if (objetosEncima < 0)
            {
                objetosEncima = 0;
            }
            ActualizarEstadoBoton();
        }
    }

    void ActualizarEstadoBoton()
    {
        bool presionado = objetosEncima > 0;

        if (sueloQueDesaparece != null)
        {
            sueloQueDesaparece.SetActive(!presionado);
        }

        if (objetoBotonNormal != null)
        {
            objetoBotonNormal.SetActive(!presionado);
            Debug.Log("BOTON NORMAL");
        }
        if (objetoBotonPulsado != null)
        {
            objetoBotonPulsado.SetActive(presionado);
            Debug.Log("BOTON PULSADO");
        }
    }
}
