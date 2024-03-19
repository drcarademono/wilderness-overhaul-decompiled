using System.Collections.Generic;
using System.Linq;
using DaggerfallWorkshop;
using UnityEngine;

public class WODistanceChecker : MonoBehaviour
{
	private Transform playerTransform;

	private float counter;

	private bool firefliesActive;

	public float distance;

	public List<WORandomMover> allChildren;

	private void Awake()
	{
		counter = Random.Range(0f, 2f);
		playerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}

	public void CreateFirefly(GameObject firefly, DaggerfallTerrain dfTerrain, int x, int y, float scale, Terrain terrain, float distance)
	{
		float num = Random.Range(0f - distance, distance);
		float num2 = Random.Range(0f - distance, distance);
		Vector3 vector = base.transform.position + new Vector3(num * scale, 0f, num2 * scale);
		vector.y = terrain.SampleHeight(vector) + Random.Range(0.5f * scale, 1.5f * scale);
		firefly.transform.parent = base.transform;
		firefly.AddComponent<WORandomMover>();
		firefly.GetComponent<WORandomMover>().startPos = base.transform.InverseTransformPoint(vector);
		firefly.transform.position = vector;
	}

	public void AddChildrenToArray()
	{
		allChildren = GetComponentsInChildren<WORandomMover>().ToList();
	}

	public void DeactivateAllChildren()
	{
		foreach (WORandomMover allChild in allChildren)
		{
			allChild.gameObject.SetActive(value: false);
		}
	}

	public void ActivateAllChildren()
	{
		foreach (WORandomMover allChild in allChildren)
		{
			allChild.gameObject.SetActive(value: true);
		}
	}

	private void FixedUpdate()
	{
		if (counter <= 0f)
		{
			if (Vector3.Distance(playerTransform.position, base.transform.position) <= distance)
			{
				if (!firefliesActive)
				{
					ActivateAllChildren();
					foreach (WORandomMover allChild in allChildren)
					{
						allChild.ToggleActivation(state: true);
						firefliesActive = true;
					}
				}
			}
			else if (firefliesActive)
			{
				DeactivateAllChildren();
				foreach (WORandomMover allChild2 in allChildren)
				{
					allChild2.ToggleActivation(state: false);
					firefliesActive = false;
				}
			}
			counter = 2f;
		}
		else
		{
			counter -= Time.fixedDeltaTime;
		}
	}
}
