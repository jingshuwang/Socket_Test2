//-----------------------------------------------------------------------
// <copyright file="PointerInputModule.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// the gaze input module
    /// </summary>
    public class PointerInputModule : BaseInputModule
    {
        /// <summary>
        /// The gaze point which to cast rays, in viewport coordinates.
        /// </summary>
        private Vector2 gazePoint = new Vector2(0.5f, 0.5f);

        /// <summary>
        /// The pointer data
        /// </summary>
        private PointerEventData pointerData;

        /// <summary>
        /// Is active
        /// </summary>
        private bool actived = false;

        /// <summary>
        /// Should be activated.
        /// </summary>
        /// <returns>
        /// Should the module be activated.
        /// </returns>
        public override bool ShouldActivateModule()
        {
            bool activeState = base.ShouldActivateModule();

            if (activeState != this.actived)
            {
                this.actived = activeState;
            }

            return activeState;
        }

        /// <summary>
        /// Called when the module is deactivated. Override this if you want custom code to execute when you deactivate your module.
        /// </summary>
        public override void DeactivateModule()
        {
            base.DeactivateModule();
            if (this.pointerData != null)
            {
                this.HandlePendingClick();
                this.HandlePointerExitAndEnter(this.pointerData, null);
                this.pointerData = null;
            }

            this.eventSystem.SetSelectedGameObject(null, this.GetBaseEventData());
        }

        /// <summary>
        /// Is the pointer with the given ID over an EventSystem object?
        /// </summary>
        /// <param name="pointerId">Pointer ID.</param>
        /// <returns>true if the point over game object</returns>
        public override bool IsPointerOverGameObject(int pointerId)
        {
            return this.pointerData != null && this.pointerData.pointerEnter != null;
        }

        /// <summary>
        /// Process the current tick for the module.
        /// </summary>
        public override void Process()
        {
            GameObject gazeObjectPrevious = this.GetCurrentGameObject();

            this.CastRayFromGaze();
            this.UpdateCurrentObject();
            this.UpdateReticle(gazeObjectPrevious);

            Camera camera = this.pointerData.enterEventCamera;

            if (this.pointerData.pointerCurrentRaycast.gameObject != null && Input.GetMouseButtonDown(0))
            {
                ExecuteEvents.Execute(this.pointerData.pointerCurrentRaycast.gameObject, this.pointerData, ExecuteEvents.pointerDownHandler);
            }

            if (!this.pointerData.eligibleForClick &&
             (Input.GetMouseButtonUp(0) || InputManager.HmdButtonUp ||
             InputManager.ControllerState.ClickButtonUp))
            {
                this.HandleTrigger();
            }
            else if (!Input.GetMouseButton(0))
            {
                this.HandlePendingClick();
            }
        }

        /// <summary>
        /// Casts the ray from gaze.
        /// </summary>
        private void CastRayFromGaze()
        {
            if (this.pointerData == null)
            {
                this.pointerData = new PointerEventData(this.eventSystem);
            }

            this.pointerData.Reset();

            // this.pointerData.position = new Vector2(this.gazePoint.x * Screen.width, this.gazePoint.y * Screen.height);
            this.eventSystem.RaycastAll(this.pointerData, this.m_RaycastResultCache);

            RaycastResult raycastResult = FindFirstRaycast(m_RaycastResultCache);
            if (raycastResult.gameObject != null && raycastResult.worldPosition == Vector3.zero)
            {
                raycastResult.worldPosition = this.GetIntersectionPosition(this.pointerData.enterEventCamera, raycastResult);
            }

            this.pointerData.pointerCurrentRaycast = raycastResult;
            this.pointerData.position = raycastResult.screenPosition;
            this.m_RaycastResultCache.Clear();
        }

        /// <summary>
        /// Get the last ray.
        /// </summary>
        /// <returns>The last ray used.</returns>
        private Ray GetLastRay()
        {
            if (this.pointerData != null)
            {
                BasePointerRaycaster raycaster = this.pointerData.pointerCurrentRaycast.module as BasePointerRaycaster;
                if (raycaster != null)
                {
                    return raycaster.GetLastRay();
                }
                else if (this.pointerData.enterEventCamera != null)
                {
                    Camera cam = this.pointerData.enterEventCamera;
                    return new Ray(cam.transform.position, cam.transform.forward);
                }
            }

            return new Ray();
        }

        /// <summary>
        /// Updates the current object.
        /// </summary>
        private void UpdateCurrentObject()
        {
            var enterTarget = this.pointerData.pointerCurrentRaycast.gameObject;
            this.HandlePointerExitAndEnter(this.pointerData, enterTarget);

            var selected = ExecuteEvents.GetEventHandler<ISelectHandler>(enterTarget);
            if (selected == this.eventSystem.currentSelectedGameObject)
            {
                ExecuteEvents.Execute(
                    this.eventSystem.currentSelectedGameObject,
                    this.GetBaseEventData(),
                    ExecuteEvents.updateSelectedHandler);
            }
            else
            {
                this.eventSystem.SetSelectedGameObject(null, this.pointerData);
            }
        }

        /// <summary>
        /// Updates the reticle.
        /// </summary>
        /// <param name="previousGazedObject">The previous gazed object.</param>
        private void UpdateReticle(GameObject previousGazedObject)
        {
            if (LaserPointer.Instance == null)
            {
                return;
            }

            Camera camera = this.pointerData.enterEventCamera;
            GameObject currentGazeObject = this.GetCurrentGameObject();

            Vector3 intersectionPosition = this.pointerData.pointerCurrentRaycast.worldPosition;
            bool isInteractive = this.pointerData.pointerPress != null ||
                                 ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentGazeObject) != null;

            // Hack here,use remote to control pointer
            if (LaserPointer.Instance != null)
            {
                if (currentGazeObject == previousGazedObject)
                {
                    if (currentGazeObject != null)
                    {
                        LaserPointer.Instance.OnPointerHover(currentGazeObject, intersectionPosition, this.GetLastRay(), true);
                    }
                }
                else
                {
                    if (previousGazedObject != null)
                    {
                        LaserPointer.Instance.OnPointerExit(previousGazedObject);
                    }

                    if (currentGazeObject != null)
                    {
                        LaserPointer.Instance.OnPointerEnter(currentGazeObject, intersectionPosition, this.GetLastRay(), isInteractive);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the pending click.
        /// </summary>
        private void HandlePendingClick()
        {
            if (!this.pointerData.eligibleForClick)
            {
                return;
            }

            var hitObject = this.pointerData.pointerCurrentRaycast.gameObject;

            ExecuteEvents.Execute(this.pointerData.pointerPress, this.pointerData, ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute(this.pointerData.pointerPress, this.pointerData, ExecuteEvents.pointerClickHandler);

            this.pointerData.pointerPress = null;
            this.pointerData.rawPointerPress = null;
            this.pointerData.eligibleForClick = false;
            this.pointerData.clickCount = 0;
        }

        /// <summary>
        /// Handles the trigger.
        /// </summary>
        private void HandleTrigger()
        {
            var triggerObject = this.pointerData.pointerCurrentRaycast.gameObject;

            this.pointerData.pressPosition = this.pointerData.position;
            this.pointerData.pointerPressRaycast = this.pointerData.pointerCurrentRaycast;
            this.pointerData.pointerPress = ExecuteEvents.ExecuteHierarchy(triggerObject, this.pointerData, ExecuteEvents.pointerDownHandler)
                ?? ExecuteEvents.GetEventHandler<IPointerClickHandler>(triggerObject);

            this.pointerData.rawPointerPress = triggerObject;
            this.pointerData.eligibleForClick = true;
            this.pointerData.delta = Vector2.zero;
            this.pointerData.dragging = false;
            this.pointerData.useDragThreshold = true;
            this.pointerData.clickCount = 1;
            this.pointerData.clickTime = Time.unscaledTime;
        }

        /// <summary>
        /// Gets the current game object.
        /// </summary>
        /// <returns>the current gazed game object</returns>
        private GameObject GetCurrentGameObject()
        {
            if (this.pointerData != null && this.pointerData.enterEventCamera != null)
            {
                return this.pointerData.pointerCurrentRaycast.gameObject;
            }

            return null;
        }

        /// <summary>
        /// Get the position that intersection happens.
        /// </summary>
        /// <param name="cam">The camera.</param>
        /// <param name="raycastResult">The raycast result</param>
        /// <returns>The position.</returns>
        private Vector3 GetIntersectionPosition(Camera cam, RaycastResult raycastResult)
        {
            // Check for camera
            if (cam == null)
            {
                return Vector3.zero;
            }

            float intersectionDistance = raycastResult.distance + cam.nearClipPlane;
            Vector3 intersectionPosition = cam.transform.position + (cam.transform.forward * intersectionDistance);
            return intersectionPosition;
        }
    }
}