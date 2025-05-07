using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Mermi h�z�
    public float lifetime = 5f; // Merminin �mr�

    private void Start()
    {
        // Mermiyi ileri do�ru hareket ettir
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;

        // Belirli bir s�re sonra mermiyi yok et
        Destroy(gameObject, lifetime);
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        // �arp��ma oldu�unda mermiyi yok et
        Destroy(gameObject);
    }*/
}
