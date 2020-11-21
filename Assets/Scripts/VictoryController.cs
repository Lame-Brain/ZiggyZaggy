using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryController : MonoBehaviour
{
    public GameObject ps;

    private void OnTriggerEnter(Collider other)
    {
        float x = this.transform.position.x, y = 1.5f, z = this.transform.position.z;
        ps.SetActive(true);
        GameManager.GAME.GetComponent<AudioSource>().PlayOneShot(GameManager.GAME.victorySound);
        other.transform.position = new Vector3(x, y, z);
        GameManager.GAME.Paused = true;
        Invoke("LevelDone", 1f);
    }

    private void LevelDone()
    {
       if(GameManager.GAME.GameState != GameManager.State.LEVELCOMPLETE) GameManager.GAME.SwitchState(GameManager.State.LEVELCOMPLETE);
    }
}
