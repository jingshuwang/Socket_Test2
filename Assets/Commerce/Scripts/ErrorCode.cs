//-----------------------------------------------------------------------
// <copyright file="ErrorCode.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    /// <summary>
    /// Error code for commerce
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// The success
        /// </summary>
        Success,

        /// <summary>
        /// The unknown error
        /// </summary>
        UnknownError,

        /// <summary>
        /// The invalid parameter error
        /// </summary>
        InvalidParameter,

        /// <summary>
        /// Launcher not login
        /// </summary>
        NotLogin,

        /// <summary>
        /// The login error
        /// </summary>
        LoginError,

        /// <summary>
        /// No specific launcher to provide the service
        /// 建议提示当前版本不支持付款，请升级小米VR客户端
        /// </summary>
        NoLauncher,

        /// <summary>
        /// The create bill failed
        /// </summary>
        CreateBillFailed,

        /// <summary>
        /// Pay canceled by user
        /// </summary>
        CanceledByUser,

        /// <summary>
        /// The pay error
        /// </summary>
        PayError
    }
}