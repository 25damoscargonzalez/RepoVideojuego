using UnityEngine;

public class LlaveRecolectable : MonoBehaviour
{
    public GameObject iconoLlave; // Arrastraremos aquí IconoLlaveUI del Canvas

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Player"))
        {
            if (iconoLlave != null)
            {
                iconoLlave.SetActive(true);
            }

            gameObject.SetActive(false);
        }
    }
}
