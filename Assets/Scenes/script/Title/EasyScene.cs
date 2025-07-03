using UnityEngine;
using UnityEngine.SceneManagement;

public class EasyScene : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnClickToPlay()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
