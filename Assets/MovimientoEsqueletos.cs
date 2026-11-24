using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    public float velocidad = 1.5f;
    public float distCajaAtaque = 1f;
    public Vector2 tamannoCajaAtaque = new Vector2(1f, 1.5f);
    public float distanciaPatrulla = 3f;
    public LayerMask capaJugador;

    float posInicialX;
    int direccion = 1;
    int pointTo;
    float movX;
    Rigidbody2D cuerpo;
    SpriteRenderer sprite;
    Animator anim;

    //Ataque
    bool atacando;

    //Vida enemigo
    public int vidaMaxima = 3;
    int vidaActual;

    //Muere
    bool muerto;

    void Start()
    {
        cuerpo = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        posInicialX = transform.position.x;
        vidaActual = vidaMaxima;
    }

    void Update()
    {
        if (muerto) return;

        atacando = DetectarJugador() != null;

        if (atacando)
        {
            movX = 0; // Paro el personaje para que ataque
        }
        else
        {
            LeerMovimiento();
            RotarPersonaje();
        }

        anim.SetBool("Moviendo", movX != 0 && !atacando);
        anim.SetBool("Atacando", atacando);
    }

    void LeerMovimiento()
    {
        if (pointTo == 0)
        {
            movX = -1;
            if (transform.position.x <= posInicialX - distanciaPatrulla) pointTo = 1;
        }
        else
        {
            movX = 1;
            if (transform.position.x >= posInicialX + distanciaPatrulla) pointTo = 0;
        }
    }

    void RotarPersonaje()
    {
        if (movX < 0)
        {
            sprite.flipX = true;
            direccion = -1;
        }
        else if (movX > 0)
        {
            sprite.flipX = false;
            direccion = 1;
        }
    }

    Collider2D DetectarJugador()
    {
        Vector2 centro = transform.position + Vector3.right * direccion * distCajaAtaque;
        return Physics2D.OverlapBox(centro, tamannoCajaAtaque, 0, capaJugador);
    }

    public void Atacar()
    {
        if (muerto) return;

        Collider2D jugadorDetectado = DetectarJugador();
        if (jugadorDetectado != null)
        {
            MovimientoJugPrincipal scriptJugador = jugadorDetectado.GetComponent<MovimientoJugPrincipal>();
            if (scriptJugador != null)
            {
                scriptJugador.RecibirDano(1);
            }
        }
    }
    public void RecibirDano(int cantidad)
    {
        if (muerto) return;

        vidaActual -= cantidad;
        anim.SetTrigger("Hit");
        Debug.Log("Vidas ESQUELETOO = " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    public void Morir()
    {
        if (muerto) return;
        muerto = true;
        cuerpo.velocity = Vector2.zero;
        anim.SetBool("Muere", true);
        GetComponent<Collider2D>().enabled = false;
        cuerpo.simulated = false;
        Destroy(gameObject, 2f);
    }

    void FixedUpdate()
    {
        if (muerto) return;
        float velX = atacando ? 0 : movX * velocidad;
        cuerpo.velocity = new Vector2(velX, cuerpo.velocity.y);
    }

    void OnDrawGizmos()
    {
        if (DetectarJugador())
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireCube(transform.position + Vector3.right * direccion * distCajaAtaque, tamannoCajaAtaque);
        Gizmos.color = Color.white;
    }
}
