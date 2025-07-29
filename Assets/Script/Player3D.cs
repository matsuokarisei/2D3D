using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player3D : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float DeadZone = 0.2f;

    [SerializeField, Header("�W�����v���x")]
    private float _jumpSpeed;
    private Vector2 _moveInput;
    private Rigidbody _rigid;
    private bool _bJump;

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
        // �O��
        if (_moveInput.y > DeadZone || _moveInput.y < -DeadZone)
        {
            transform.Translate(Vector3.forward * _moveInput.y * Time.deltaTime);
        }
        // ���E
        if (_moveInput.x > DeadZone || _moveInput.x < -DeadZone)
        {
            transform.Translate(Vector3.right * _moveInput.x * Time.deltaTime);
        }
    }

    //�v���C���[�����ɐڂ��Ă��邩�ǂ����𔻒肷�郁�\�b�h
    private void HitFloor()
    {
        // �n�ʂƂ̔���Ɏg�����C���[���擾�iLayer��: "floor"�j
        int layerMask = LayerMask.GetMask("floor");

        // ���C���΂��N�_�i�v���C���[�̑����j
        Vector3 rayOrigin = transform.position;

        // �v���C���[�̔����̍����Ԃ񉺂Ƀ��C���΂�
        float rayDistance = (transform.localScale.y / 2f) + 0.1f;

        // ��������Raycast���ď��ɓ����邩�`�F�b�N
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayDistance, layerMask))
        {
            if (_bJump && hit.collider.CompareTag("floor"))
            {
                _bJump = false; // ���n�Ɣ���
            }
        }
        else
        {
            _bJump = true; // ���ɐG��Ă��Ȃ� = ��
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //�����W�����v���삪�s���Ă��Ȃ��܂��́A_bJump��true�̎���return�ȍ~�̏������s��Ȃ�
        if (!context.performed || _bJump)
        {
            return;
        }

        //�������_jumpSpeed�����X�ɉ���������
        _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode.Impulse);
    }
}