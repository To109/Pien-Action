using UnityEngine;
using UnityEngine.InputSystem;

public class EnemySpawner : MonoBehaviour
{
    // スポーンする敵のプレハブ
    [SerializeField, Header("敵オブジェクト")]
    private GameObject _enemy;

    private Player _player; // プレイヤーの参照
    private GameObject _enemyObj; // 生成した敵オブジェクトの参照

    private bool _hasSpawned = false; // 敵を一度でも生成したかどうかのフラグ

    void Start()
    {
        _player = FindObjectOfType<Player>(); // シーン内のPlayerオブジェクトを探す
        _enemyObj = null; // 敵オブジェクトはまだ生成されていないのでnullに初期化
    }

    void Update()
    {
        _SpawnEnemy(); // 敵生成処理を呼び出し
    }//comment

    // 敵を生成する処理
    private void _SpawnEnemy()
    {
        // プレイヤーが存在しないか、すでに敵を生成済みの場合は処理を中断
        if (_player == null || _hasSpawned)
        {
            return;
        }

        // プレイヤーの現在位置
        Vector3 playerPos = _player.transform.position;
        // カメラの画面右上のワールド座標を取得
        // Z座標はカメラからの距離を正の値にして渡す必要があるため Mathf.Abs(Camera.main.transform.position.z)
        Vector3 cameraMaxPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Mathf.Abs(Camera.main.transform.position.z)));
        // 敵オブジェクトのスケール（大きさ）
        Vector3 scale = _enemy.transform.localScale;

        // 敵スポーン地点とプレイヤーの距離（X方向のみ比較）
        float distance = Vector2.Distance(transform.position, new Vector2(playerPos.x, transform.position.y));
        // プレイヤー位置とカメラ右端（敵オブジェクトの幅の半分を足して余裕を持たせた位置）との距離
        float spawnDis = Vector2.Distance(playerPos, new Vector2(cameraMaxPos.x + scale.x / 2.0f, playerPos.y));

        // プレイヤーが一定距離以内に近づき、まだ敵が生成されていなければ敵を生成
        if (distance <= spawnDis && _enemyObj == null)
        {
            _enemyObj = Instantiate(_enemy, transform.position, Quaternion.identity); // 敵をスポーン位置に生成
            _hasSpawned = true; // 敵を生成済みにする
        }
    }
}
