using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode : MonoBehaviour
{
    // Start is called before the first frame update
    private BasicDefense attachedDefense = null;
    [SerializeField] 
    private GameObject canvas = null;
    void Start()
    {
        if (canvas == null) Debug.LogError(this.gameObject.name + " doesn't have canvas set in Editor. Drag it into the script.");
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
