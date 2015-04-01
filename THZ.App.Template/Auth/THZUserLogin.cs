namespace THZ.App.Template.Auth
{
    using Uninf.Auth;

    public class THZUserLogin:IAuthInfo
    {
        public int Id { get; set; }

        public string AuthRole()
        {
            return "User";
        }
    }
}