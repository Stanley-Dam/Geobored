using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Bullet : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject hitFX;
    [SerializeField] private AudioSource audioS;
    private int damage;
    private float bulletSpeed;
    private GameObject bulletOwner;
    #endregion

    private void Start()
    {
        //plays sound fx with random pitch
        float pitch = Random.Range(0.9f, 1.1f);
        audioS.pitch = pitch;
        audioS.Play(0);
    }

    private void Update()
    {
        this.transform.position += transform.right * bulletSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10 && collision.gameObject.GetComponent<Bullet>().BulletOwner == bulletOwner)
            Physics2D.IgnoreCollision(this.GetComponent<CircleCollider2D>(), collision.gameObject.GetComponent<CircleCollider2D>());
        else
        {
            bulletSpeed = 0;

            ContactPoint2D contact = collision.GetContact(0);
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;

            if (hitFX)
            {
                GameObject hitFXIns = Instantiate(hitFX, pos, rot);
                try
                {
                    Color color = collision.transform.GetComponent<SpriteRenderer>().color;
                    hitFXIns.GetComponent<PlaySoundParticel>().ParticelColor = color;
                }
                catch
                {
                    hitFXIns.GetComponent<PlaySoundParticel>().ParticelColor = Color.white;
                }

            }
            if (collision.gameObject.layer == 9 && collision.gameObject != bulletOwner)
            {
                PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
                player.TakeDamage(damage);
                player.Killer = bulletOwner;
            }
            Destroy(this.gameObject);
        }
    }

    public int SetDamage { get { return damage; } set { damage = value; } }
    public float SetBulletSpeed { get { return bulletSpeed; } set { bulletSpeed = value; } }
    public GameObject BulletOwner { get { return bulletOwner; } set { bulletOwner = value; } }
}