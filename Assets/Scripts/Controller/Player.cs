using UnityEngine;
using UnityEngine.InputSystem;


//New Input System 사용
//Input Action Asset을 C# Class로 생성하는 방법을 사용.
//Input Action Asset의 Action Map들을 설정하고, 해당 Action Map의 이벤트를 발생시킬 입력(컨트롤러, 키보드 등)을 바인딩한다.
//해당하는 키 입력이 들어오면 액션의 설정한 입력의 값이나, 버튼이 눌렸는지 여부(입력 정보)를 담은 이벤트가 실행된다.
//이벤트 실행 시 액션에 등록된 이벤트 핸들러 함수들을 전부 실행하며, 입력 정보를 이벤트 핸들러 함수의 인자로 전달한다.

public class Player : MonoBehaviour
{
    private Camera playerCamera;
    private Animator animator;

    private bool isMoving = false;
    private float moveSpeed = 5;
    private Vector3 moveDirection = Vector3.zero;
    private LayerMask wall;

    private void Awake()
    {
        playerCamera = Camera.main; 
        animator = GetComponentInChildren<Animator>();
        wall = LayerMask.GetMask("Wall");
    }

    //예시로 Performed는 입력이 진행중일 때, canceled는 입력이 끊기는 순간 발생하는 이벤트.
    private void OnEnable()
    {
        //플레이어 조작 중일 때는 UI 조작 모드를 비활성화
        InputActions.keyActions.UI.Disable();

        //플레이어 키입력에 따라 발생하는 이벤트 등록
        InputActions.keyActions.Player.Move.performed += OnMovePerformed;
        InputActions.keyActions.Player.Move.canceled += OnMoveCanceled;
        InputActions.keyActions.Player.Check.started += OnCheckStarted;

        //레벨 클리어 시 발생하는 이벤트 등록
        StageManager.Instance.OnLevelClear += Spawn;
    }

    private void OnDisable()
    {
        InputActions.keyActions.UI.Enable();

        InputActions.keyActions.Player.Move.performed -= OnMovePerformed;
        InputActions.keyActions.Player.Move.canceled -= OnMoveCanceled;
        InputActions.keyActions.Player.Check.started -= OnCheckStarted;

        StageManager.Instance.OnLevelClear -= Spawn;
    }

    //이동, 회전
    private void Update()
    {
        //빛 발사 후, 벽 layerMask가 안 걸리면서 키가 입력되는 상태일 때 이동
        if (!Physics.Raycast(transform.position + Vector3.up, moveDirection, 0.5f, wall))
        {
            if(isMoving)
            {
                gameObject.transform.position += moveDirection * Time.deltaTime * moveSpeed;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection, Vector3.up), 0.2f);
            }
        }
    }

	//Action의 입력 정보를 context, 리턴 값은 ReadValue로 가져올 수 있음. Up으로 매핑한 입력이 들어오면 Vector2(0, 1) 값을 가져오는 식.
    //이동 방향은 카메라 기준이므로, 카메라를 기준으로 플레이어가 이동할 방향 벡터를 만들어 준다.
    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        isMoving = true;
        Vector2 input = context.ReadValue<Vector2>();

        moveDirection = (input.x * playerCamera.transform.right) + (input.y * playerCamera.transform.forward);
        moveDirection.y = 0;
        moveDirection.Normalize();

        animator.CrossFade("RUN", 0.1f); //RUN 애니메이션으로 0.1f 간의 겹치는 시간을 가지고 부드럽게 전환.
    }

    public void OnMoveCanceled(InputAction.CallbackContext context)
    {
        isMoving = false;
        animator.CrossFade("WAIT", 0.2f);
    }

    //엔터키 누르면 작동하는 심볼과 상호작용을 하는 함수. 
    public void OnCheckStarted(InputAction.CallbackContext context)
    {

        //레이캐스트 범위에 심볼이 걸린 채로 말을 걸면, 충돌한 오브젝트의 정보를 얻는다
        //충돌한 오브젝트가 심볼인 경우, 심볼의 인카운터 함수를 실행.
        Physics.Raycast(transform.position + Vector3.up, moveDirection, out RaycastHit raycastHit, 2.0f);

        if (raycastHit.collider == null)
        {
            return;
        }

        if (raycastHit.collider.TryGetComponent(out RoomSymbol encountedSymbol))
        {
            encountedSymbol.TalkStart();
        }
    }

    public void Spawn()
    {
        transform.position = Vector3.zero;
    }
}
