using System.Collections.Generic;
using XiaoQingWa_Work_Model.Entity;

namespace XiaoQingWa_Work_IDAL
{
    public interface IUserInfoRepository : IDependency<UserInfoEntity>
    {
        UserInfoEntity GetUserInfo(string userName);
        bool DelUserInfo(int id);
        bool DelUserInfoBatch(int[] ids);
        bool UpdateUserStatu(int id, int statu);
        bool UpdateUserPassWord(int id, string password);
        List<UserInfoEntity> GetUserInfoListByQueryModel(UserQuery userQuery);
     
    }
}
