using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D inventory;
    [SerializeField] private GameObject sparklePrefab;
    [SerializeField] private GameObject invntryClbrtnPrefab;
    [SerializeField] private GameObject celebrationPrefab;
    [SerializeField] private GameObject fairy;
    [SerializeField] private Animator animator;
    [SerializeField] private SFXManager sfx;
    [SerializeField] private Transform UI;
    [SerializeField] Buttons sceneLoader;

    [SerializeField] float cheerDuration = .5f;
    [SerializeField] float sadDuration = 1f;
    [SerializeField] float jumpPower = 1f;
    [SerializeField] int jumpNum = 2;
    [SerializeField] float returnTime = .3f;

    private GameObject sparkle;
    private Vector2 originalPos;
    private Vector3 fairyOriginalPos;
    private Vector2 difference;
    private Collider2D hit;
    private BoxCollider2D[] contacts = new BoxCollider2D[15];

    private bool hasContact = false;
    private bool isObject = false;
    private byte magObjCount = 0;

    private void Start()
    {
        fairyOriginalPos = fairy.transform.localPosition;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(pos, Vector3.zero).collider;
           
            if (hit && hit.transform.parent.CompareTag("Object"))
            {
                isObject = true;
                originalPos = hit.transform.position;
                difference = (Vector2)pos - (Vector2)hit.transform.position;
                hit.GetComponent<SpriteRenderer>().sortingLayerName = "Objects";
            }
        }

        if (Input.GetMouseButton(0) && isObject)
        {
            hit.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
        }

        if (Input.GetMouseButtonUp(0) && isObject)
        {
            hit.OverlapCollider(new ContactFilter2D().NoFilter(), contacts);

            for (int i = 0; i < contacts.Length; i++)
            {
                if (contacts[i] == inventory)
                    hasContact = true;

                contacts[i] = null;
            }


            if (hasContact)
                if (hit.GetComponent<GardenObject>().isMagical)
                    StartCoroutine(RevealObject(hit));
                else
                {
                    StartCoroutine(WrongObject());
                    StartCoroutine(ReturnObject(hit));
                }
            else
                StartCoroutine(ReturnObject(hit));

            hit = null;
            isObject = false;
            hasContact = false;
        }
    }

    private IEnumerator RevealObject(Collider2D obj)
    {
        fairy.transform.DOKill();
        sfx.Play(SFXManager.Clip.Success);

        DOTween.Sequence()
            .Append(obj.transform.GetComponent<SpriteRenderer>().DOFade(0, cheerDuration))
            .Join(fairy.transform.DOLocalJump(fairyOriginalPos, jumpPower, jumpNum, cheerDuration * 2, true))
            .Join(obj.GetComponent<GardenObject>().shadow.DOColor(Color.white, cheerDuration));

        sparkle = Instantiate(sparklePrefab, obj.GetComponent<GardenObject>().shadow.transform.position, Quaternion.identity, UI.transform);
        sparkle.transform.localScale = Vector3.one * 10;
        StartCoroutine(DeleteSparkle());
        animator.SetBool("Happy", true);
        yield return new WaitForSeconds(cheerDuration);
        animator.SetBool("Happy", false);
        DOTween.Sequence().Kill();
        Destroy(obj.GetComponent<GardenObject>().sparkle);
        Destroy(obj.gameObject);
        magObjCount++;

        if (magObjCount == 8)
            StartCoroutine(Win());
    }
    private IEnumerator WrongObject()
    {
        sfx.Play(SFXManager.Clip.Failure);
        animator.SetBool("Sad", true);
        yield return new WaitForSeconds(sadDuration / 8);
        fairy.transform.DORotate(new Vector3(0f, 180f, 0f), sadDuration / 4);
        yield return new WaitForSeconds(sadDuration * 3 / 8);
        fairy.transform.DORotate(new Vector3(0f, 0f, 0f), sadDuration / 4);
        yield return new WaitForSeconds(sadDuration * 0.275f);
        fairy.transform.DOKill();
        animator.SetBool("Sad", false);
    }
    private IEnumerator ReturnObject(Collider2D obj)
    {
        obj.transform.DOMove(originalPos, returnTime).SetEase(Ease.InOutCirc);
        yield return new WaitForSeconds(.001f);
        obj.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
    }
    private IEnumerator DeleteSparkle()
    {
        yield return new WaitForSeconds(15f);
        Destroy(sparkle);
    }
    private IEnumerator Win()
    {
        yield return new WaitForSeconds(cheerDuration);       
        
        sfx.Play(SFXManager.Clip.Win);

        Instantiate(celebrationPrefab, Vector3.zero, Quaternion.identity);
        Instantiate(invntryClbrtnPrefab, UI.transform.position, Quaternion.identity,UI);

        fairy.transform.DOLocalJump(fairyOriginalPos, jumpPower, jumpNum + 1, 2.5f, true);
        fairy.transform.DOLocalRotate(new Vector3(0f, 180f, 0f), 1.25f);
        animator.SetBool("Happy", true);
        yield return new WaitForSeconds(1.25f);
        fairy.transform.DOLocalRotate(Vector3.zero, 1.25f);
        yield return new WaitForSeconds(1.25f);
        animator.SetBool("Happy", false);

        yield return new WaitForSeconds(2.5f);

        StartCoroutine(sceneLoader.SceneLoader(2));
    }
}
