using UnityEngine;

public class CajaRompible : MonoBehaviour
{
    public float tiempoReaparicion = 1.5f; 

    Vector3 posicionInicial;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    Collider2D colisionador;

    void Start()
    {
        posicionInicial = transform.position;

        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        colisionador = GetComponent<Collider2D>();
    }

    public void RomperYReaparecer()
    {
        sprite.enabled = false;
        colisionador.enabled = false;
        rb.simulated = false;

        Invoke("ReaparecerCaja", tiempoReaparicion);
    }

    void ReaparecerCaja()
    {
        transform.position = posicionInicial;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        sprite.enabled = true;
        colisionador.enabled = true;
        rb.simulated = true;
    }
}
