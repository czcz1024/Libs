namespace THZ.App.Template.Interfaces
{
    using System.Collections.Generic;

    using THZ.App.Template.Models.ServiceModel;

    public interface ITHZHelper
    {
        THZUserInfo UserInfo(int uid);

        IEnumerable<THZUserInfo> UsersInfo(params int[] uids);

        THZCompanyInfo CompanyInfo(int cid);

        IEnumerable<THZCompanyInfo> CompaniesInfo(params int[] cids);

        IEnumerable<int> UserFriends(int uid);

        IEnumerable<int> UserFollowUsers(int uid);

        IEnumerable<int> UserFans(int uid);



        IEnumerable<int> UserFollowCompanies(int uid);

        IEnumerable<int> UserOwnerCompanies(int uid);

        IEnumerable<int> UserManageCompanies(int uid);

        IEnumerable<int> UserJoinCompanies(int uid);
        IEnumerable<int> UserJoinCompaniesHrm(int uid);
    }
}