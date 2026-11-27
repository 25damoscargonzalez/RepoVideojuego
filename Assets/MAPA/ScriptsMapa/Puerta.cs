using UnityEngine;

public class Puerta : MonoBehaviour
{
    [Header("Destino")]
    public Transform puntoAparicion;
    public Puerta puertaDestino;

    [Header("Cámaras")]
    public GameObject camaraZona;

    [Header("Opcional")]
    public GameObject aviso;

    Transform jugador;

    void Start()
    {
        if (aviso != null)
        {
            aviso.SetActive(false);
        }
    }

    void Update()
    {
        if (jugador != null && Input.GetKeyDown(KeyCode.W))
        {
            Teletransportar();
        }
    }

    void Teletransportar()
    {
        Rigidbody2D rb = jugador.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        jugador.position = puertaDestino.puntoAparicion.position;

        camaraZona.SetActive(false);
        puertaDestino.camaraZona.SetActive(true);

        if (aviso != null)
        {
            aviso.SetActive(false);
        }
        jugador = null;
    }

    void OnTriggerEnter2D(Collider2D dentro)
    {
        if (dentro.CompareTag("Player"))
        {
            jugador = dentro.transform;
            if (aviso != null)
            {
                aviso.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D dentro)
    {
        if (dentro.CompareTag("Player"))
        {
            jugador = null;
            if (aviso != null)
            {
                aviso.SetActive(false);
            }

        }
    }
}