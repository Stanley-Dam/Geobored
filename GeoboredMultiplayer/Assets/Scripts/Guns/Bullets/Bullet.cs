using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Variables
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected int damage;
    protected GameObject hitFX;
    protected AudioSource audioS;
    protected GameObject bulletOwner;
    #endregion

    private void Awake()
    {
        hitFX = (GameObject)Resources.Load("Bullets/FX/hitFX");
        audioS = this.GetComponent<AudioSource>();

        //plays sound fx with random pitch
        float pitch = Random.Range(0.9f, 1.1f);
        audioS.pitch = pitch;
        audioS.Play(0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10 && collision.gameObject.GetComponent<Bullet>().BulletOwner == bulletOwner)
        {
            Physics2D.IgnoreCollision(this.GetComponent<CircleCollider2D>(), collision.gameObject.GetComponent<CircleCollider2D>());
        }
        else
        {
            bulletSpeed = 0;

            ContactPoint2D contact = collision.GetContact(0);
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;

            if (hitFX)
            {
                var hitFXIns = Instantiate(hitFX, pos, rot);
                try
                {
                    Color color = collision.transform.GetComponent<SpriteRenderer>().color;
                    hitFXIns.GetComponent<PlaySoundParticel>().SetColor = color;
                }
                catch
                {
                    hitFXIns.GetComponent<PlaySoundParticel>().SetColor = Color.white;
                }

            }
            if (collision.gameObject.layer == 9 && collision.gameObject != bulletOwner)
                collision.gameObject.SendMessage("TakeDamage", damage);
            Destroy(this.gameObject);
        }
    }

    public GameObject BulletOwner { get { return bulletOwner; } set { bulletOwner = value; } }
}