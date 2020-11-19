using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject trackPrefab, platformPrefab, ball1Prefab, ball2Prefab;
    public Material[] platformTx, trackTx;

    private GameObject[] _track;
    private int _length = 25, _twistyness = 10;
    private bool _pathY = true;
    // Start is called before the first frame update
    void Start()
    {
        int x = 0, y = 3;
        _track = new GameObject[_length];
        _track[0] = Instantiate(platformPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        _track[0].GetComponent<MeshRenderer>().material = platformTx[Random.Range(0,platformTx.Length)];
        _track[1] = Instantiate(trackPrefab, new Vector3(0, 0, 2), Quaternion.identity);
        _track[1].GetComponent<MeshRenderer>().material = trackTx[Random.Range(0, trackTx.Length)];
        for (int i = 2; i < _length-3; i++)
        {
            _track[i] = Instantiate(trackPrefab, new Vector3(x, 0, y), Quaternion.identity);
            _track[i].GetComponent<MeshRenderer>().material = trackTx[Random.Range(0, trackTx.Length)];
            if (Random.Range(0, 100) < _twistyness) _pathY = !_pathY;
            if (_pathY) y++;
            if (!_pathY) x++;
        }
        if (!_pathY) { x--; y++; }
        _track[_length - 3] = Instantiate(trackPrefab, new Vector3(x, 0, y), Quaternion.identity);
        _track[_length - 3].GetComponent<MeshRenderer>().material = trackTx[Random.Range(0, trackTx.Length)];
        y++;
        _track[_length - 2] = Instantiate(trackPrefab, new Vector3(x, 0, y), Quaternion.identity);
        _track[_length - 2].GetComponent<MeshRenderer>().material = trackTx[Random.Range(0, trackTx.Length)];
        y += 2;
        _track[_length - 1] = Instantiate(platformPrefab, new Vector3(x, 0, y), Quaternion.identity);
        _track[_length - 1].GetComponent<MeshRenderer>().material = platformTx[Random.Range(0, platformTx.Length)];
        Instantiate(ball1Prefab, new Vector3(0, 1, 0), ball1Prefab.transform.rotation);
        Instantiate(ball2Prefab, new Vector3(x, 1, y), ball2Prefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
