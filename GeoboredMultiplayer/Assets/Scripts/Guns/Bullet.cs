using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Variables
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int damage;
    [SerializeField] private MultiPlayerBullet multiplayerBullet;
    private GameObject hitFX;
    private AudioSource audioS;
    #endregion

    private void Start() {
        //plays sound fx with random pitch
        hitFX = (GameObject)Resources.Load("Bullets/FX/hitFX");
        audioS = this.GetComponent<AudioSource>();
        float pitch = Random.Range(0.9f, 1.1f);

        audioS.pitch = pitch;
        audioS.Play(0);
    }

    // Update is called once per frame
    private void Update() {
        this.transform.position += transform.right * bulletSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
        if (collision.gameObject.layer == 9)
            collision.gameObject.SendMessage("TakeDamage", damage);

        if(multiplayerBullet != null && multiplayerBullet.GetBulletManagedByClient())
            multiplayerBullet.GetNetworkManager().BulletHitOnServer(multiplayerBullet);

        Destroy(this.gameObject);
    }
}
