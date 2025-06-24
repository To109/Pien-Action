using UnityEngine;

public class SubEnemy : MonoBehaviour
{
    private GameObject _player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = FindAnyObjectByType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = _player.transform.position - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector2.left, dir);
    }
}
