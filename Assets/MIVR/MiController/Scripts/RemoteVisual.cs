//-----------------------------------------------------------------------
// <copyright file="RemoteVisual.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    /// <summary>
    /// RemoteVisual for v1 or v1o
    /// </summary>
    public class RemoteVisual : MonoBehaviour
    {
        /// <summary>
        /// Mi controller v1o trigger model.
        /// </summary>
        private Transform trigger;

        /// <summary>
        /// Mi controller v1o trigger animator.
        /// </summary>
        private Animator triggerAnim;

        /// <summary>
        /// Mi controller triangle button.
        /// </summary>
        private Transform btnTriangle;

        /// <summary>
        /// Mi controller triangle button animator.
        /// </summary>
        private Animator triangleAnim;

        /// <summary>
        /// Mi controller system button.
        /// </summary>
        private Transform btnCicle;

        /// <summary>
        /// Mi controller touch pad.
        /// </summary>
        private Transform btnTouch;

        /// <summary>
        /// Mi controller system button animator.
        /// </summary>
        private Animator circleAnim;

        /// <summary>
        /// Mi controller touch pad animator.
        /// </summary>
        private Animator touchAnim;

        /// <summary>
        /// Mi controller touch pad center position.
        /// </summary>
        private Vector3 centerPos;

        /// <summary>
        /// whether Mi controller is v1 or v1o.
        /// </summary>
        private bool isV1o = false;

        /// <summary>
        /// initialize controller model.
        /// </summary>
        private bool isInitialized = false;

        /// <summary>
        /// Awake function.
        /// </summary>
        private void Awake()
        {
            this.isV1o = VrManager.Instance.Model == VrManager.DeviceModel.MIVRAllInOne ? true : false;

            if (!this.isInitialized)
            {
                this.btnTouch = this.transform.Find("Canvas/BtnTouch");
                this.btnTriangle = this.transform.Find("Canvas/BtnTriangle");
                this.btnCicle = this.transform.Find("Canvas/BtnCircle");

                this.circleAnim = this.btnCicle.GetComponent<Animator>();
                this.triangleAnim = this.btnTriangle.GetComponent<Animator>();
                this.touchAnim = this.btnTouch.GetComponent<Animator>();
                this.centerPos = this.btnTouch.GetComponent<RectTransform>().localPosition;

                this.circleAnim.SetBool("pressed", false);
                this.triangleAnim.SetBool("pressed", false);
                this.touchAnim.SetBool("pressed", false);
                this.touchAnim.SetBool("touched", false);

                if (this.isV1o)
                {
                    this.trigger = this.transform.Find("Controller/trigger");
                    this.triggerAnim = this.trigger.GetComponent<Animator>();

                    this.trigger.gameObject.SetActive(true);
                    this.triggerAnim.SetBool("pressed", false);
                }

                this.isInitialized = true;
            }
        }

        /// <summary>
        /// Update function.
        /// </summary>
        private void Update()
        {
            if (InputManager.ControllerState.IsTouching)
            {
                this.touchAnim.SetBool("touched", true);
                RectTransform trans = this.btnTouch.transform.GetChild(0).GetComponent<RectTransform>();

                Vector2 touchPos;

                if (InputManager.ControllerState.IsTouching)
                {
                    touchPos = InputManager.ControllerState.TouchPosition;
                }
                else
                {
                    // for test
                    touchPos = new Vector2(0.5f, 0.5f);
                }

                float touchx = Mathf.Clamp01(touchPos.x) - 0.5f;
                float touchy = 0.5f - Mathf.Clamp01(touchPos.y);
                float diameter = 0;
                if (this.isV1o)
                {
                    diameter = 43f;
                }
                else
                {
                    diameter = 18f;
                }

                Vector3 offset = new Vector3(touchx * diameter, touchy * diameter, 0);

                if (!this.isV1o)
                {
                    float distance = offset.magnitude;
                    float outerDis = distance + 2.0f;
                    if (outerDis > (diameter / 2.0f) && offset.y > 5f && Mathf.Abs(offset.x) > 5f)
                    {
                        if (offset.x < 0)
                        {
                            offset.x += 2f;
                        }
                        else
                        {
                            offset.x -= 2f;
                        }

                        offset.y -= 2f;
                    }
                }
                else
                {
                    offset.y += 2.5f;
                }

                trans.localPosition = offset;
            }
            else
            {
                this.touchAnim.SetBool("touched", false);
            }

            if (InputManager.ControllerState.ClickButtonState)
            {
                this.touchAnim.SetBool("pressed", true);
            }
            else
            {
                this.touchAnim.SetBool("pressed", false);
            }

            if (InputManager.ControllerState.AppButtonState)
            {
                if (this.isV1o)
                {
                    this.triggerAnim.SetBool("pressed", true);
                }
                else
                {
                    this.triangleAnim.SetBool("pressed", true);
                }
            }
            else
            {
                if (this.isV1o)
                {
                    this.triggerAnim.SetBool("pressed", false);
                }
                else
                {
                    this.triangleAnim.SetBool("pressed", false);
                }
            }

            if (InputManager.ControllerState.MenuButtonState)
            {
                this.circleAnim.SetBool("pressed", true);
            }
            else
            {
                this.circleAnim.SetBool("pressed", false);
            }
        }
    }
}