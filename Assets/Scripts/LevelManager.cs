using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject trackPrefab, platformPrefab, endPlatformPrefab, ball1Prefab, ball2Prefab;
    public Material[] platformTx, trackTx;

    public void BuildLevel(int x, int y, int ball, int length, int twistyness)
    {
        bool pathY = true;
        GameObject go;
        go = Instantiate(platformPrefab, new Vector3(x, 0, y), Quaternion.identity);
        go.GetComponent<MeshRenderer>().material = platformTx[(Random.Range(0, platformTx.Length))];
        if (ball == 1) go = Instantiate(ball1Prefab, new Vector3(x, 1, y), Quaternion.identity);
        if (ball == 2) go = Instantiate(ball2Prefab, new Vector3(x, 1, y), Quaternion.identity);
        go = Instantiate(trackPrefab, new Vector3(x, 0, y+2), Quaternion.identity);
        go.GetComponent<MeshRenderer>().material = trackTx[(Random.Range(0, trackTx.Length))];
        y += 2;
        for(int i = 0; i < length-2; i++)
        {
            if (twistyness > Random.Range(1, 101)) pathY = !pathY;
            if (pathY) y++;
            if (!pathY) x++;
            go = Instantiate(trackPrefab, new Vector3(x, 0, y), Quaternion.identity);
            go.GetComponent<MeshRenderer>().material = trackTx[(Random.Range(0, trackTx.Length))];
        }
        y++;
        go = Instantiate(trackPrefab, new Vector3(x, 0, y), Quaternion.identity);
        go.GetComponent<MeshRenderer>().material = trackTx[(Random.Range(0, trackTx.Length))];
        go = Instantiate(endPlatformPrefab, new Vector3(x, 0, y+2), Quaternion.identity);
        go.GetComponent<MeshRenderer>().material = platformTx[(Random.Range(0, platformTx.Length))];
    }
}
