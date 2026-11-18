using UnityEngine;

public class MarcoMapa : MonoBehaviour
{

    public Vector2 tamanno = new Vector2();
    public Color color = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireCube(transform.position, tamanno);
    }
}
