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
    private TypeFloatDict buildCost;
	[SerializeField]
	private TypeFloatDict projectileCost;

    private float timeSinceAttack = 0;
    private float timeSinceEnemyCheck = 0;
    private BasicEnemy targetedEnemy = null;
    private float checkDelay = 0.2f;
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetedEnemy == null) 
        {
            checkDelay += Time.deltaTime;

            if (checkDelay > timeSinceEnemyCheck)
            {
                CheckForEnemy();
            }
        }
    }

    private void CheckForEnemy()
    {

    }
}
