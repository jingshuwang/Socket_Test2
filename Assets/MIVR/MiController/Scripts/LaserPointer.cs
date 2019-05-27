//-----------------------------------------------------------------------
// <copyright file="LaserPointer.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    /// <summary>
    /// Laser pointer.
    /// </summary>
    public class LaserPointer : MonoBehaviour
    {
        /// <summary>
        /// Maximum distance of the pointer (meters).
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        [Range(0.0f, 20.0f)]
        public float MaxLaserDistance = 10f;

        /// <summary>
        /// Maximum distance of the reticle (meters).
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        [Range(0.4f, 20.0f)]
        public float MaxReticleDistance = 10f;

        /// <summary>
        /// the reticle used at the end of the ray
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public GameObject Reticle;

        /// <summary>
        /// the line used at the end of the ray
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public GameObject Line;

        /// <summary>
        /// The line renderer.
        /// </summary>
        private LineRenderer lineRenderer;

        /// <summary>
        /// Whether the pointer intersects something.
        /// </summary>
        private bool isPointerIntersecting;

        /// <summary>
        /// The position where pointer intersect with.
        /// </summary>
        private Vector3 pointerIntersection;

        /// <summary>
        /// The ray used to cast from the remote.
        /// </summary>
        private Ray pointerIntersectionRay;

        /// <summary>
        /// Gets or sets the laser point used
        /// </summary>
        public static LaserPointer Instance { get; set; }

        /// <summary>
        /// Gets the line renderer.
        /// </summary>
        public LineRenderer LaserLineRenderer
        {
            get { return this.lineRenderer; }
        }

        /// <summary>
        /// Gets a value indicating whether IsPointerIntersecting.
        /// </summary>
        public bool IsPointerIntersecting
        {
            get { return this.IsPointerIntersecting; }
        }

        /// <summary>
        /// Gets pointerIntersection.
        /// </summary>
        public Vector3 PointerIntersection
        {
            get { return this.pointerIntersection; }
        }

        /// <summary>
        /// OnPointerEnter event happens.
        /// </summary>
        /// <param name="targetObject">The target object.</param>
        /// <param name="intersectionPosition">The intersect position.</param>
        /// <param name="intersectionRay">The intersect ray.</param>
        /// <param name="isInteractive">Is the game object interactive.</param>
        public void OnPointerEnter(GameObject targetObject, Vector3 intersectionPosition, Ray intersectionRay, bool isInteractive)
        {
            this.pointerIntersection = intersectionPosition;
            this.pointerIntersectionRay = intersectionRay;
            this.isPointerIntersecting = true;
            this.Reticle.GetComponent<ReticleBehaviour>().PointerEnter = true;
        }

        /// <summary>
        /// OnPointerHover event happens.
        /// </summary>
        /// <param name="targetObject">The target object.</param>
        /// <param name="intersectionPosition">The intersect position.</param>
        /// <param name="intersectionRay">The intersection ray.</param>
        /// <param name="isInteractive">Is interactive.</param>
        public void OnPointerHover(GameObject targetObject, Vector3 intersectionPosition, Ray intersectionRay, bool isInteractive)
        {
            this.pointerIntersection = intersectionPosition;
            this.pointerIntersectionRay = intersectionRay;
        }

        /// <summary>
        /// OnPointerExit event happens.
        /// </summary>
        /// <param name="targetObject">The target object.</param>
        public void OnPointerExit(GameObject targetObject)
        {
            this.pointerIntersection = Vector3.zero;
            this.pointerIntersectionRay = new Ray();
            this.isPointerIntersecting = false;
            this.Reticle.GetComponent<ReticleBehaviour>().PointerEnter = false;
        }

        /// <summary>
        /// The max pointer distance.
        /// </summary>
        /// <returns>The value of max distance.</returns>
        public float GetMaxPointerDistance()
        {
            return this.MaxReticleDistance;
        }

        /// <summary>
        /// Awake this instance.
        /// </summary>
        private void Awake()
        {
            Instance = this;
            if (this.Line != null)
            {
                this.lineRenderer = this.Line.GetComponent<LineRenderer>();
            }
        }

        /// <summary>
        /// Start this.
        /// </summary>
        private void Start()
        {
            if (this.Reticle != null)
            {
                this.Reticle.transform.position = this.transform.position + (this.transform.forward * this.MaxLaserDistance);
            }

            this.lineRenderer.SetPosition(1, this.transform.position + (this.transform.forward * this.MaxLaserDistance));
        }

        /// <summary>
        /// Lates the update.
        /// </summary>
        private void LateUpdate()
        {
            this.lineRenderer.SetPosition(0, transform.position);
            Vector3 lineEndPoint = this.transform.position + (this.transform.forward * this.MaxLaserDistance);

            if (this.isPointerIntersecting && Vector3.Distance(transform.position, this.pointerIntersection) < this.MaxLaserDistance)
            {
                this.Reticle.transform.position = this.pointerIntersection;
                lineEndPoint = this.pointerIntersection;
            }
            else
            {
                this.Reticle.transform.position = transform.position + (transform.forward * this.MaxLaserDistance);
                lineEndPoint = transform.position + (transform.forward * this.MaxLaserDistance);
            }

            this.lineRenderer.SetPosition(1, lineEndPoint);
        }
    }
}