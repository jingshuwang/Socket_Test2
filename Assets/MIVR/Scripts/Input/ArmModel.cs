//-----------------------------------------------------------------------
// <copyright file="ArmModel.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MIVR
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Arm model.
    /// </summary>
    public class ArmModel
    {
        /// <summary>
        /// The K delta alpha.
        /// </summary>
        private const float KDeltaAlpha = 4.0f;

        /// <summary>
        /// Angle ranges the for arm extension offset to start and end (degrees).
        /// </summary>
        private const float KMinExtensionAngle = 7.0f;

        /// <summary>
        /// Angle ranges the for arm extension offset to start and end (degrees).
        /// </summary>
        private const float KMaxExtensionAngle = 60.0f;

        /// <summary>
        /// Increases elbow bending as the controller moves up (unitless).
        /// </summary>
        private const float KExtensionWeight = 0.4f;

        /// <summary>
        /// Initial relative location of the shoulder (meters).
        /// </summary>
        private static readonly Vector3 KDefaultShoulderRight = new Vector3(0.19f, -0.19f, -0.03f);

        /// <summary>
        /// Offset of the laser pointer origin relative to the wrist (meters)
        /// </summary>
        private static readonly Vector3 KPointerOffset = new Vector3(0.0f, 0.003f, 0.11f);

        /// <summary>
        /// The K elbow position.
        /// </summary>
        private static readonly Vector3 KElbowPosition = new Vector3(0.195f, -0.43f, 0.2f); // -0.5f, -0.075f 0.125f

        /// <summary>
        /// The K wrist position.
        /// </summary>
        private static readonly Vector3 KWristPosition = new Vector3(0.0f, 0.0f, 0.25f);

        /// <summary>
        /// The K arm extension offset.
        /// </summary>
        private static readonly Vector3 KArmExtensionOffset = new Vector3(-0.13f, 0.14f, 0.08f);

        /// <summary>
        /// The instance.
        /// </summary>
        private static ArmModel instance = null;

        /// <summary>
        /// Offset of the elbow due to the accelerometer
        /// </summary>
        private Vector3 elbowOffset;

        /// <summary>
        /// Forward direction of the arm model.
        /// </summary>
        private Vector3 torsoDirection;

        /// <summary>
        /// Indicates if this is the first frame to receive new IMU measurements.
        /// </summary>
        private bool firstUpdate;

        /// <summary>
        /// Multiplier for handedness such that 1 = Right, 0 = Center, -1 = left.
        /// </summary>
        private Vector3 handedMultiplier;

        /// <summary>
        /// The last controller quat.
        /// </summary>
        private Quaternion lastControllerQuat;

        /// <summary>
        /// The camera object
        /// </summary>
        private GameObject cameraObject = null;

        /// <summary>
        /// Represents when gaze-following behavior should occur.
        /// </summary>
        public enum GazeBehavior
        {
            /// <summary>
            /// The shoulder will never follow the gaze.
            /// </summary>
            Never,

            /// <summary>
            /// The shoulder will follow the gaze during controller motion.
            /// </summary>
            DuringMotion,

            /// <summary>
            /// The shoulder will always follow the gaze.
            /// </summary>
            Always
        }

        /// <summary>
        /// Handedness of user.
        /// </summary>
        public enum Handedness
        {
            /// <summary>
            /// The rignt handed.
            /// </summary>
            RigntHanded,

            /// <summary>
            /// The left handed.
            /// </summary>
            LeftHanded
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ArmModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ArmModel();
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets the Height of the elbow  (m).
        /// </summary>
        /// [Range(0.0f, 0.2f)]
        public float AddedElbowHeight { get; private set; }

        /// <summary>
        /// Gets the added elbow depth.
        /// </summary>
        /// <value>The added elbow depth.</value>
        /// [Range(0.0f, 0.2f)]
        public float AddedElbowDepth { get; private set; }

        /// <summary>
        /// Gets the pointer tilt angle.
        /// </summary>
        /// <value> Downward tilt of the laser pointer relative to the controller (degrees).</value>
        /// [Range(0.0f, 30.0f)]
        public float PointerTiltAngle { get; private set; }

        /// Controller distance from the face after which the alpha value decreases (meters).
        /// <summary>
        /// Gets the fade distance from face.
        /// </summary>
        /// <value>Controller distance from the face after which the alpha value decreases (meters).</value>
        /// [Range(0.0f, 0.4f)]
        public float FadeDistanceFromFace { get; private set; }

        /// <summary>
        /// Gets the handedness.
        /// </summary>
        /// <value>The handedness.</value>
        public Handedness MyHandedness { get; private set; }

        /// <summary>
        /// Gets the follow gaze.
        /// </summary>
        /// <value>Determines if the shoulder should follow the gaze. </value>
        public GazeBehavior FollowGaze { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ArmModel"/> use accelerometer.
        /// </summary>
        /// <value><c>true</c> if use accelerometer; otherwise, <c>false</c>.</value>
        public bool UseAccelerometer { get; private set; }

        /// <summary>
        /// Gets Vector to represent the pointer's location.
        /// NOTE: This is in skeleton coordinates where MiCamera stands at (0,0,0).
        /// </summary>
        /// <value>The pointer position.</value>
        public Vector3 PointerPosition { get; private set; }

        /// <summary>
        /// Gets Vector to represent the pointer's location.
        /// NOTE: This is in skeleton coordinates where MiCamera stands at (0,0,0).
        /// </summary>
        /// <value>The pointer rotation.</value>
        public Quaternion PointerRotation { get; private set; }

        /// <summary>
        /// Gets the wrist position.
        /// NOTE: This is in skeleton coordinates where MiCamera stands at (0,0,0).
        /// </summary>
        /// <value>The wrist position.</value>
        public Vector3 WristPosition { get; private set; }

        /// <summary>
        /// Gets the wrist rotation.
        /// NOTE: This is in skeleton coordinates where MiCamera stands at (0,0,0).
        /// </summary>
        /// <value>The wrist rotation.</value>
        public Quaternion WristRotation { get; private set; }

        /// <summary>
        /// Gets the elbow position.
        /// NOTE: This is in skeleton coordinates where MiCamera stands at (0,0,0).
        /// </summary>
        /// <value>The elbow position.</value>
        public Vector3 ElbowPosition { get; private set; }

        /// <summary>
        /// Gets the elbow rotation.
        /// NOTE: This is in skeleton coordinates where MiCamera stands at (0,0,0).
        /// </summary>
        /// <value>The elbow rotation.</value>
        public Quaternion ElbowRotation { get; private set; }

        /// <summary>
        /// Gets the shoulder position.
        /// NOTE: This is in skeleton coordinates where MiCamera stands at (0,0,0).
        /// </summary>
        /// <value>The shoulder position.</value>
        public Vector3 ShoulderPosition { get; private set; }

        /// <summary>
        /// Gets the shoulder rotation.
        /// NOTE: This is in skeleton coordinates where MiCamera stands at (0,0,0).
        /// </summary>
        /// <value>The shoulder rotation.</value>
        public Quaternion ShoulderRotation { get; private set; }

        /// <summary>
        /// Gets the alpha value.
        /// The suggested rendering alpha value of the controller.
        /// This is to prevent the controller from intersecting face.
        /// </summary>
        /// <value>The alpha value.</value>
        public float AlphaValue { get; private set; }

        /// <summary>
        /// Raises the controller update event.
        /// </summary>
        public void OnControllerUpdate()
        {
            this.UpdateHandedness();
            this.UpdateTorsoDirection();
            ControllerState state = InputManager.ControllerState;
            if (state.ConnectionState == ConnectionState.Connected)
            {
                this.UpdateFromController();
            }
            else
            {
                this.ResetState();
            }

            this.elbowOffset = Vector3.zero;
            this.ApplyArmModel();
            this.UpdateTransparency();
            this.UpdatePointer();
            this.lastControllerQuat = InputManager.ControllerState.Orientation;
        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        public void OnStart()
        {
            this.UpdateHandedness();

            // Reset other relevant state.
            this.firstUpdate = true;
            this.elbowOffset = Vector3.zero;
            this.AlphaValue = 1.0f;
            this.lastControllerQuat = new Quaternion();
            this.AddedElbowHeight = 0.0f;
            this.AddedElbowDepth = 0.0f;
            this.PointerTiltAngle = 10.0f;
            this.FadeDistanceFromFace = 0.32f;
            this.MyHandedness = Handedness.RigntHanded;
            this.FollowGaze = GazeBehavior.DuringMotion;
            this.UseAccelerometer = false;
            this.cameraObject = Camera.main.gameObject;
        }

        /// <summary>
        /// Updates the handedness.
        /// </summary>
        private void UpdateHandedness()
        {
            // Determine handedness multiplier.
            this.handedMultiplier.Set(0, 1, 1);
            if (this.MyHandedness == Handedness.RigntHanded)
            {
                this.handedMultiplier.x = 1.0f;
            }
            else if (this.MyHandedness == Handedness.LeftHanded)
            {
                this.handedMultiplier.x = -1.0f;
            }

            // Place the shoulder in anatomical positions based on the height and handedness.
            this.ShoulderRotation = Quaternion.identity;
            this.ShoulderPosition = Vector3.Scale(KDefaultShoulderRight, this.handedMultiplier);
        }

        /// <summary>
        /// Gets the head orientation.
        /// </summary>
        /// <returns>The head orientation.</returns>
        private Vector3 GetHeadOrientation()
        {
            return this.cameraObject.transform.rotation * Vector3.forward;
        }

        /// <summary>
        /// Updates the torso direction.
        /// </summary>
        private void UpdateTorsoDirection()
        {
            // Ignore updates here if requested.
            if (this.FollowGaze == GazeBehavior.Never)
            {
                return;
            }

            // Determine the gaze direction horizontally.
            Vector3 gazeDirection = this.GetHeadOrientation();
            gazeDirection.y = 0.0f;
            gazeDirection.Normalize();

            // Use the gaze direction to update the forward direction.
            if (this.FollowGaze == GazeBehavior.Always)
            {
                this.torsoDirection = gazeDirection;
            }
            else if (this.FollowGaze == GazeBehavior.DuringMotion)
            {
                Quaternion rotationQuat = Quaternion.Inverse(this.lastControllerQuat) * InputManager.ControllerState.Orientation;
                float rotationDegree;
                Vector3 rotationAxis = new Vector3();
                rotationQuat.ToAngleAxis(out rotationDegree, out rotationAxis);
                float angularVelocity = Mathf.Deg2Rad * rotationDegree / Time.deltaTime;
                float gazeFilterStrength = Mathf.Clamp((angularVelocity - 0.2f) / 45.0f, 0.0f, 0.1f);
                this.torsoDirection = Vector3.Slerp(this.torsoDirection, gazeDirection, gazeFilterStrength);
            }

            // Rotate the fixed joints.
            Quaternion gazeRotation = Quaternion.FromToRotation(Vector3.forward, this.torsoDirection);
            this.ShoulderRotation = gazeRotation;
            this.ShoulderPosition = gazeRotation * this.ShoulderPosition;
        }

        /// <summary>
        /// Updates from controller.
        /// </summary>
        private void UpdateFromController()
        {
            if (this.firstUpdate)
            {
                this.firstUpdate = false;
            }
        }

        /// <summary>
        /// Resets the state.
        /// </summary>
        private void ResetState()
        {
            // We've lost contact, quickly reset the state.
            this.firstUpdate = true;
        }

        /// <summary>
        /// Applies the arm model.
        /// </summary>
        private void ApplyArmModel()
        {
            // Find the controller's orientation relative to the player
            Quaternion controllerOrientation = InputManager.ControllerState.Orientation;
            controllerOrientation = Quaternion.Inverse(this.ShoulderRotation) * controllerOrientation;

            // Get the relative positions of the joints
            this.ElbowPosition = KElbowPosition + new Vector3(0.0f, this.AddedElbowHeight, this.AddedElbowDepth);
            this.ElbowPosition = Vector3.Scale(this.ElbowPosition, this.handedMultiplier) + this.elbowOffset;
            this.WristPosition = Vector3.Scale(KWristPosition, this.handedMultiplier);
            Vector3 armExtensionOffset = Vector3.Scale(KArmExtensionOffset, this.handedMultiplier);

            // Extract just the x rotation angle
            Vector3 controllerForward = controllerOrientation * Vector3.forward;
            float xrotationAngle = 90.0f - Vector3.Angle(controllerForward, Vector3.up);

            // Remove the z rotation from the controller
            Quaternion xyplaneRotation = Quaternion.FromToRotation(Vector3.forward, controllerForward);

            // Offset the elbow by the extension
            float normalizedAngle = (xrotationAngle - KMinExtensionAngle) / (KMaxExtensionAngle - KMinExtensionAngle);
            float extensionRatio = Mathf.Clamp(normalizedAngle, 0.0f, 1.0f);
            if (!this.UseAccelerometer)
            {
                this.ElbowPosition += armExtensionOffset * extensionRatio;
            }

            // Calculate the lerp interpolation factor
            float totalAngle = Quaternion.Angle(xyplaneRotation, Quaternion.identity);
            float lerpSuppresion = 1.0f - Mathf.Pow(totalAngle / 180.0f, 6);
            float lerpValue = lerpSuppresion * (0.4f + (0.6f * extensionRatio * KExtensionWeight));

            // Apply the absolute rotations to the joints
            Quaternion lerpRotation = Quaternion.Lerp(Quaternion.identity, xyplaneRotation, lerpValue);
            this.ElbowRotation = this.ShoulderRotation * Quaternion.Inverse(lerpRotation) * controllerOrientation;
            this.WristRotation = this.ShoulderRotation * controllerOrientation;

            // Determine the relative positions
            this.ElbowPosition = this.ShoulderRotation * this.ElbowPosition;
            this.WristPosition = this.ElbowPosition + (this.ElbowRotation * this.WristPosition);
        }

        /// <summary>
        /// Updates the transparency.
        /// </summary>
        private void UpdateTransparency()
        {
            // Determine how vertical the controller is pointing.
            float distToFace = Vector3.Distance(this.WristPosition, Vector3.zero);
            if (distToFace < this.FadeDistanceFromFace)
            {
                this.AlphaValue = Mathf.Max(0.0f, this.AlphaValue - (KDeltaAlpha * Time.deltaTime));
            }
            else
            {
                this.AlphaValue = Mathf.Min(1.0f, this.AlphaValue + (KDeltaAlpha * Time.deltaTime));
            }
        }

        /// <summary>
        /// Updates the pointer.
        /// </summary>
        private void UpdatePointer()
        {
            // Determine the direction of the ray.
            this.PointerPosition = this.WristPosition + (this.WristRotation * KPointerOffset);
            this.PointerRotation = this.WristRotation * Quaternion.AngleAxis(this.PointerTiltAngle, Vector3.right);
        }
    }
}