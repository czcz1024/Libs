namespace THZ.App.Template.Auth
{
    using Uninf.Auth;

    public class THZUserLoginSerializer:IAuthInfoSerializer<THZUserLogin>
    {
        public string Serialize(THZUserLogin obj)
        {
            return obj.Id.ToString();
        }

        public THZUserLogin Deserialize(string src)
        {
            return new THZUserLogin { 
                                        Id=int.Parse(src.Split(':')[0])
                                    };
        }
    }
}