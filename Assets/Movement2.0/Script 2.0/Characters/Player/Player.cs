using UnityEngine;
using Cinemachine;


namespace Movement
{
    [RequireComponent(typeof(PlayerInputs))]
    [RequireComponent(typeof(CapsuleColliderUtilitiy))]
    public class Player : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField] public PlayerSO Data { get; private set; }
        [field: SerializeField] public PlayerCoolDownData CoolDownData { get; private set; }
        [field: SerializeField] public CinemachineInputProvider cinemachineInput { get; private set; }


        [field: Header("Collisions")]
        [field: SerializeField] public PlayerLayerData LayerData { get; private set; }
        [field: SerializeField] public PlayerAnimationData AnimationsData { get; private set; }
        [field: SerializeField] public PlayerParticalData ParticalData { get; private set; }
        [field: SerializeField] public PlayerUIData UIData { get; private set; }
        [field: SerializeField] public PlayerTestingData TestingData { get; private set; }





        public Transform playerTransform { get; private set; }
        public CharacterController CharacterController { get; private set; }
        public Animator Animator { get; private set; }

        public PlayerInputs Inputs { get; private set; }
        public CapsuleColliderUtilitiy ColliderUtilitiy { get; private set; }
        public Transform playerCamera { get; private set; }
        public PlayerMovementStateMachine movementStateMachine;

        private void Awake()
        {
            CharacterController = GetComponent<CharacterController>();
            Animator = GetComponent<Animator>();
            Inputs = GetComponent<PlayerInputs>();
            ColliderUtilitiy = GetComponent<CapsuleColliderUtilitiy>();

            AnimationsData.Intialoze();

            playerCamera = Camera.main.transform;
            movementStateMachine = new PlayerMovementStateMachine(this);
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerTransform = transform;
            movementStateMachine.ChangeState(movementStateMachine.IdleingState);
        }
        private void OnTriggerEnter(Collider collider)
        {
            movementStateMachine.OnTriggerEnter(collider);
        }
        private void OnTriggerStay(Collider collider)
        {
            movementStateMachine.OnTriggerStay(collider);
        }
        private void OnTriggerExit(Collider collider)
        {
            movementStateMachine.OnTriggerExit(collider);
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 3f);
        }

        private void Update()
        {
            movementStateMachine.HandleInput();
            movementStateMachine.Update();
        }
        private void FixedUpdate()
        {
            movementStateMachine.PhysicsUpdate();
        }
        private void LerpToLedge()
        {
            Vector3 hitpostion = new Vector3(movementStateMachine.ReusableData.faceLedge.point.x, movementStateMachine.ReusableData.rayFindLedge.point.y, movementStateMachine.ReusableData.faceLedge.point.z);
            playerTransform.position = Vector3.Slerp(movementStateMachine.Player.playerTransform.position, hitpostion, 1f);
            playerTransform.position = playerTransform.TransformPoint(movementStateMachine.ReusableData.playerOffect);
        }

        private void OnAnimatorMove()
        {
            movementStateMachine.OnAnimationMove();
        }
        public void OnMovementStateAnimationEnterEvent()
        {
            movementStateMachine.OnAnimationEnterEvent();
        }

        public void OnMovementStateAnimationExitEvent()
        {
            movementStateMachine.OnAnimationExitEvent();
        }

        public void OnMovementStateAnimationTransitionEvent()
        {
            movementStateMachine.OnAnimationTransitionEvent();
        }
    }
}

