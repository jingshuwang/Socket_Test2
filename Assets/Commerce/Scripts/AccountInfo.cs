//-----------------------------------------------------------------------
// <copyright file="AccountInfo.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MIVR
{
    using UnityEngine;

    /// <summary>
    /// Account info
    /// </summary>
    public class AccountInfo
    {
        /// <summary>
        /// Gets or sets the open identifier.
        /// </summary>
        /// <value>
        /// The open identifier.
        /// </value>
        public string OpenId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public string SessionId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the nick.
        /// </summary>
        /// <value>
        /// The name of the nick.
        /// </value>
        public string NickName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the avatar.
        /// </summary>
        /// <value>
        /// The avatar.
        /// </value>
        public string Avatar
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public string Gender
        {
            get;
            set;
        }
    }
}
