using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player3D : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float DeadZone = 0.2f;

    [SerializeField, Header("ジャンプ速度")]
    private float _jumpSpeed;
    private Vector2 _moveInput;
    private Rigidbody _rigid;
    private ScriptChanger changer; // 共有元参照
    public void SetChanger(ScriptChanger sc) => changer = sc;

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        HitFloor();
    }

    private void FixedUpdate()
    {
        // 前方
        if (Mathf.Abs(_moveInput.y) > DeadZone)
        {
            transform.Translate(Vector3.forward * _moveInput.y * MoveSpeed * Time.deltaTime);
        }
        // 左右
        if (Mathf.Abs(_moveInput.x) > DeadZone)
        {
            transform.Translate(Vector3.right * _moveInput.x * MoveSpeed * Time.deltaTime);
        }
    }

    //プレイヤーが床に接しているかどうかを判定するメソッド
    private void HitFloor()
    {
        // 地面との判定に使うレイヤーを取得（Layer名: "floor"）
        int layerMask = LayerMask.GetMask("floor");

        // レイを飛ばす起点（プレイヤーの足元）
        Vector3 rayOrigin = transform.position;

        // プレイヤーの半分の高さぶん下にレイを飛ばす
        float rayDistance = (transform.localScale.y / 2f) + 0.1f;

        // 下方向にRaycastして床に当たるかチェック
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayDistance, layerMask))
        {
            if (changer.IsJumping && hit.collider.CompareTag("floor"))
            {
                changer.IsJumping = false; // 着地と判定
            }
        }
        else
        {
            changer.IsJumping = true; // 床に触れていない = 空中
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //もしジャンプ操作が行われていないまたは、_bJumpがtrueの時はreturn以降の処理を行わない
        if (!context.performed || changer.IsJumping)
        {
            return;
        }

        //上方向に_jumpSpeed分徐々に加速させる
        _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode.Impulse);
    }
}