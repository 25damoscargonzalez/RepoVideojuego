using UnityEngine;

public class MovimientoJugPrincipal : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public float distCaja;
    public Vector2 tamannoCaja;
    public LayerMask capaSuelo;

    Vector3 movimiento;
    Rigidbody2D cuerpo;
    SpriteRenderer sprite;
    Animator anim;

    // LeerSalto()
    bool saltando;

    // AtacandoNormal()
    public float duracionAtaque = 0.3f;

    bool atacando;
    float tiempoAtaque;
    bool enSuelo;

    // AtacandoPorAire()
    public float fuerzaCaida = 10f;  

    bool atacandoAire;

    // Deslizando()
    public float duracionDeslizando = 0.4f;

    float tiempoDeslizando;
    bool deslizando;

    // Start is called before the first frame update
    void Start()
    {
        movimiento = new Vector3();
        cuerpo = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        enSuelo = EstaEnElSuelo();

        LeerMovimiento();
        RotarPersonaje();
        LeerSalto();
        AtacandoNormal();
        AtacandoPorAire();
        Deslizando();

        ActualizarAnim();
        
    }

    private void LeerMovimiento()
    {
        if(Input.GetKey(KeyCode.A))
        {
            movimiento = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movimiento = Vector3.right;
        }
        else
        {
            movimiento = new Vector3(); //Se para al soltar las teclas
        }
    }

    private void RotarPersonaje()
    {
        if(movimiento.x < 0)
        {
            sprite.flipX = true;
        } else if (movimiento.x > 0)
        {
            sprite.flipX = false;
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
        // Empieza el ataque: solo en el suelo, y si no está ya atacando
        if (Input.GetMouseButtonDown(0) && enSuelo && !atacando)
        {
            atacando = true;
            tiempoAtaque = duracionAtaque;
        }

        // Cuenta atrás: cuando termina el golpe, se apaga
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && enSuelo && !deslizando)
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

    bool EstaEnElSuelo()
    {
        RaycastHit2D rc = Physics2D.BoxCast(transform.position, tamannoCaja, 0, Vector2.down, distCaja, capaSuelo);
        if (rc) 
        {
            Debug.Log("Suelo");
            return true;
        }
        else
        {
            Debug.Log("No toca el sueloooooooooo");
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
    }

    void FixedUpdate() //Todo el codigo relacionado con las fisicas
    {
        cuerpo.velocity = new Vector2(movimiento.x * velocidad, cuerpo.velocity.y);
        if (saltando)
        {
            cuerpo.AddForce(Vector2.up * fuerzaSalto);
            saltando = false;
        }

        //Ataque aereo: cae mas rapido
        if (atacandoAire)
        {
            cuerpo.velocity = new Vector2(cuerpo.velocity.x, -fuerzaCaida);
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
    }

}
