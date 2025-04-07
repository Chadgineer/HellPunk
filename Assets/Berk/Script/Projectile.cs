using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Mermi hýzý
    public float lifetime = 5f; // Merminin ömrü

    private void Start()
    {
        // Mermiyi ileri doðru hareket ettir
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;

        // Belirli bir süre sonra mermiyi yok et
        Destroy(gameObject, lifetime);
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        // Çarpýþma olduðunda mermiyi yok et
        Destroy(gameObject);
    }*/
}
