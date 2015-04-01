namespace THZ.App.Template.Auth
{
    using Uninf.Auth;

    public class THZAuthOperator:AuthOperator<THZUserLogin>
    {
        public THZAuthOperator(IAuthInfoEncryptor encryptor, IAuthInfoSerializer<THZUserLogin> serializer, IAuthInfoStorage storage)
            : base(encryptor, serializer, storage)
        {
        }

        public THZAuthOperator(IAuthInfoSerializer<THZUserLogin> serializer)
            : base(serializer)
        {
        }

        public override string StorageName()
        {
            return ".ASPXAUTH";
        }
    }
}