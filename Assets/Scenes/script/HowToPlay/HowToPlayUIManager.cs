using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HowToPlayUIManager : MonoBehaviour
{    public void OnBackButton()
    {
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }
}
