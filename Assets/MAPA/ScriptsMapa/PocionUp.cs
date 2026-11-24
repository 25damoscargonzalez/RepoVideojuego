using UnityEngine;

public class PocionUp : MonoBehaviour
{
    public float multiplicadorSalto = 1.6f; 
    public float duracionPoder = 10f;       
    public float tiempoReaparicion = 12f;   

    SpriteRenderer sprite;
    Collider2D colisionador;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        colisionador = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D dentro)
    {
        if (dentro.CompareTag("Player"))
        {
            MovimientoJugPrincipal jugador = dentro.GetComponent<MovimientoJugPrincipal>();
            if (jugador != null)
            {
                jugador.ActivarSuperSalto(multiplicadorSalto, duracionPoder);
            }

            sprite.enabled = false;
            colisionador.enabled = false;

            if (tiempoReaparicion > 0)
            {
                Invoke("ReactivarPocion", tiempoReaparicion);
            }
        }
    }

    void ReactivarPocion()
    {
        sprite.enabled = true;
        colisionador.enabled = true;
    }
}
