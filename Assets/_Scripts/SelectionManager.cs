using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance;
    [SerializeField]
    private bool isSelecting = true;
    [SerializeField]
    private int SelectionLimit = 2;
    [SerializeField]
    private List<GameObject> SelectedGameObjects = new List<GameObject>();
    public GameObject LastSelectedObject { get; set; }
    private bool hasUpdated = false;

    public void Start()
    {
        if (instance != null) { Destroy(this.gameObject); } else { instance = this; }
    }

    public void Update()
    {
        if (isSelecting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GraphicRaycastManager.instance.GetClickedObject() != null)
                {
                    GameObject objectSelected = GraphicRaycastManager.instance.GetClickedObject();
                    if (objectSelected.GetComponent<Selectable>())
                    {
                        if (objectSelected.GetComponent<Selectable>().IsActive())
                        {
                            LastSelectedObject = objectSelected;
                            Selectable sObj = objectSelected.GetComponent<Selectable>();
                            AddToSelectedObjects(sObj, SelectionLimit);
                            sObj.OnSelect();
                        }
                    }
                }
            }
        }
    }

    private void AddToSelectedObjects(Selectable selectableObject, int selectLimit)
    {
        StartCoroutine(IAddToSelectedObjects(selectableObject, selectLimit));
    }

    private IEnumerator IAddToSelectedObjects(Selectable selectableObject, int selectLimit)
    {
        if (SelectedGameObjects.Count >= selectLimit) yield break;
        SelectedGameObjects.Add(selectableObject.gameObject);
        hasUpdated = true;
        yield return null; //update that the list had a change in value at exactly after 1 end of frame (just a workaround for updating list value)
        hasUpdated = false;
        yield break;
    }

    public void SetSelectionLimit(int sLimit)
    {
        SelectionLimit = sLimit;
    }

    public void ClearSelection()
    {
        SelectedGameObjects.Clear();
    }

    public void SetSelectState(bool state)
    {
        isSelecting = state;
    }

    public List<GameObject> GetSelectedObjects()
    {
        return SelectedGameObjects;
    }

    public bool IsListUpdated()
    {
        return hasUpdated;
    }

    public void SetIsSelecting(bool state)
    {
        isSelecting = state;
    }

}
