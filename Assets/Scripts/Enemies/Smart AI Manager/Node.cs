using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Node : MonoBehaviour
{

    bool alive = true;

    public int cost = 1;
    public int bestCost = int.MaxValue;

    public Vector2Int gridIndex;

	public Vector3 bestNextNodePos = Vector3.zero;
    
	public Vector3 position;

    public List<Node> neighbourNodes = new List<Node>();
    public List<Node> cornerNodes = new List<Node>();

	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		position = this.transform.position;
	}
	
    public void SetDestination()
    {
        cost = 0;
        bestCost = 0;
    }

    public void ResetNode()
    {
        cost = 1;
        bestCost = int.MaxValue;
    }

    public void SetAlive(bool set)
    {
        alive = set;
    }

    public bool GetAlive()
    {
        return alive;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if(bestNextNodePos != Vector3.zero)
        {
            Debug.DrawRay(transform.position, bestNextNodePos - transform.position, Color.green);
        }
#endif
        //GetComponentInChildren<TextMeshPro>().text = bestCost.ToString();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position, new Vector3(1, 1, 1));
    }
}
