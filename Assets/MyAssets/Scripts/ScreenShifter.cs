using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScreenShifter : MonoBehaviour
{
    [SerializeField] private Vector2 border;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject PathParent;

    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private float arrowTimeOffset = .3f;


    Vector3[] pathArray = new Vector3[4];

    private float exitMove;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            pathArray[i] = PathParent.transform.GetChild(i).localPosition + new Vector3(0, Mathf.Sign(arrow.transform.position.y) * 202.5f, 0);
        }
        pathArray[3] = pathArray[0];

        StartCoroutine(Delay());
    }

    private void OnMouseEnter()
    {
        mainCamera.transform.DOMoveY(border.y, 3f).SetEase(Ease.OutSine);
    }

    private void OnMouseExit()
    {
        exitMove = (Mathf.Sign(border.y) > 0) 
                   ? Mathf.Min(border.y, mainCamera.transform.position.y + .2f)                   
                   : Mathf.Max(border.y, mainCamera.transform.position.y - .2f);
        
        mainCamera.transform.DOKill();
        mainCamera.transform.DOMoveY(exitMove, .5f).SetEase(Ease.OutSine);
    }
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(arrowTimeOffset);
        arrow.transform.DOLocalPath(pathArray, moveDuration).SetLoops(-1);
    }
}
