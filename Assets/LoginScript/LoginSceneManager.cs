using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
public class LoginSceneManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject signScene;
    public GameObject loginScene;
    public GameObject Main;



    public void SignupButton()
    {
        loginScene.SetActive(false);
    }

    public void SignupButton_()
    {
        loginScene.SetActive(true);
    }


}
}
