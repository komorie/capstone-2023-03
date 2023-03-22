using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


//New Input System ���
//Input Action Asset�� C# Class�� �����ϴ� ����� ���.
//Input Action Asset�� Action Map���� �����ϰ�, �ش� Action Map�� �̺�Ʈ�� �߻���ų �Է�(��Ʈ�ѷ�, Ű���� ��)�� ���ε��Ѵ�.
//�ش��ϴ� Ű �Է��� ������ �׼��� ������ �Է��� ���̳�, ��ư�� ���ȴ��� ����(�Է� ����)�� ���� �̺�Ʈ�� ����ȴ�.
//�̺�Ʈ ���� �� �׼ǿ� ��ϵ� �̺�Ʈ �ڵ鷯 �Լ����� ���� �����ϸ�, �Է� ������ �̺�Ʈ �ڵ鷯 �Լ��� ���ڷ� �����Ѵ�.

public class Player : MonoBehaviour
{
    private Camera playerCamera;
    private Animator animator;

    private bool isMoving = false;
    private float moveSpeed = 5;
    private Vector3 moveDirection = Vector3.zero;

    private void Awake()
    {
        if(Camera.main == null)
        {
            playerCamera = new GameObject().AddComponent<Camera>();
            playerCamera.tag = "MainCamera";
        }
        else
        {
            playerCamera = Camera.main;
        }
        playerCamera.AddComponent<PlayerCamera>();
        playerCamera.name = "PlayerCamera";
    }

    //���÷� Performed�� �Է��� �������� ��,  canceled�� �Է��� ����� ���� �߻��ϴ� �̺�Ʈ.
    private void OnEnable()
    {
        //�÷��̾� ���� ���� ���� UI ���� ��带 ��Ȱ��ȭ
        InputManager.Instance.KeyActions.UI.Disable();

        InputManager.Instance.KeyActions.Player.Move.performed += OnMovePerformed;
        InputManager.Instance.KeyActions.Player.Move.canceled += OnMoveCanceled;
        InputManager.Instance.KeyActions.Player.Check.started += Talk;
        LevelManager.Instance.onLevelClear += Spawn;
    }

    private void OnDisable()
    {
        InputManager.Instance.KeyActions.UI.Enable();

        InputManager.Instance.KeyActions.Player.Move.performed -= OnMovePerformed;
        InputManager.Instance.KeyActions.Player.Move.canceled -= OnMoveCanceled;
        InputManager.Instance.KeyActions.Player.Check.started -= Talk;
        LevelManager.Instance.onLevelClear -= Spawn;
    }

    //�̵�, ȸ��
    private void Update()
    {
        //����ĳ��Ʈ���� �ɺ��� �ɸ� ä�� ���� �ɸ�, �浹ü�� ������ ��´�
        //�浹ü�� �ɺ��̸� �ɺ��� ������ ���� ��ȭâ�� ���
        //���� �Ŵ� ���� ��ǲ�Ŵ����� Ư�� Ű & �ɺ��� ���� �� �Լ��� ���
        Debug.DrawRay(transform.position + Vector3.up, moveDirection * 2.0f, Color.red);

        if (isMoving)
        {
            gameObject.transform.position += moveDirection * Time.deltaTime * moveSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection, Vector3.up), 0.2f);
        }
    }

	//Action�� �Է� ������ context, ���� ���� ReadValue�� ������ �� ����.Up���� ������ �Է��� ������ Vector2(0, 1) ���� �������� ��.
    //�̵� ������ ī�޶� �����̹Ƿ�, ī�޶� �������� �÷��̾ �̵��� ���� ���͸� ����� �ش�.
    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        isMoving = true;
        Vector2 input = context.ReadValue<Vector2>();
        moveDirection = (input.x * playerCamera.transform.right) + (input.y * playerCamera.transform.forward);
        moveDirection.y = 0;
    }

    public void OnMoveCanceled(InputAction.CallbackContext context)
    {
        isMoving = false;
    }

    //�ɺ��� �ٶ󺸰� ����Ű ������ �۵�
    //�ٶ󺸴� ���� RoomSymbol�� ������ RoomSymbol�� SymbolEncounter �Լ� ����
    public void Talk(InputAction.CallbackContext context)

    {
        Physics.Raycast(transform.position + Vector3.up, moveDirection, out RaycastHit raycastHit, 2.0f);

        if (raycastHit.collider == null)
        {
            return;
        }

        if (raycastHit.collider.TryGetComponent(out RoomSymbol encountedSymbol))
        {
            encountedSymbol.SymbolEncounter();
        }
    }

    public void Spawn()
    {
        transform.position = Vector3.zero;
    }
}
