using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GraphicRaycastManager : MonoBehaviour
{
    public static GraphicRaycastManager instance;
    [SerializeField]
    private GraphicRaycaster m_Raycaster;
    [SerializeField]
    private PointerEventData m_PointerEventData;
    [SerializeField]
    private EventSystem m_EventSystem;
    [SerializeField]
    public GameObject ClickedGameObject { get; set; }

    public void Start()
    {
        if (instance != null) { Destroy(this.gameObject); } else { instance = this; }
    }

    public GameObject GetClickedObject()
    {
        GameObject clickedObject = null;
        if (Input.GetMouseButtonDown(0))
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            m_Raycaster.Raycast(m_PointerEventData, results);

            if (results.Count > 0)
            {
                clickedObject = results[0].gameObject;
            }
        }
        return clickedObject;
    }
}
