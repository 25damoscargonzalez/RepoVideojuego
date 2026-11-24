using UnityEngine;

public class TrampasDia : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D dentro)
    {
        if (dentro.CompareTag("Player"))
        {
            MovimientoJugPrincipal jugador = dentro.GetComponent<MovimientoJugPrincipal>();
            if (jugador != null)
            {
                jugador.RecibirDano(3);
            }
        }
    }
}
