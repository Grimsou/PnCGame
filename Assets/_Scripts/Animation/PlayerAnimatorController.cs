using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Gameplay
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        #region Private fields

        [SerializeField] private Animator _animator;
        [SerializeField] private float _directionDampTime = 0.25f;

        #endregion  

        #region MonoBehaviour Callbacks

        // Start is called before the first frame update
        void Start()
        {
            if (!_animator)
            {
                Debug.LogError("PlayerAnimatorManager was Missing Animator Component so we referenced it", this);
                _animator = GetComponent<Animator>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!_animator)
            {
                return;
            }

            // deal with Jumping
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            // only allow jumping if we are running.
            if (stateInfo.IsName("Base Layer.Run"))
            {
                // When using trigger parameter
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _animator.SetTrigger("Jump");
                }
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (v <= 0)
            {
                v = 0;
            }

            _animator.SetFloat("Speed", h * h + v * v);
            _animator.SetFloat("Direction", h, _directionDampTime, Time.deltaTime);
        }

        #endregion
    }
}

