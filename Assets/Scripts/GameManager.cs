using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance;
	public static GameManager Instance 
	{ 
		get 
		{
			if (_instance == null) Debug.LogError("No Gamemanager");

			return _instance; 
		} 
	}
	private int waveCounter=0;
	[SerializeField]
	private TypeFloatDict resources;
	private float oxygenGain = 5;
	private float carbonGain = 2.5f;
	private bool isMovingRoot = false;
	private RootJoint attachedJoint;
	private int layerMask;
	Dictionary<ResourceType, float> totalDrain = new Dictionary<ResourceType, float>();
	// Start is called before the first frame update
	void Start()
	{
		_instance = this;
		foreach(KeyValuePair<ResourceType, float> kvp in resources) 
		{
			totalDrain.Add(kvp.Key, 0);
		}
		layerMask = LayerMask.GetMask("UI");
	}

	// Update is called once per frame
	void Update()
	{
		foreach(KeyValuePair<ResourceType, float> kvp in totalDrain)
		{
			if (kvp.Value > 0) resources[kvp.Key] += totalDrain[kvp.Key] *Time.deltaTime;
		}
		resources[ResourceType.OXYGEN] += oxygenGain * Time.deltaTime;
		resources[ResourceType.CARBON] += carbonGain * Time.deltaTime;
		if (Input.GetMouseButtonDown(0))
		{
			HandleClick();
		}
	}
	public bool TryDrainGroup(Dictionary<ResourceType, float> group)
	{
		if (group == null) return false;
		foreach (KeyValuePair<ResourceType,float> kvp in group)
		{
			if (resources[kvp.Key] < kvp.Value) return false;
		}
		foreach (KeyValuePair<ResourceType, float> kvp in group)
		{
			resources[kvp.Key] -= kvp.Value;
		}
		return true;
	}
	public bool TryDrain(ResourceType type, float value)
	{
		if(resources[type]>value)
		{
			resources[type] -= value;
			return true;
		}
		return false;
	}
	public float DrainUpTo(ResourceType type, float value) 
	{
		float drained = Mathf.Min(value, resources[type]);
		if (drained > 0)  resources[type] -= drained;
		return drained;
	}
	public void AddRootResource(Dictionary<ResourceType, float> drain)
	{
		foreach (KeyValuePair<ResourceType, float> kvp in drain)
		{
			if (kvp.Value > 0) totalDrain[kvp.Key] += kvp.Value;
		}
	}
	public void RemoveRootResource(Dictionary<ResourceType, float> drain)
	{
		foreach (KeyValuePair<ResourceType, float> kvp in drain)
		{
			if (kvp.Value > 0) totalDrain[kvp.Key] -= kvp.Value;
		}
	}

	private void HandleClick()
	{
		if (!isMovingRoot)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if(Physics.Raycast(ray, out hitInfo,100, layerMask))
			{
				RootJoint joint = null;
				if (hitInfo.transform.parent != null)
				{
					joint = hitInfo.transform.parent.gameObject.GetComponent<RootJoint>();
				}


				if (joint != null)
				{
					attachedJoint = joint;
					isMovingRoot = true;
					attachedJoint.ToggleDrag();
				}
			}
		}
		else
		{
			attachedJoint.ToggleDrag();
			attachedJoint = null;
			isMovingRoot = false;
		}
	}
}
