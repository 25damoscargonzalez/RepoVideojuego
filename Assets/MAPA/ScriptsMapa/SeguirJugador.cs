using UnityEngine;

public class SeguirJugador : MonoBehaviour
{
    public Transform jugador;
    public float suavizado = 5f;
    public Vector2 desplazamiento = new Vector2(0, 1f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        Vector3 destino = new Vector3(
            jugador.position.x + desplazamiento.x,
            jugador.position.y + desplazamiento.y,
            transform.position.z);   

        transform.position = Vector3.Lerp(transform.position, destino, suavizado * Time.deltaTime);
    }

}
