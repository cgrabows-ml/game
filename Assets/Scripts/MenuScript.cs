using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void Stage1()
    {
        SceneManager.LoadScene("battle_scene");
    }
}