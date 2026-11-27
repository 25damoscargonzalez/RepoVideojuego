using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaConLlave : MonoBehaviour
{
    [Header("Interfaz y Objetos")]
    public GameObject iconoLlaveUI;
    public GameObject puertaBloqueada;
    public GameObject puertaDesbloqueada;
    public GameObject canvasVictoria;
    public GameObject canvasHUD;
    public GameObject aviso;

    MovimientoJugPrincipal jugadorEnPuerta;
    bool abierta = false;
    bool victoria = false;

    void Start()
    {
        if (puertaBloqueada != null)
        {
            puertaBloqueada.SetActive(true);
        }

        if (puertaDesbloqueada != null)
        {
            puertaDesbloqueada.SetActive(false);
        }

        if (canvasVictoria != null)
        {
            canvasVictoria.SetActive(false);
        }

        if (aviso != null)
        {
            aviso.SetActive(false);
        }
    }

    void Update()
    {
        if (victoria)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }

        if (jugadorEnPuerta != null && Input.GetKeyDown(KeyCode.W))
        {
            if (!abierta && jugadorEnPuerta.tieneLlave)
            {
                AbrirPuerta();
            }
            else if (abierta)
            {
                GanarPartida();
            }
        }
    }

    void AbrirPuerta()
    {
        abierta = true;
        jugadorEnPuerta.tieneLlave = false;
        jugadorEnPuerta.SumarPuntos(200);

        if (puertaBloqueada != null)
        {
            puertaBloqueada.SetActive(false);
        }

        if (puertaDesbloqueada != null)
        {
            puertaDesbloqueada.SetActive(true);
        }

        if (iconoLlaveUI != null)
        {
            iconoLlaveUI.SetActive(false);
        }
    }

    void GanarPartida()
    {
        victoria = true;

        if (aviso != null)
        {
            aviso.SetActive(false);
        }

        if (canvasHUD != null)
        {
            canvasHUD.SetActive(false);
        }

        if (canvasVictoria != null)
        {
            canvasVictoria.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    void OnTriggerEnter2D(Collider2D dentro)
    {
        if (dentro.CompareTag("Player"))
        {
            jugadorEnPuerta = dentro.GetComponent<MovimientoJugPrincipal>();

            if (!victoria && aviso != null)
            {
                aviso.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D dentro)
    {
        if (dentro.CompareTag("Player"))
        {
            jugadorEnPuerta = null;

            if (aviso != null)
            {
                aviso.SetActive(false);
            }
        }
    }
}
