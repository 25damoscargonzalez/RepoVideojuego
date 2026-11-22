using UnityEngine;

public class TrampasDia : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D dentro)
    {
        if (dentro.CompareTag("Player"))
        {
            dentro.GetComponent<MovimientoJugPrincipal>().Morir();
        }
    }
}