using UnityEngine;

public class MovimientoJugPrincipal : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public float distCaja;
    public Vector2 tamannoCaja;
    public LayerMask capaSuelo;

    // Vida y corazones UI
    [Header("Vida")]
    public int vidaMaxima = 3;
    public GameObject[] corazones;
    int vidaActual;

    // Power-Up Super Salto
    float fuerzaSaltoOriginal;
    float tiempoSuperSalto;
    bool superSaltoActivo;

    //LeerMovimiento
    [Header("LeerMovimiento")]
    Vector3 movimiento;
    bool moviendose;
    Rigidbody2D cuerpo;
    SpriteRenderer sprite;
    Animator anim;
    float direccion = 1;   // 1 = derecha, -1 = izquierda

    // LeerSalto()
    bool saltando;

    // AtacandoNormal()
    [Header("AtacandoNormal")]
    public float duracionAtaque = 0.3f;
    public float distCajaAtaque = 0.8f;
    public Vector2 tamannoCajaAtaque = new Vector2(1.2f, 1.2f);
    public LayerMask capaEnemigos;

    bool atacando;
    float tiempoAtaque;
    bool enSuelo;

    // AtacandoPorAire()
    [Header("AtacandoPorAire")]
    public float fuerzaCaida = 10f;

    bool atacandoAire;

    // Deslizando()
    [Header("Deslizando")]
    public float duracionDeslizando = 0.4f;
    public float fuerzaDeslizando = 10f;

    float tiempoDeslizando;
    bool deslizando;

    // Morir()
    [Header("Morir")]
    public Transform puntoReaparicion;
    public GameObject camaraMapa;
    public GameObject camaraHabitacion;
    public float duracionMuerte = 1f;

    float tiempoMuerte;
    bool muerto;

    // Start is called before the first frame update
    void Start()
    {
        movimiento = new Vector3();
        cuerpo = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        vidaActual = vidaMaxima;
        ActualizarCorazones();

        fuerzaSaltoOriginal = fuerzaSalto;
    }

    // Update is called once per frame
    void Update()
    {
        if (muerto)
        {
            tiempoMuerte -= Time.deltaTime;
            if (tiempoMuerte <= 0)
            {
                Reaparecer();
            }

            ActualizarAnim();
            return;
        }

        enSuelo = EstaEnElSuelo();

        ControlarSuperSalto();
        LeerMovimiento();
        RotarPersonaje();
        LeerSalto();
        AtacandoNormal();
        AtacandoPorAire();
        Deslizando();

        ActualizarAnim();

    }

    public void ActivarSuperSalto(float multiplicador, float duracion)
    {
        fuerzaSalto = fuerzaSaltoOriginal * multiplicador;
        tiempoSuperSalto = duracion;
        superSaltoActivo = true;
        sprite.color = new Color(0.5f, 0.9f, 1f); 
    }

    void ControlarSuperSalto()
    {
        if (superSaltoActivo)
        {
            tiempoSuperSalto -= Time.deltaTime;
            if (tiempoSuperSalto <= 0)
            {
                fuerzaSalto = fuerzaSaltoOriginal;
                superSaltoActivo = false;
                sprite.color = Color.white; 
            }
        }
    }

    private void LeerMovimiento()
    {
        if (Input.GetKey(KeyCode.A))
        {
            movimiento = Vector3.left;
            moviendose = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movimiento = Vector3.right;
            moviendose = true;
        }
        else
        {
            movimiento = new Vector3(); //Se para al soltar las teclas
            moviendose = false;
        }
    }

    private void RotarPersonaje()
    {
        if (movimiento.x < 0)
        {
            sprite.flipX = true;
            direccion = -1;
        }
        else if (movimiento.x > 0)
        {
            sprite.flipX = false;
            direccion = 1;
        }
    }

    private void LeerSalto()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !saltando && EstaEnElSuelo())
        {
            saltando = true;
        }
    }

    void AtacandoNormal()
    {
        if (Input.GetMouseButtonDown(0) && enSuelo && !atacando)
        {
            atacando = true;
            tiempoAtaque = duracionAtaque;

            Vector2 centro = transform.position + Vector3.right * direccion * distCajaAtaque;
            Collider2D[] enemigosGolpeados = Physics2D.OverlapBoxAll(centro, tamannoCajaAtaque, 0, capaEnemigos);

            foreach (Collider2D enemigo in enemigosGolpeados)
            {
                MovimientoEnemigo scriptEnemigo = enemigo.GetComponent<MovimientoEnemigo>();
                if (scriptEnemigo != null)
                {
                    scriptEnemigo.RecibirDano(1);
                }
            }
        }

        if (atacando)
        {
            tiempoAtaque -= Time.deltaTime;
            if (tiempoAtaque <= 0)
            {
                atacando = false;
            }
        }
    }


    void AtacandoPorAire()
    {
        if (Input.GetKeyDown(KeyCode.F) && !enSuelo && !atacandoAire)
        {
            atacandoAire = true;
        }

        if (atacandoAire && enSuelo)
        {
            atacandoAire = false;
        }

    }

    void Deslizando()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && enSuelo && moviendose && !deslizando)
        {
            deslizando = true;
            tiempoDeslizando = duracionDeslizando;
        }

        if (deslizando)
        {
            tiempoDeslizando -= Time.deltaTime;
            if (tiempoDeslizando <= 0)
            {
                deslizando = false;
            }
        }
    }

    public void RecibirDano(int cantidad)
    {
        if (muerto) return;
        vidaActual -= cantidad;
        ActualizarCorazones();

        Debug.Log("Vidas restantes = " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void ActualizarCorazones()
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            corazones[i].SetActive(i < vidaActual);
        }
    }

    public void Morir()
    {
        if (muerto) return;
        muerto = true;
        tiempoMuerte = duracionMuerte;
        movimiento = Vector3.zero;
        cuerpo.velocity = Vector2.zero;
        anim.SetBool("Muerto", true);
    }

    void Reaparecer()
    {
        transform.position = puntoReaparicion.position;
        cuerpo.velocity = Vector2.zero;

        camaraMapa.SetActive(false);
        camaraHabitacion.SetActive(true);

        atacando = false;
        atacandoAire = false;
        deslizando = false;
        vidaActual = vidaMaxima;
        ActualizarCorazones();

        anim.SetBool("Muerto", false);
        muerto = false;
    }

    bool EstaEnElSuelo()
    {
        RaycastHit2D rc = Physics2D.BoxCast(transform.position, tamannoCaja, 0, Vector2.down, distCaja, capaSuelo);
        if (rc)
        {
            //Debug.Log("Suelo");
            return true;
        }
        else
        {
            //Debug.Log("No toca el sueloooooooooo");
            return false;
        }
    }

    void ActualizarAnim()
    {
        if (movimiento.x != 0)
        {
            anim.SetBool("Moviendo", true);
        }
        else
        {
            anim.SetBool("Moviendo", false);
        }

        if (!EstaEnElSuelo())
        {
            anim.SetBool("Saltando", true);
        }
        else
        {
            anim.SetBool("Saltando", false);
        }

        anim.SetBool("Atacando", atacando);

        anim.SetBool("AtacandoAire", atacandoAire);

        anim.SetBool("Deslizando", deslizando);

        anim.SetBool("Muerto", muerto);
    }

    void FixedUpdate() //Todo el codigo relacionado con las fisicas
    {
        cuerpo.velocity = new Vector2(movimiento.x * velocidad, cuerpo.velocity.y);

        if (saltando)
        {
            cuerpo.AddForce(Vector2.up * fuerzaSalto);
            saltando = false;
        }

        //Ataque aereo
        if (atacandoAire)
        {
            cuerpo.velocity = new Vector2(cuerpo.velocity.x, -fuerzaCaida);
        }

        if (deslizando)
        {
            cuerpo.velocity = new Vector2(direccion * fuerzaDeslizando, cuerpo.velocity.y);
        }

    }

    void OnDrawGizmos()
    {
        if (EstaEnElSuelo())
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireCube(transform.position + Vector3.down * distCaja, tamannoCaja);
        Gizmos.color = Color.white;

        //Gizmo ataqueNormal
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + Vector3.right * direccion * distCajaAtaque, tamannoCajaAtaque);
        Gizmos.color = Color.white;

    }

}
