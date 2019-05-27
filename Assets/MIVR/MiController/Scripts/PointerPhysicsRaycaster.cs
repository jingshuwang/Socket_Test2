//-----------------------------------------------------------------------
// <copyright file="PointerPhysicsRaycaster.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed.")]
    public class PointerPhysicsRaycaster : BasePointerRaycaster
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
        protected const int NO_EVENT_MASK_SET = -1;

        [SerializeField]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        protected LayerMask raycasterEventMask = NO_EVENT_MASK_SET;

        private Camera cachedEventCamera;

        protected PointerPhysicsRaycaster()
        {
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        public override Camera eventCamera
        {
            get
            {
                return Camera.main;
            }
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        public int finalEventMask
        {
            get
            {
                return (this.eventCamera != null) ? this.eventCamera.cullingMask & this.eventMask : NO_EVENT_MASK_SET;
            }
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        public LayerMask eventMask
        {
            get
            {
                return this.raycasterEventMask;
            }

            set
            {
                this.raycasterEventMask = value;
            }
        }

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            if (this.eventCamera == null)
            {
                return;
            }

            if (!this.IsPointerAvailable())
            {
                return;
            }

            Ray ray = this.GetRay();
            float dist = MaxPointerDistance;
            float radius = PointerRadius;
            RaycastHit[] hits;

            if (radius > 0.0f)
            {
                hits = Physics.SphereCastAll(ray, radius, dist, this.finalEventMask);
            }
            else
            {
                hits = Physics.RaycastAll(ray, dist, this.finalEventMask);
            }

            if (hits.Length == 0)
            {
                return;
            }

            System.Array.Sort(hits, (r1, r2) => r1.distance.CompareTo(r2.distance));

            for (int b = 0, bmax = hits.Length; b < bmax; ++b)
            {
                Vector3 projection = Vector3.Project(hits[b].point - ray.origin, ray.direction);
                Vector3 hitPosition = projection + ray.origin;

                RaycastResult result = new RaycastResult
                {
                    gameObject = hits[b].collider.gameObject,
                    module = this,
                    distance = hits[b].distance,
                    worldPosition = hitPosition,
                    worldNormal = hits[b].normal,
                    screenPosition = eventData.position,
                    index = resultAppendList.Count,
                    sortingLayer = 0,
                    sortingOrder = 0
                };

                resultAppendList.Add(result);
            }
        }
    }
}