using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLoader : MonoBehaviour
{
    public void TransToEnd1()
    {
        SceneManager.LoadScene("End1Scene");
    }

    public void TransToEnd2()
    {
        SceneManager.LoadScene("End2Scene");
    }
}
