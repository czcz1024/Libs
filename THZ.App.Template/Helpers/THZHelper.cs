namespace THZ.App.Template.Helpers
{
    using System.Collections.Generic;

    using THZ.App.Template.Interfaces;
    using THZ.App.Template.Models.ServiceModel;

    public class THZHelper:ITHZHelper
    {
        public THZUserInfo UserInfo(int uid)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<THZUserInfo> UsersInfo(params int[] uids)
        {
            throw new System.NotImplementedException();
        }

        public THZCompanyInfo CompanyInfo(int cid)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<THZCompanyInfo> CompaniesInfo(params int[] cids)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<int> UserFriends(int uid)
        {
            if (uid == 2217) return new[] { 2216,2215 };
            else if (uid == 2216) return new[] { 2217 };
            return new[] { 0 };
        }

        public IEnumerable<int> UserFollowUsers(int uid)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<int> UserFans(int uid)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<int> UserFollowCompanies(int uid)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<int> UserOwnerCompanies(int uid)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<int> UserManageCompanies(int uid)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<int> UserJoinCompanies(int uid)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<int> UserJoinCompaniesHrm(int uid)
        {
            throw new System.NotImplementedException();
        }
    }
}