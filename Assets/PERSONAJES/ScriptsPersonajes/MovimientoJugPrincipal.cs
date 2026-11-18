using System.Collections;
using System.Collections.Generic;
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

    bool saltando;

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
        LeerMovimiento();
        RotarPersonaje();
        LeerSalto();
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
            movimiento = new Vector3(); //Se para en seco tras soltar las teclas
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

    }

    void FixedUpdate() //Todo el codigo relacionado con las fisicas
    {
        cuerpo.velocity = new Vector2(movimiento.x * velocidad, cuerpo.velocity.y);
        if (saltando)
        {
            cuerpo.AddForce(Vector2.up * fuerzaSalto);
            saltando = false;
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
