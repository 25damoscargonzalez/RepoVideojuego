using UnityEngine;

public class Puerta : MonoBehaviour
{
    [Header("Destino")]
    public Transform puntoAparicion;      // dónde aparece el jugador al llegar a ESTA puerta
    public Puerta puertaDestino;          // la puerta a la que lleva esta

    [Header("Cámaras")]
    public GameObject camaraZona;         // Virtual Camera de la zona donde está esta puerta

    [Header("Opcional")]
    public GameObject aviso;              // texto "W"

    Transform jugador;

    void Start()
    {
        if (aviso != null) aviso.SetActive(false);
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
        if (rb != null) rb.velocity = Vector2.zero;

        jugador.position = puertaDestino.puntoAparicion.position;

        camaraZona.SetActive(false);
        puertaDestino.camaraZona.SetActive(true);

        if (aviso != null) aviso.SetActive(false);
        jugador = null;
    }

    void OnTriggerEnter2D(Collider2D dentro)
    {
        if (dentro.CompareTag("Player"))
        {
            jugador = dentro.transform;
            if (aviso != null) aviso.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D dentro)
    {
        if (dentro.CompareTag("Player"))
        {
            jugador = null;
            if (aviso != null) aviso.SetActive(false);
        }
    }
}