using UnityEngine;


namespace Nomimovment
{
    [RequireComponent(typeof(PlayerInputs))]
    [RequireComponent(typeof(PlayerResizableCapsuleCollider))]
    public class Player : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField] public PlayerSO Data { get; private set; }


        [field: Header("Collisions")]
        public PlayerResizableCapsuleCollider ResizableCapsuleCollider { get; private set; }
        [field: SerializeField] public PlayerLayerData LayerData { get; private set; }
        [field: Header("Cameras")]
        [field: SerializeField] public PlayerCameraRecenteringUtility CameraRecenteringUtility { get; private set; }



        [field: SerializeField] public PlayerAnimationData AnimationsData { get; private set; }
        [field: SerializeField] public PlayerParticalData ParticalData { get; private set; }


        public Transform playerTransform { get; private set; }
        //public CharacterController CharacterController { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        public Animator Animator { get; private set; }

        public PlayerInputs Inputs { get; private set; }
        public Transform playerCamera { get; private set; }
        public PlayerMovementStateMachine movementStateMachine;

        private void Awake()
        {
            //CharacterController = GetComponent<CharacterController>();
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponent<Animator>();
            ResizableCapsuleCollider = GetComponent<PlayerResizableCapsuleCollider>();

            AnimationsData.Intialoze();

            playerCamera = Camera.main.transform;
            movementStateMachine = new PlayerMovementStateMachine(this);
        }

        private void Start()
        {
            Inputs = GetComponent<PlayerInputs>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerTransform = transform;
            movementStateMachine.ChangeState(movementStateMachine.IdleingState);
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
        private void OnTriggerEnter(Collider collider)
        {
            movementStateMachine.OnTriggerEnter(collider);
        }
        private void OnTriggerExit(Collider collider)
        {
            movementStateMachine.OnTriggerExit(collider);
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
