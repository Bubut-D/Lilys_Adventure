using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GardenObject : MonoBehaviour
{
    public Image shadow;
    [SerializeField] private GameObject sparklePrefab;

    [HideInInspector] public GameObject sparkle;

    
    private float duration;
    public bool isMagical = true;

    private void Start()
    {
        if (isMagical)
            duration = Random.Range(5,20);
    }
    private void Update()
    {
        if (!isMagical)
            return;
        if (!sparkle && Time.timeSinceLevelLoad >= duration)
            sparkle = Instantiate(sparklePrefab, transform.position, Quaternion.identity);
        if (sparkle)
        {
            sparkle.transform.position = transform.position;
            if (Time.timeSinceLevelLoad >= duration + 15f)
            {
                Destroy(sparkle);
            }
        }

    }
}
