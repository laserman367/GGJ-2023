using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDefense : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float attackDelay;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float range;
    [SerializeField]
    private float knockBack;
	[SerializeField]
	private float projectileSpeed;
	[SerializeField]
    private TypeFloatDict buildCost;
	public TypeFloatDict BuildCost { get { return buildCost; } }
	[SerializeField]
	private TypeFloatDict projectileCost;

    private float timeSinceAttack = 0;
    private float timeSinceEnemyCheck = 0;
    private BasicEnemy targetedEnemy = null;
    private float checkDelay = 0.2f;
	[SerializeField]
	private GameObject projectilePrefab = null;
	[SerializeField]
	private Sprite projectileSprite = null;
	void Start()
    {
		if (projectilePrefab == null) Debug.LogError(this.gameObject.name + " doesn't have projectile prefab.");
		if (projectileSprite == null) Debug.LogError(this.gameObject.name + " doesn't have projectile sprite.");
	}

    // Update is called once per frame
    void Update()
    {
        if (targetedEnemy == null) 
        {
			timeSinceEnemyCheck += Time.deltaTime;

            if (checkDelay < timeSinceEnemyCheck)
            {
				timeSinceEnemyCheck = 0;

				CheckForEnemy();
            }
        }
		else
		{
			timeSinceAttack += Time.deltaTime;
			if(timeSinceAttack> attackDelay)
			{
				timeSinceAttack = 0;
				if(GameManager.Instance.TryDrainGroup(projectileCost))
				{
					GameObject projObj = GameObject.Instantiate(projectilePrefab);
					Projectile proj = projObj.GetComponent<Projectile>();
					proj.transform.position = this.transform.position;
					Vector2 dir = new Vector2(targetedEnemy.transform.position.x - this.transform.position.x, targetedEnemy.transform.position.y - this.transform.position.y);
					float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
					proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
					proj.SetValues(knockBack, damage, projectileSpeed,projectileSprite);
				}
			}
		}
    }

    private void CheckForEnemy()
    {
		targetedEnemy = EnemyManager.Instance.CheckForEnemy(this.transform.position, range);
    }
}
