using System.Collections.Generic;

using YH.Security.Entity;
using YH.Framework.ControlUtil;

namespace YH.Security.BLL
{
    /// <summary>
    /// 字段的列表权限
    /// </summary>
	public class FieldPermit : BaseBLL<FieldPermitInfo>
    {
        public FieldPermit() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 获取指定实体类名的字段列权限集合
        /// </summary>
        /// <param name="entityFullName">实体类名，如YH.Security.Entity.LoginLogInfo</param>
        /// <param name="userId">当前用户ID</param>
        /// <returns></returns>
        public Dictionary<string,int> GetColumnsPermit(string entityFullName, int userId)
        {
            //获取用户所属的角色
            var strRoleIdList = "-1";
            var roleList = BLLFactory<Role>.Instance.GetRolesByUser(userId);
            if (roleList != null)
            {
                roleList.ForEach(s => strRoleIdList += string.Concat(",", s.ID));
            }

            Dictionary<string, int> dict = new Dictionary<string, int>();
            string condition = string.Format("EntityFullName='{0}' AND Role_ID in ({1})", entityFullName, strRoleIdList);
            var list = this.Find(condition);
            if(list != null)
            {
                foreach(var info in list)
                {                    
                    if(!dict.ContainsKey(info.FiledCode))
                    {
                        dict.Add(info.FiledCode, info.Permit);
                    }
                    else
                    {
                        //如果已存在，则判断权限值最小的优先
                        //0可读写，1只读，2隐藏值，3不显示
                        var oldPermit = dict[info.FiledCode];
                        if(info.Permit < oldPermit)
                        {
                            dict[info.FiledCode] = info.Permit;
                        }
                    }
                }
            }
            return dict;
        }
    }
}
