//-----------------------------------------------------------------------
// <copyright file="LoginFinishedEventArgs.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System;

    /// <summary>
    /// The login finished Event args
    /// </summary>
    public class LoginFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// the error code
        /// </value>
        public ErrorCode Code
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public AccountInfo Account
        {
            get;
            set;
        }
    }
}