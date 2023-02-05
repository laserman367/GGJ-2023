using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	// Start is called before the first frame update
	private float knockback;
	private float damage;
	private float projectileSpeed;
	private bool hasActivated = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		this.transform.Translate(this.transform.right * projectileSpeed * Time.deltaTime);
    }
	private void OnTriggerEnter(Collider other)
	{
		if (hasActivated) return;
		if (other.gameObject.layer == 7)
		{
			hasActivated = true;
			GameObject.Destroy(this.gameObject);
		}
		BasicEnemy enemyScript = other.GetComponent<BasicEnemy>();
		if (enemyScript != null)
		{
			enemyScript.Damage(damage);
			enemyScript.transform.Translate(knockback * ((enemyScript.IsLeftOfTree)?-1:1), 0, 0);
			hasActivated = true;
			GameObject.Destroy(this.gameObject);
			
		}
		
	}
	public void SetValues(float kb, float dmg, float projSpd, Sprite projectileSprite)
	{
		knockback = kb;
		damage = dmg;
		projectileSpeed = projSpd;
		this.gameObject.GetComponentInChildren<SpriteRenderer>().sprite= projectileSprite;
	}
}
