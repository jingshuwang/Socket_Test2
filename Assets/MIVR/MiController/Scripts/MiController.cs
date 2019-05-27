//-----------------------------------------------------------------------
// <copyright file="MiController.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    /// <summary>
    /// Mi controller.
    /// </summary>
    public class MiController : MonoBehaviour
    {
        /// <summary>
        /// UI canvas of this controller
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Canvas UICanvas;

        /// <summary>
        /// Enable controller model
        /// </summary>
        private static bool enableControllerModel = true;

        /// <summary>
        /// Is Controller Visible
        /// </summary>
        private static bool isControllerVisible = false;

        /// <summary>
        /// The controller
        /// </summary>
        private GameObject controller;

        /// <summary>
        /// Last controller's quaternion
        /// </summary>
        private Quaternion lastControllerQuat;

        /// <summary>
        /// lastFrameRepeat for statistic
        /// </summary>
        private bool lastFrameRepeat = false;

        /// <summary>
        /// repeatDataCount for statistic
        /// </summary>
        private int repeatDataCount = 0;

        /// <summary>
        /// doubleRepeatDataCount for statistic
        /// </summary>
        private int doubleRepeatDataCount = 0;

        /// <summary>
        /// zeroDataCount for statistic
        /// </summary>
        private int zeroDataCount = 0;

        /// <summary>
        /// totalDataCount for statistic
        /// </summary>
        private int totalDataCount = 0;

        /// <summary>
        /// Frame Counting
        /// </summary>
        private int frameCount = 0;

        /// <summary>
        /// Gets or sets instance of the controller
        /// </summary>
        public static MiController Instance { get; set; }

        /// <summary>
        /// Gets or sets enable state
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed.")]
        public static bool EnableControllerModel
        {
            get
            {
                return enableControllerModel;
            }

            set
            {
                enableControllerModel = value;
            }
        }

        /// <summary>
        /// controller visibility
        /// </summary>
        /// <param name="isVisible">whether controller is visible.</param>
        public void Visibility(bool isVisible)
        {
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(isVisible);
            }

            isControllerVisible = isVisible;
        }

        /// <summary>
        /// Awake function
        /// </summary>
        private void Awake()
        {
            if (!EnableControllerModel)
            {
                gameObject.SetActive(false);
                return;
            }

            Instance = this;

            this.Visibility(false);
        }

        /// <summary>
        /// Update transform of controller model.
        /// </summary>
        private void UpdateTransform()
        {
            this.transform.localPosition = InputManager.ControllerState.Position;
            this.transform.localRotation = InputManager.ControllerState.Orientation;
        }

        /// <summary>
        /// Update visibility of controller model.
        /// </summary>
        private void UpdateVisibility()
        {
            if (InputManager.ControllerState.ConnectionState != ConnectionState.Connected)
            {
                if (isControllerVisible)
                {
                    this.Visibility(false);
                }
            }
            else
            {
                if (!isControllerVisible)
                {
                    this.Visibility(true);
                }
            }
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        private void Update()
        {
            // Debug.Log("MiController.Update: " + frameCount);
            this.frameCount++;

            this.UpdateTransform();

            this.UpdateVisibility();

            /* for statistic
            if (lastControllerQuat == orientaiton)
            {
                if (lastFrameRepeat)
                {
                    doubleRepeatDataCount++;
                }
                lastFrameRepeat = true;
                repeatDataCount++;
            }
            else
            {
                lastFrameRepeat = false;
            }

            lastControllerQuat = orientaiton;

            if (orientaiton.x == 0.0f || orientaiton.y == 0.0f || orientaiton.z == 0.0f || orientaiton.w == 0.0f)
            {
                zeroDataCount++;
            }

            totalDataCount++;
            if (totalDataCount % 500 == 0)
            {
                float repeatDataRate = (float)this.repeatDataCount / (float)this.totalDataCount;
                Debug.Log("total: " + this.totalDataCount + " repeat data count: " + this.repeatDataCount + " rate: " + this.repeatDataRate);
                float doubleRepeatDataRate = (float)this.doubleRepeatDataCount / (float)this.totalDataCount;
                Debug.Log("total: " + this.totalDataCount + " double repeat data count: " + this.doubleRepeatDataCount + " rate: " + this.doubleRepeatDataRate);
                // float zeroDataRate = (float)zeroDataCount / (float)totalDataCount;
                // Debug.Log("total: " + totalDataCount + " zero data count: " + zeroDataCount + " rate: " + zeroDataRate);
            }
            //*/
        }
    }
}