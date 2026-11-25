using UnityEngine;

public class PuertaConLlave : MonoBehaviour
{
    public GameObject iconoLlave; 
    public GameObject puertaBloqueada;
    public GameObject puertaDesbloqueada;

    bool jugadorEnPuerta = false;
    bool abierta = false;

    void Start()
    {
        if (puertaBloqueada != null) puertaBloqueada.SetActive(true);
        if (puertaDesbloqueada != null) puertaDesbloqueada.SetActive(false);
    }

    void Update()
    {
        if (jugadorEnPuerta && !abierta && Input.GetKeyDown(KeyCode.W))
        {
            if (iconoLlave != null && iconoLlave.activeSelf)
            {
                AbrirPuerta();
            }
            else
            {
                Debug.Log("Necesitas la llave para abrir esta puerta");
            }
        }
    }

    void AbrirPuerta()
    {
        abierta = true;

        if (puertaBloqueada != null) puertaBloqueada.SetActive(false);
        if (puertaDesbloqueada != null) puertaDesbloqueada.SetActive(true);

        if (iconoLlave != null) iconoLlave.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Player"))
        {
            jugadorEnPuerta = true;
        }
    }

    void OnTriggerExit2D(Collider2D otro)
    {
        if (otro.CompareTag("Player"))
        {
            jugadorEnPuerta = false;
        }
    }
}
