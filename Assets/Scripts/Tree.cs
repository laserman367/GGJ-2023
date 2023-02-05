using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tree : MonoBehaviour
{

    private float maxHealth = 100;
    private float currHealth;
	[SerializeField] 
    private float regenSpeed = 5;
    private float regenCost = 3;
	[SerializeField]
	private Scrollbar healthbar = null;
    // Start is called before the first frame update
    void Start()
    {
		if (healthbar == null) Debug.LogError("Tree does not have Healthbar set.");
		currHealth = maxHealth;
	}

    // Update is called once per frame
    void Update()
    {
		healthbar.size = currHealth/maxHealth;

		if (currHealth < maxHealth && regenSpeed>0) 
        {
            float toHeal = Mathf.Min(maxHealth - currHealth, regenSpeed*Time.deltaTime);

            if(GameManager.Instance.TryDrain(ResourceType.CARBON, toHeal*regenCost))
            {
				currHealth = Mathf.Clamp(toHeal + currHealth, -1, maxHealth);
			}
		}
    }
    public void Damage(float damage)
    {
        currHealth -= damage;
        if(currHealth < 0)
        {
            Console.WriteLine("You have become the dieded! Health restored.");
            currHealth = maxHealth;

		}
    }
}
