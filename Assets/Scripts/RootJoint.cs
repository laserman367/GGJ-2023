using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.EventSystems;

public class RootJoint : MonoBehaviour
{
	// Start is called before the first frame update
	private int upgradeLevel = 0;
	public float MaxDrainSpeed { get;  private set; }
	private Vector3 startPoint = new Vector3(0,0,0);
	private Vector3 endPoint = new Vector3(1, 0, 0);
	private RootJoint parent;
	private List<Joint> children = new List<Joint>();	
	private bool isCursorLocked;
	[SerializeField]
	private float moveSpeed;
	private LineRenderer lineRenderer;
	private Transform clickBoxTransform;
	private float maxLength = 3;
	private ResourceNode resourceNode;
	private int layerMask;
	private Dictionary<ResourceType, float> drain;
	void Start()
	{
		MaxDrainSpeed = 3;
		layerMask = LayerMask.GetMask("Resources");
		clickBoxTransform = this.transform.GetChild(0);
		lineRenderer = this.GetComponent<LineRenderer>();
		startPoint = this.transform.position;
		endPoint += this.transform.position;
	}

	void FixedUpdate()
	{
		if(isCursorLocked)
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = endPoint.z;
			endPoint = Vector3.MoveTowards(endPoint, mousePos, moveSpeed*Time.deltaTime);
			if(Vector3.Distance(endPoint, startPoint) > maxLength)
			{
				Vector3 direction = (endPoint - startPoint).normalized;
				endPoint = startPoint + (direction * maxLength);
			}
			lineRenderer.SetPosition(1, endPoint-this.transform.position);
			clickBoxTransform.position = endPoint;
		}
		
	}

	public void ToggleDrag()
	{
		if(isCursorLocked)
		{
			Collider[] colliders = Physics.OverlapBox(endPoint, new Vector3(0.2f,0.2f,0.2f), Quaternion.identity, layerMask);
			if(colliders.Length > 0)
			{
				foreach(Collider collider in colliders)
				{

					resourceNode = collider.GetComponent<ResourceNode>();
					if (resourceNode != null)
					{
						drain = resourceNode.AttachRoot(this);
						GameManager.Instance.AddDrain(drain);
						break;
					}
				}
				
			}	
		}
		else
		{
			if(drain != null)
			{
				resourceNode.DetachRoot(this);
				GameManager.Instance.RemoveDrain(drain);
				drain = null;
			}
		}
		isCursorLocked = !isCursorLocked;
	}

	public void SetParent(RootJoint parent)
	{ 
		this.parent = parent; 
	}
}
