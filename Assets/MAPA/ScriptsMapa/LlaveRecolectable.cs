using UnityEngine;

public class LlaveRecolectable : MonoBehaviour
{
    public GameObject iconoLlave;

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Player"))
        {
            MovimientoJugPrincipal jugador = otro.GetComponent<MovimientoJugPrincipal>();
            if (jugador != null)
            {
                jugador.tieneLlave = true;
                jugador.SumarPuntos(50);
            }

            if (iconoLlave != null)
            {
                iconoLlave.SetActive(true);
            }

            gameObject.SetActive(false);
        }
    }
}
