using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Debug_Timestamp : MonoBehaviour
{
    private float currentTime;

    private bool canStart;

    public TextMeshProUGUI endText;

    [SerializeField]
    private GameEvent onCreateHitIndicator;

    private void Update()
    {
        if (canStart)
        {
            currentTime += Time.deltaTime * 1;
        }
    }

    public void Timestamp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            canStart = true;
            print (currentTime);
            currentTime = 0;
            onCreateHitIndicator.Raise(this, null);
        }
    }

    public void Debug_EndGame(Component sender, object data)
    {
        Time.timeScale = 0;
        if (sender is System_EnemyStamina)
        {
            endText.text = ("PLAYER WON");
        }
        else if (sender is System_PlayerHealth)
        {
            endText.text = ("PLAYER LOST");
        }
        else
        {
            endText.text = ("PLAYER WON");
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
