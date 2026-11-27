using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class MovimientoJugPrincipal : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public float distCaja;
    public Vector2 tamannoCaja;
    public LayerMask capaSuelo;

    //Camaras
    [Header("Camaras")]
    public Camera camHabitacion;
    public GameObject camMapaDia;

    //Puntuacion
    [Header("Puntuacion")]
    public TextMeshProUGUI textoPuntosHUD;
    public TextMeshProUGUI textoPuntosVictoria;
    public TextMeshProUGUI textoPuntosDerrota;
    int puntosActuales = 0;

    //Vida y corazones UI
    [Header("Vida")]
    public int vidaMaxima = 3;
    public GameObject[] corazones;
    int vidaActual;

    //Power-Up Super Salto
    [Header("PowerUp")]
    public GameObject iconoPocionUp;
    public TextMeshProUGUI textoContadorPocion;

    float fuerzaSaltoOriginal;
    float tiempoSuperSalto;
    bool superSaltoActivo;

    //Llave
    [Header("Inventario")]
    public bool tieneLlave = false;

    //LeerMovimiento
    [Header("LeerMovimiento")]
    Vector3 movimiento;
    bool moviendose;
    Rigidbody2D cuerpo;
    SpriteRenderer sprite;
    Animator anim;
    float direccion = 1; // 1 = derecha, -1 = izquierda

    //LeerSalto()
    bool saltando;

    //AtacandoNormal()
    [Header("AtacandoNormal")]
    public float duracionAtaque = 0.3f;
    public float distCajaAtaque = 0.8f;
    public Vector2 tamannoCajaAtaque = new Vector2(1.2f, 1.2f);
    public LayerMask capaEnemigos;

    bool atacando;
    float tiempoAtaque;
    bool enSuelo;

    //AtacandoPorAire()
    [Header("AtacandoPorAire")]
    public float fuerzaCaida = 10f;
    public float distCajaAtaqueAire = 0.8f;
    public Vector2 tamannoCajaAtaqueAire = new Vector2(1.8f, 1f);

    bool atacandoAire;
    bool danoAireAplicado;

    //Deslizando()
    [Header("Deslizando")]
    public float duracionDeslizando = 0.4f;
    public float fuerzaDeslizando = 10f;

    float tiempoDeslizando;
    bool deslizando;

    //Morir()
    [Header("Morir")]
    public GameObject canvasDerrota;
    public GameObject canvasHUD;
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
        ActualizarPuntosUI();

        fuerzaSaltoOriginal = fuerzaSalto;

        if (camHabitacion != null)
        {
            camHabitacion.gameObject.SetActive(true);
            camHabitacion.orthographicSize = 3f;
        }

        if (camMapaDia != null)
        {
            camMapaDia.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (muerto)
        {
            tiempoMuerte -= Time.deltaTime;
            if (tiempoMuerte <= 0)
            {
                if (canvasDerrota != null)
                {
                    canvasDerrota.SetActive(true);
                }

                if (canvasHUD != null)
                {
                    canvasHUD.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
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

    public void SumarPuntos(int cantidad)
    {
        puntosActuales += cantidad;
        ActualizarPuntosUI();
    }

    void ActualizarPuntosUI()
    {
        if (textoPuntosHUD != null)
        {
            textoPuntosHUD.text = "Puntos: " + puntosActuales;
        }

        if (textoPuntosVictoria != null)
        {
            textoPuntosVictoria.text = puntosActuales.ToString();
        }

        if (textoPuntosDerrota != null)
        {
            textoPuntosDerrota.text = puntosActuales.ToString();
        }
    }


    public bool TieneSuperSalto()
    {
        return superSaltoActivo;
    }

    public void ActivarSuperSalto(float multiplicador, float duracion)
    {
        if (superSaltoActivo) return;

        fuerzaSalto = fuerzaSaltoOriginal * multiplicador;
        tiempoSuperSalto = duracion;
        superSaltoActivo = true;
        sprite.color = new Color(0.5f, 0.9f, 1f);

        if (iconoPocionUp != null) iconoPocionUp.SetActive(true);
    }

    void ControlarSuperSalto()
    {
        if (superSaltoActivo)
        {
            tiempoSuperSalto -= Time.deltaTime;

            if (textoContadorPocion != null)
            {
                textoContadorPocion.text = Mathf.CeilToInt(tiempoSuperSalto).ToString();
            }

            if (tiempoSuperSalto <= 0)
            {
                fuerzaSalto = fuerzaSaltoOriginal;
                superSaltoActivo = false;
                sprite.color = Color.white;

                if (iconoPocionUp != null) iconoPocionUp.SetActive(false);
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

            foreach (Collider2D obj in enemigosGolpeados)
            {
                MovimientoEnemigo scriptEnemigo = obj.GetComponent<MovimientoEnemigo>();
                if (scriptEnemigo != null)
                {
                    scriptEnemigo.RecibirDano(1);
                }

                CajaRompible scriptCaja = obj.GetComponent<CajaRompible>();
                if (scriptCaja != null)
                {
                    scriptCaja.RomperYReaparecer();
                }

                PalancaInteractiva scriptPalanca = obj.GetComponent<PalancaInteractiva>();
                if (scriptPalanca != null)
                {
                    scriptPalanca.AlternarPalanca();
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
            danoAireAplicado = false;
        }

        if (atacandoAire && !danoAireAplicado)
        {
            Vector2 centro = transform.position + Vector3.down * distCajaAtaqueAire;
            Collider2D[] enemigosGolpeados = Physics2D.OverlapBoxAll(centro, tamannoCajaAtaqueAire, 0, capaEnemigos);

            if (enemigosGolpeados.Length > 0)
            {
                foreach (Collider2D enemigo in enemigosGolpeados)
                {
                    MovimientoEnemigo scriptEnemigo = enemigo.GetComponent<MovimientoEnemigo>();
                    if (scriptEnemigo != null)
                    {
                        scriptEnemigo.RecibirDano(2); 
                    }
                }
                danoAireAplicado = true; 
            }
        }

        if (atacandoAire && enSuelo)
        {
            atacandoAire = false;
            danoAireAplicado = false;
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

        //Gizmo ataqueAire
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + Vector3.down * distCajaAtaqueAire, tamannoCajaAtaqueAire);
        Gizmos.color = Color.white;

    }

}
