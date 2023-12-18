using UnityEngine;


namespace Movement
{
    [RequireComponent(typeof(PlayerInputs))]
    public class Player : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField] public PlayerSO Data { get; private set; }


        [field: Header("Collisions")]
        [field: SerializeField] public CapsuleColliderUtilitiy ColliderUtilitiy { get; private set; }
        [field: SerializeField] public PlayerLayerData LayerData { get; private set; }
        [field: SerializeField] public PlayerAnimationData AnimationsData { get; private set; }
        [field: SerializeField] public PlayerParticalData ParticalData { get; private set; }
        [field: SerializeField] public PlayerUIData UIData { get; private set; }
        [field: SerializeField] public PlayerTestingData TestingData { get; private set; }





        public Transform playerTransform { get; private set; }
        public CharacterController CharacterController { get; private set; }
        public Animator Animator { get; private set; }

        public PlayerInputs Inputs { get; private set; }
        public Transform playerCamera { get; private set; }
        public PlayerMovementStateMachine movementStateMachine;

        private void Awake()
        {
            CharacterController = GetComponent<CharacterController>();
            Animator = GetComponent<Animator>();
            Inputs = GetComponent<PlayerInputs>();

            AnimationsData.Intialoze();

            playerCamera = Camera.main.transform;
            movementStateMachine = new PlayerMovementStateMachine(this);
        }
        private void OnValidate()
        {
            ColliderUtilitiy.Initialize(gameObject);
            ColliderUtilitiy.CalculateCapsuleColliderDimensions();
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
        private void OnTriggerExit(Collider collider)
        {
            movementStateMachine.OnTriggerExit(collider);
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
            Vector3 hitpostion = movementStateMachine.ReusableData.rayFindLedge.point;
            playerTransform.position = Vector3.Slerp(movementStateMachine.Player.playerTransform.position, hitpostion, 1);
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

