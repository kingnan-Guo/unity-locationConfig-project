using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvasSize : MonoBehaviour
{

    // Start is called before the first frame update
    private RectTransform rectTransform;
    private Canvas canvas;
    private Image image;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = FindObjectOfType<Canvas>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        SetImageHeightTo100Percent();
    }

    private void SetImageHeightTo100Percent()
    {
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.sizeDelta = new Vector2(0, 0);
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);

        image.rectTransform.anchorMin = new Vector2(0, 0);
        image.rectTransform.anchorMax = new Vector2(0, 1);
        image.rectTransform.sizeDelta = new Vector2(0, 0);
        image.rectTransform.offsetMin = new Vector2(0, 0);
        image.rectTransform.offsetMax = new Vector2(0, 0);
    }
}

// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.UI;
// using System;
// using UnityEngine.EventSystems;
// public class durg : MonoBehaviour
// {
//     private int mode = 0;
//     GameObject gameObj;
//     public GameObject btnTurn;
//     Vector3 pos;
//     Ray ray;
//     RaycastHit hitInfo;
//     LayerMask mask = 1 << 8;
//     public EventSystem es;
//     public GraphicRaycaster gr;
//     void Awake()
//     {
//         btnTurn.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(BtnTurnRight);
//         btnTurn.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(BtnTurnLeft);
//         btnTurn.gameObject.SetActive(false);
//     }
//     private void BtnTurnRight()
//     {
//         //gameObj.transform.Rotate(Vector3.up * 300 * Time.deltaTime);
//         gameObj.transform.eulerAngles += new Vector3(0, 15, 0);
//     }
//     private void BtnTurnLeft()
//     {
//         //gameObj.transform.Rotate(Vector3.up * 300 * Time.deltaTime);
//         gameObj.transform.eulerAngles += new Vector3(0, -15, 0);
//     }
//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//             if (Physics.Raycast(ray, out hitInfo))
//             {
//                 if (hitInfo.transform.gameObject.tag == "Furniture")
//                 {
//                     gameObj = hitInfo.transform.gameObject;
//                     mode = 1;
//                 }
//             }
//         }
//         if (Input.GetMouseButton(0))
//         {
//             if (check())
//                 return;
//             else
//             {
//                 ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//                 if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask.value))
//                 {
//                     if (mode == 1)
//                     {
//                         gameObj.transform.position = hitInfo.point;
//                         btnTurn.gameObject.SetActive(false);
//                     }
//                 }
//             }
//         }
//         if (Input.GetMouseButtonUp(0))
//         {
//             mode = 0;
//             if (check())
//                 return;
//             else
//             {
//                 ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//                 if (Physics.Raycast(ray, out hitInfo))
//                 {
//                     if (hitInfo.transform.gameObject.tag == "Furniture")
//                     {
//                         btnTurn.gameObject.SetActive(true);
//                         Vector3 screenPos = Camera.main.WorldToScreenPoint(gameObj.transform.position);
//                         btnTurn.GetComponent<RectTransform>().position = screenPos;
//                     }
//                     else
//                     {
//                         btnTurn.gameObject.SetActive(false);
//                     }
//                 }
//             }
//         }
//     }
//     bool check()
//     {
//         PointerEventData eventData = new PointerEventData(es);
//         eventData.pressPosition = Input.mousePosition;
//         eventData.position = Input.mousePosition;
//         List<RaycastResult> list = new List<RaycastResult>();
//         gr.Raycast(eventData, list);
//         return list.Count > 0;
//     }
// }
