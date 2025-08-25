using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptChanger : MonoBehaviour
{
    private Player2D _moveTypeA;
    private Player3D _moveTypeB;

    private bool move = true;

    public bool IsJumping { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        // コンポーネント取得
        _moveTypeA = GetComponent<Player2D>();
        _moveTypeB = GetComponent<Player3D>();

        // 初期状態設定
        _moveTypeA.enabled = move;
        _moveTypeB.enabled = !move;

        // ScriptChangerの参照を渡す
        _moveTypeA.SetChanger(this);
        _moveTypeB.SetChanger(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 切り替え
            move = !move;

            _moveTypeA.enabled = move;
            _moveTypeB.enabled = !move;
        }
    }
}
