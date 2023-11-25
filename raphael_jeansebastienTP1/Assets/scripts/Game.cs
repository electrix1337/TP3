using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] GameObject canvasObj;
    [SerializeField] GameObject timeCanvas;
    public static bool isGameOver = false;
    bool hasWaited = false;

    static Canvas canva;
    static GameTime gameTime;
    void Start()
    {
        canva = canvasObj.GetComponent<Canvas>();
        canva.enabled = false;
        gameTime = timeCanvas.GetComponent<GameTime>();
    }

    void Update()
    {
        if (isGameOver) // Ne rien faire tant que la partie n'est pas finis
        {
            if (hasWaited) // Lorsque la partie viens de finir, nous ajoutons un petit délai avant d'em repartir une autre
            {
                if (Input.anyKey) // Si le joueur clique sur n'importe qu'elle 'key' recommencer une autre partie
                {
                    isGameOver = false;
               
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    Time.timeScale = 1;
                }
            }
            else
            {
               StartCoroutine(Wait(1f));
            }
        }
    }

    IEnumerator Wait(float sec) // Petit délai
    {
        yield return new WaitForSecondsRealtime(sec);
        hasWaited = true;
    }
    public static void GameOver() // Lorsque la partie finis, afficher le canva de fin de partie
    {
        Time.timeScale = 0;
        //gameTime.EndGame();
        canva.enabled = true;
        isGameOver = true;
    }
}
