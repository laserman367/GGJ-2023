using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode : MonoBehaviour
{
    // Start is called before the first frame update
    private BasicDefense attachedDefense = null;
    [SerializeField] 
    private GameObject upgradeUI = null;
    [SerializeField]
	Sprite[] sprites = null;
	[SerializeField]
	private bool isAvailable = false;
	[SerializeField]
	SpriteRenderer spriteRenderer = null;
	private bool isUpgrading = false;
	void Start()
    {
        if (upgradeUI == null) Debug.LogError(this.gameObject.name + " doesn't have canvas set in Editor. Drag it into the script.");
		if (spriteRenderer == null) Debug.LogError(this.gameObject.name + " doesn't have sprite renderer set in Editor. Drag it into the script.");
		else
		{
			if (isAvailable) spriteRenderer.sprite = sprites[1];
			else spriteRenderer.sprite = sprites[0];
		}
		if(sprites==null || sprites.Length<3) Debug.LogError(this.gameObject.name + " doesn't have all sprites set in Editor. Drag them into the script.");
	}
	private void OnMouseEnter()
	{
		if (!isUpgrading) spriteRenderer.sprite = sprites[isAvailable ? 2 : 0];
	}
	private void OnMouseExit()
	{
		if(!isUpgrading) spriteRenderer.sprite = sprites[isAvailable ? 1 : 0];
	}

	// Update is called once per frame
	void Update()
    {
        
    }
	public void TryAttachDefense(int type)
	{
		GameObject defenseObj = GameManager.Instance.RequestDefense(type);
		if (defenseObj != null)
		{
			GameObject.Instantiate(defenseObj);
			defenseObj.transform.position = transform.position;
			attachedDefense = defenseObj.GetComponent<BasicDefense>();
			upgradeUI.SetActive(false);
			isUpgrading = false;
		}
	}
    public bool AttachDefense(BasicDefense defense)
    {
        if (attachedDefense != null) return false;
        attachedDefense = defense;
        return true;
    }
    public BasicDefense DetachDefense()
    {
        BasicDefense oldDefense = attachedDefense;
        attachedDefense = null;
		return oldDefense;
	}
	public void HandleClick()
	{
		upgradeUI.SetActive(!isUpgrading);
		isUpgrading = !isUpgrading;
	}
}
