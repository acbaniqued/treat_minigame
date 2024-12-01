using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

enum rotateDirection { left, right, up, down }

public class ImageFlipEffect : MonoBehaviour
{
    private Transform imageObj;
    [SerializeField]
    private GameObject imageBackFace;
    [SerializeField]
    private float flipSpeed = 0.5f;
    [SerializeField]
    private rotateDirection rotateDirection;
    [SerializeField]
    private float targetDegrees = 180f;
    private bool isFlipped;
    [SerializeField]
    private bool isRotating = false;

    public void Awake()
    {
        imageObj = GetComponent<Transform>();
        if(imageBackFace.activeSelf) isFlipped = true;
    }

    private void ShowBackface()
    {
        imageBackFace.SetActive(true);
    }

    private void ShowFrontface()
    {
        imageBackFace.SetActive(false);
    }

    private void ShowCorrectFace()
    {
        if (isFlipped)
        {
            isFlipped = false;
            ShowFrontface();
        }
        else
        {
            isFlipped = true;
            ShowBackface();
        }
    }

    public void FlipImage()
    {
        StartCoroutine(ICalculateImageFlip(flipSpeed, imageObj, targetDegrees, rotateDirection));
    }

    private Vector3 GetEndRotation(rotateDirection rotDir, Transform targetObj, float targetDegrees)
    {
        Vector3 endRot = targetObj.eulerAngles;

        switch (rotDir) 
        {
            case (rotateDirection.left):
                {
                    endRot = new Vector3(targetObj.eulerAngles.x, targetObj.eulerAngles.y + targetDegrees, targetObj.eulerAngles.z);
                    break;
                }
            case (rotateDirection.right):
                {
                    endRot = new Vector3(targetObj.eulerAngles.x, targetObj.eulerAngles.y + targetDegrees, targetObj.eulerAngles.z);
                    break;
                }
            case (rotateDirection.up):
                {
                    endRot = new Vector3(targetObj.eulerAngles.x + targetDegrees, targetObj.eulerAngles.y, targetObj.eulerAngles.z);
                    break;
                }
            case (rotateDirection.down):
                {
                    endRot = new Vector3(targetObj.eulerAngles.x + targetDegrees, targetObj.eulerAngles.y, targetObj.eulerAngles.z);
                    break;
                }
        }
        return endRot;
    }

    private IEnumerator ICalculateImageFlip(float speed, Transform target, float targetDegrees, rotateDirection rotDir)
    {
        isRotating = true;
        Vector3 startRotation = target.transform.rotation.eulerAngles;
        Vector3 endRotation = GetEndRotation(rotDir, target, targetDegrees);

        float timer = 0.0f;
        bool reachedHalfwayRotation = false;
        
        while (timer < speed)
        {
            timer += Time.deltaTime;
            target.transform.eulerAngles = Vector3.Lerp(startRotation, endRotation, timer / speed);

            bool isHalfwayRotation = timer >= speed / 2;

            if (isHalfwayRotation)
            {
                if (!reachedHalfwayRotation)
                {
                    reachedHalfwayRotation = true;
                    ShowCorrectFace();
                }
            }

            yield return null;
        }
        isRotating = false; 
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FlipImage();
        }
    }
}
