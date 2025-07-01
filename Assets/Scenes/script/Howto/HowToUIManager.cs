using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HowToUIManager : MonoBehaviour
{    public void OnBackButton()
    {
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }
}
