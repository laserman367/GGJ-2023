using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{

	public SimpleTypeFloatDict simpleResourceTypePercentages;
	public TypeFloatDict resourceTypePercentages;

	[SerializeField]
	private int maxResourcesToDrain;
	[SerializeField]
	private float maxResourcesPerSecond;
	private float currDrain = 0;
	private bool isFullyDraining;
	List<RootJoint> joints = new List<RootJoint>();

	private 
	// Start is called before the first frame update
	void Start()
	{
		//Normalizing the dictionary to a total of 1 weight.
		float totalFloat = 0;
		foreach(KeyValuePair<ResourceType, float> resourcePair in resourceTypePercentages)
		{
			totalFloat += resourcePair.Value;
		}
		if(totalFloat>0)
		{
			TypeFloatDict balancedDict = new TypeFloatDict();
			foreach (KeyValuePair<ResourceType, float> resourcePair in resourceTypePercentages)
			{
				balancedDict.Add(resourcePair.Key, resourceTypePercentages[resourcePair.Key] / totalFloat);
			}
			resourceTypePercentages = balancedDict;
		}
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public Dictionary<ResourceType, float> AttachRoot(RootJoint joint)
	{
		Dictionary<ResourceType, float> drain = new Dictionary<ResourceType, float>();
		if (!isFullyDraining)
		{
			float drainSpeed = Mathf.Min(joint.MaxDrainSpeed, maxResourcesPerSecond - currDrain);
			currDrain += drainSpeed;
			
			foreach (KeyValuePair<ResourceType, float> resourcePair in resourceTypePercentages)
			{
				drain[resourcePair.Key] = resourceTypePercentages[resourcePair.Key] * drainSpeed;
			}
		}
		joints.Add(joint);
		return drain;
	}
	public void DetachRoot(RootJoint joint)
	{
		joints.Remove(joint);
	}

}
