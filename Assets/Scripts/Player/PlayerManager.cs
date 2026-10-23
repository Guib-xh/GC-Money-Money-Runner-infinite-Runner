using System.Collections;
using Boss;
using Countdown;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        //GAME CONFIG
        [SerializeField] private SSO_GameConfig _gameConfig;
        [SerializeField] private AnimationCurve _jumpHeightCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        //DISPATCH
        [SerializeField] private RSE_MoveLeft _moveLeftEvent;
        [SerializeField] private RSE_MoveRight _moveRightEvent;
        [SerializeField] private RSE_UsePowerUpRequested _usePowerUpRequestedEvent;
        
        //LISTEN
        [SerializeField] private InputActionReference _moveLeftInputActionReference;
        [SerializeField] private InputActionReference _moveRightInputActionReference;
        [SerializeField] private InputActionReference _jumpInputActionReference;
        [SerializeField] private InputActionReference _slideInputActionReference;
        [SerializeField] private InputActionReference _usePowerUpActionReference;
        
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        
        //WRITE
        [SerializeField] private RSO_CurrentLine _currentLine;
        
        //READ
        [SerializeField] private RSO_BossActive _bossActive;
        [SerializeField] private RSO_CountdownActive _countdownActive;

        private int _pendingLineMove;
        private bool _isMovingLine;
        private bool _isJumping;
        private bool _isSliding;
        private Coroutine _slideCoroutine;


        private void OnEnable()
        {
            _moveLeftInputActionReference.action.performed += OnMoveLeftRequested;
            _moveRightInputActionReference.action.performed += OnMoveRightRequested;
            _jumpInputActionReference.action.performed += OnJumpRequested;
            _slideInputActionReference.action.performed += OnSlideRequested;
            _usePowerUpActionReference.action.performed += OnUsePowerUpRequested;

            _runStartedEvent.Event += Initialize;
        }

        private void OnDisable()
        {
            _moveLeftInputActionReference.action.performed -= OnMoveLeftRequested;
            _moveRightInputActionReference.action.performed -= OnMoveRightRequested;
            _jumpInputActionReference.action.performed -= OnJumpRequested;
            _slideInputActionReference.action.performed -= OnSlideRequested;
            _usePowerUpActionReference.action.performed -= OnUsePowerUpRequested;

            _runStartedEvent.Event -= Initialize;
        }

        private void Initialize()
        {
            //Debug.Log("Initializing Player Manager");
            StopAllCoroutines();
            _isMovingLine = false;
            _pendingLineMove = 0;
            _isSliding = false;
            _isJumping = false;

            _currentLine.Value = (_gameConfig.lineCount - 1) / 2;
            SetX(0);
        }
        
        //COLLISION

        private void OnTriggerEnter(Collider other)
        {
            other.GetComponentInParent<IRunnerTrigger>()?.OnPlayerTrigger();
        }
        
        //MOUVEMENTS GAUCHE / DROITE

        private void OnMoveLeftRequested(InputAction.CallbackContext _)
        {
            if (IsPlayerCanMove()) return;
            _moveLeftEvent.Dispatch();
            RequestLineMovement(-1);
        }

        private void OnMoveRightRequested(InputAction.CallbackContext _)
        {
            if (IsPlayerCanMove()) return;
            _moveRightEvent.Dispatch();
            RequestLineMovement(1);
        } 

        private void RequestLineMovement(int direction)
        {
            //Debug.Log("Requesting Line Movement");
            int target = _currentLine.Value + direction;
            if (target < 0 || target >= _gameConfig.lineCount) return; // hors piste

            if (_isMovingLine)
            {
                _pendingLineMove = direction;
                return;
            }
            StartCoroutine(MoveLineRoutine(direction));

        }
        
        private IEnumerator MoveLineRoutine(int direction)
        {
            _isMovingLine = true;
            int fromLane = _currentLine.Value;
            int toLine = _currentLine.Value + direction;
            float fromX = LaneToX(fromLane);
            float toX = LaneToX(toLine);

            float t = 0f;
            while (t < _gameConfig.lateralMoveDuration)
            {
                t += Time.unscaledDeltaTime;
                float p = Mathf.Clamp01(t / _gameConfig.lateralMoveDuration);
                SetX(Mathf.Lerp(fromX, toX, p));
                yield return null;
            }

            SetX(toX);

            _currentLine.Value = toLine;
            _isMovingLine = false;

            if (_pendingLineMove != 0)
            {
                int nextLaneMove = _pendingLineMove;
                _pendingLineMove = 0;
                RequestLineMovement(nextLaneMove);
            }
        }

        private float LaneToX(int lane) => (lane - (_gameConfig.lineCount - 1) / 2f) * _gameConfig.lineWidthLv;

        private void SetX(float newX)
        {
            Vector3 p = transform.localPosition;
            p.x = newX;
            transform.localPosition = p;
        }
        
        //MOUVEMENTS SAUT
        
        private void OnJumpRequested(InputAction.CallbackContext _)
        {
            if (IsPlayerCanMove()) return;
            if (_isJumping || _isSliding) return;
            StartCoroutine(JumpCoroutine());
        }

        private IEnumerator JumpCoroutine()
        {
            _isJumping = true;
            float t = 0f;
            while (t < _gameConfig.jumpDuration)
            {
                t += Time.deltaTime;
                float p = Mathf.Clamp01(t / _gameConfig.jumpDuration);
                SetHeight(_gameConfig.jumpHeight * _jumpHeightCurve.Evaluate(p));
                yield return null;
            }

            SetHeight(0);
            _isJumping = false;
        }

        private void SetHeight(float height)
        {
            Vector3 p = transform.localPosition;
            p.y = height;
            transform.localPosition = p;
        }
        
        //MOUVEMENTS SLIDE

        private void OnSlideRequested(InputAction.CallbackContext _)
        {
            if (IsPlayerCanMove()) return;
            if (_isJumping || _isSliding) return;
            _slideCoroutine = StartCoroutine(SlideCoroutine());
        }

        private IEnumerator SlideCoroutine()
        {
            _isSliding = true;
            SetScaleY(_gameConfig.slideHeightScale);
            float t = 0f;
            while (t < _gameConfig.slideDuration)
            {
                t += Time.deltaTime;
                yield return null;
            }

            StopSlide();
        }

        private void StopSlide()
        {
            if (_slideCoroutine != null) StopCoroutine(_slideCoroutine);
            _isSliding = false;
            SetScaleY(1f);
        }

        private void SetScaleY(float scaleY)
        {
            Vector3 s = transform.localScale;
            s.y = scaleY;
            transform.localScale = s;
        }
        
        //UTilisation PowerUp

        private void OnUsePowerUpRequested(InputAction.CallbackContext context)
        {
            if (IsPlayerCanMove()) return;
            _usePowerUpRequestedEvent.Dispatch();
        }

        private bool IsPlayerCanMove()
        {
            return _bossActive.Value || _countdownActive.Value;
        }

    }
}
