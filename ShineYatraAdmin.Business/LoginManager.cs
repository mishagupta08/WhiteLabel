namespace ShineYatraAdmin.Business
{
    #region namespace

    using Entity;
    using Repository;
    using System.Threading.Tasks;

    #endregion namespace

    /// <summary>
    /// Holds Login Manager
    /// </summary>
    public class LoginManager
    {
        /// <summary>
        /// method to validate user
        /// </summary>
        /// <param name="loginDetail"></param>
        /// <returns></returns>
        public async Task<UserDetail> ValidateUser(LoginModel loginDetail)
        {
            return await Program.ValidateUser(loginDetail);
        }
    }
}
