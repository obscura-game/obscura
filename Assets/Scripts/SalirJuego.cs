using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    public Button salirBoton;

    void Start()
    {

        if (salirBoton != null)
        {
            salirBoton.onClick.AddListener(QuitGame);
        }
    }

    void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;  
        #else
            Application.Quit(); 
        #endif
    }
}
