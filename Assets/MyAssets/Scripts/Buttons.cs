using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [SerializeField] private Animator transition;
    public void Exit()
    {
        StartCoroutine(SceneLoader(-1));
    }
    public void Level1()
    {
        StartCoroutine(SceneLoader(1));
    }
    public void StartMenu()
    {
        StartCoroutine(SceneLoader(0));
    }
    public IEnumerator SceneLoader(int index)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
        switch (index)
        {
            case 0: SceneManager.LoadScene(0);
                break;
            case 1:SceneManager.LoadScene(1);
                break;
            case 2:SceneManager.LoadScene(2);
                break;
            default: Application.Quit();
                break;
        }
    }
}
