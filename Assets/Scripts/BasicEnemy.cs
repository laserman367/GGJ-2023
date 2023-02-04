using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField]
	private float movementSpeed;
	private float speedMultiplier = 1;
	[SerializeField]
	private float attackDelay;
	private float timeSinceAttack=0;
	[SerializeField]
	private float attackRange;
	[SerializeField]
	private float maxHealth;
	private float currHealth;
	[SerializeField]
	private EnemyType enemyType;
	[SerializeField]
	private Sprite[] sprites = null;
	[SerializeField]
	private SpriteRenderer spriteRenderer = null;
	[SerializeField]
	private Tree tree;
	private bool isLeftOfTree = false;
    void Start()
    {
		currHealth = maxHealth;
		if (sprites == null || sprites.Length < System.Enum.GetNames(typeof(EnemyType)).Length) Debug.LogError(this.gameObject.name + " doesn't have sprites set, set them in the Editor.");
		else if (spriteRenderer == null) Debug.LogError(this.gameObject.name + " doesn't have spritrenderer set, set it in the Editor.");
		else spriteRenderer.sprite = sprites[((int)enemyType)];
	}

    // Update is called once per frame
    void Update()
    {

		if (Mathf.Abs(tree.transform.position.x - this.transform.position.x) < attackRange)
		{
			timeSinceAttack += Time.deltaTime;
			if (timeSinceAttack > attackDelay) 
			{
				timeSinceAttack = 0;
				Attack();
			}
		}
		else this.transform.Translate((isLeftOfTree ? 1 : -1) * movementSpeed * Time.deltaTime * speedMultiplier, 0, 0);
    }
	public void Damage(float damage)
	{
		currHealth -= damage;
		if (currHealth <= 0) 
		{
			EnemyManager.Instance.RemoveEnemy(this);
		}
	}
	private void Attack()
	{

	}
	public void SetTree(Tree tree)
	{ 
		this.tree = tree;
		isLeftOfTree = this.transform.position.x < tree.transform.position.x;
		if (isLeftOfTree)
		{
			spriteRenderer.flipX = true;
		}
	}
}
