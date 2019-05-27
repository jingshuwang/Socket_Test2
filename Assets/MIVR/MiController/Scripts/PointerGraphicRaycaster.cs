//-----------------------------------------------------------------------
// <copyright file="PointerGraphicRaycaster.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    [RequireComponent(typeof(Canvas))]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed.")]
    public class PointerGraphicRaycaster : BasePointerRaycaster
    {
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public bool IgnoreReversedGraphics = true;
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public BlockingObjects BlockingObjectType = BlockingObjects.None;
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public LayerMask BlockingMask = NoEventMaskSet;

        private const int NoEventMaskSet = -1;
        private static readonly List<Graphic> SortedGraphics = new List<Graphic>();

        private Canvas targetCanvas;
        private List<Graphic> raycastResults = new List<Graphic>();
        private Camera cachedPointerEventCamera = null;

        protected PointerGraphicRaycaster()
        {
        }

        /// <summary>
        /// Blocking Objects
        /// </summary>
        public enum BlockingObjects
        {
            [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed.")]
            None = 0,
            [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed.")]
            TwoD = 1,
            [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed.")]
            ThreeD = 2,
            [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed.")]
            All = 3,
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        public override Camera eventCamera
        {
            get { return Camera.main; }
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        private Canvas canvas
        {
            get
            {
                if (this.targetCanvas != null)
                {
                    return this.targetCanvas;
                }

                this.targetCanvas = this.GetComponent<Canvas>();
                return this.targetCanvas;
            }
        }

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            if (this.canvas == null)
            {
                return;
            }

            if (this.eventCamera == null)
            {
                return;
            }

            if (!this.IsPointerAvailable())
            {
                return;
            }

            if (this.canvas.renderMode != RenderMode.WorldSpace)
            {
                Debug.LogError("PointerGraphicRaycaster requires that the canvase renderMode is set to WorldSpace.");
                return;
            }

            if (this.cachedPointerEventCamera == null && LaserPointer.Instance != null)
            {
                this.cachedPointerEventCamera = LaserPointer.Instance.gameObject.GetComponent<Camera>();
            }

            Ray ray = GetRay();
            float hitDistance = float.MaxValue;

            if (this.BlockingObjectType != BlockingObjects.None)
            {
                float dist = this.eventCamera.farClipPlane - this.eventCamera.nearClipPlane;

                if (this.BlockingObjectType == BlockingObjects.ThreeD || this.BlockingObjectType == BlockingObjects.All)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, dist, this.BlockingMask))
                    {
                        hitDistance = hit.distance;
                    }
                }

                if (this.BlockingObjectType == BlockingObjects.TwoD || this.BlockingObjectType == BlockingObjects.All)
                {
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, dist, this.BlockingMask);

                    if (hit.collider != null)
                    {
                        hitDistance = hit.fraction * dist;
                    }
                }
            }

            this.raycastResults.Clear();
            Ray finalRay;

            if (this.cachedPointerEventCamera != null)
            {
                Raycast(this.canvas, ray, this.cachedPointerEventCamera, this.MaxPointerDistance, this.raycastResults, out finalRay); // this.eventCamera
            }
            else
            {
                Raycast(this.canvas, ray, this.eventCamera, this.MaxPointerDistance, this.raycastResults, out finalRay);
            }

            for (int index = 0; index < this.raycastResults.Count; index++)
            {
                GameObject go = this.raycastResults[index].gameObject;

                bool appendGraphic = true;

                if (this.IgnoreReversedGraphics)
                {
                    // If we have a camera compare the direction against the cameras forward.
                    Vector3 cameraFoward = Vector3.zero;

                    if (this.cachedPointerEventCamera != null)
                    {
                        cameraFoward = this.cachedPointerEventCamera.transform.rotation * Vector3.forward;
                    }
                    else
                    {
                        cameraFoward = this.eventCamera.transform.rotation * Vector3.forward;
                    }

                    Vector3 dir = go.transform.rotation * Vector3.forward;
                    appendGraphic = Vector3.Dot(cameraFoward, dir) > 0;
                }

                if (appendGraphic)
                {
                    float distance = 0;

                    Transform trans = go.transform;
                    Vector3 transForward = trans.forward;

                    distance = Vector3.Dot(transForward, trans.position - finalRay.origin) / Vector3.Dot(transForward, finalRay.direction);

                    if (distance < 0)
                    {
                        continue;
                    }

                    if (distance >= hitDistance)
                    {
                        continue;
                    }

                    RaycastResult castResult = new RaycastResult
                    {
                        gameObject = go,
                        module = this,
                        distance = distance,
                        worldPosition = finalRay.origin + (finalRay.direction * distance),
                        screenPosition = this.eventCamera.WorldToScreenPoint(finalRay.origin + (finalRay.direction * distance)),
                        index = resultAppendList.Count,
                        depth = this.raycastResults[index].depth,
                        sortingLayer = this.canvas.sortingLayerID,
                        sortingOrder = this.canvas.sortingOrder
                    };
                    resultAppendList.Add(castResult);
                }
            }
        }

        private static void Raycast(Canvas canvas, Ray ray, Camera cam, float maxPointerDistance, List<Graphic> results, out Ray finalRay)
        {
            Vector3 screenPoint = cam.WorldToScreenPoint(ray.GetPoint(maxPointerDistance));
            finalRay = cam.ScreenPointToRay(screenPoint);

            IList<Graphic> foundGraphics = GraphicRegistry.GetGraphicsForCanvas(canvas);
            for (int i = 0; i < foundGraphics.Count; ++i)
            {
                Graphic graphic = foundGraphics[i];

                if (graphic.depth == -1 || !graphic.raycastTarget)
                {
                    continue;
                }

                if (!RectTransformUtility.RectangleContainsScreenPoint(graphic.rectTransform, screenPoint, cam))
                {
                    continue;
                }

                if (graphic.Raycast(screenPoint, cam))
                {
                    SortedGraphics.Add(graphic);
                }
            }

            SortedGraphics.Sort((g1, g2) => g2.depth.CompareTo(g1.depth));

            for (int i = 0; i < SortedGraphics.Count; ++i)
            {
                results.Add(SortedGraphics[i]);
            }

            SortedGraphics.Clear();
        }
    }
}