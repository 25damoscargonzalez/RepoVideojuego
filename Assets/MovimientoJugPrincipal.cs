using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugPrincipal : MonoBehaviour
{

    public float velocidad;

    Vector3 movimiento;
    Rigidbody2D cuerpo;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        movimiento = new Vector3();
        cuerpo = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        LeerMovimiento();
        RotarPersonaje();
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

    void FixedUpdate() //Todo el codigo relacionado con las fisicas
    {
        cuerpo.velocity = new Vector2(movimiento.x * velocidad, cuerpo.velocity.y);
    }

}
