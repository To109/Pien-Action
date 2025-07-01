using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TilteManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSpaceClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            SceneManager.LoadScene("PlayScene");
        }
    }
}
