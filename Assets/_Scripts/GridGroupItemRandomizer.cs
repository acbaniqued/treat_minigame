using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGroupItemRandomizer : MonoBehaviour
{
    [SerializeField]
    private Transform gridGroupContainer;
    private List<Transform> groupChildren = new List<Transform>();
    private List<Transform> GetGroupItems(Transform gridGroup)
    {
        List<Transform> itemList = new List<Transform>();
        int groupChildCount = gridGroupContainer.childCount;

        for (int i = 0; i < groupChildCount; i++)
        {
            itemList.Add(gridGroupContainer.GetChild(i));
        }

        return itemList;
    }

    public void RandomizeItemPosition(Transform groupContainer)
    {
        for (int i = 0; i < groupContainer.childCount; i++) 
        {
            groupContainer.GetChild(i).SetSiblingIndex(Random.Range(0, groupContainer.childCount));
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            RandomizeItemPosition(gridGroupContainer);
        }
    }
}
