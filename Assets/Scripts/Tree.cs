using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    private float maxHealth = 100;
    private float currHealth;
    private float regenSpeed = 5;
    private float regenCost = 3;
    // Start is called before the first frame update
    void Start()
    {
		currHealth = maxHealth;
	}

    // Update is called once per frame
    void Update()
    {
        if (currHealth < maxHealth && regenSpeed>0) 
        {
            float toHeal = Mathf.Min(maxHealth - currHealth, regenSpeed*Time.deltaTime);

            float carbonDrained = GameManager.Instance.TryDrain(ResourceType.CARBON, toHeal*regenCost);
            currHealth = Mathf.Clamp((carbonDrained / regenCost) + currHealth, -1, maxHealth);

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
