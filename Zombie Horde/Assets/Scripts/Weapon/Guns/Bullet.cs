using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 2.5f;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private LayerMask enemy;
    [SerializeField] private GunData gunData;

    private void Awake()
    {
        Destroy(gameObject, _lifeTime);//Makes sure missed bullets automatically get destroyed after _lifeTime seconds
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector3 pBulletVelocity)
    {
        rigidBody.velocity = pBulletVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            collision.GetComponentInParent<EnemyHealth>().TakePlayerDamage((int)gunData.weaponDamage);
        }
        Destroy(this.gameObject);
    }
}