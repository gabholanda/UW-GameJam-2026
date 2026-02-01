using UnityEngine;
using UnityEngine.SceneManagement;

public class beginOnClick : MonoBehaviour
{

    void OnMouseDown() {
	Debug.Log("Sprite Clicked");
	SceneManager.LoadScene("note");
	}
}
