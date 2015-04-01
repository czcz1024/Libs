namespace THZ.App.Template.Auth
{
    using Uninf.Auth;

    public class THZCookieStorage : THZCookieAuthInfoStorage
    {
        public THZCookieStorage()
        {
            this.domain = ".tuohuangzu.com";
        }
    }
}