using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using WHC.WorkflowLite.Entity;

namespace WHC.WorkflowLite.IDAL
{
	/// <summary>
	/// IAppFlow 的摘要说明。
	/// </summary>
	public interface IFormFlow : YH.Framework.ControlUtil.IBaseDAL<FormFlowInfo>
	{
        /// <summary>
        /// 获取指定流程模板的流程环节列表
        /// </summary>
        List<FormFlowInfo> GetFormFlow(string form_id);
                        
        /// <summary>
        /// 获取指定流程模板的第一个流程环节信息
        /// </summary>
        FormFlowInfo GetFirstFormFlow(string form_id);
                        
        /// <summary>
        /// 为指定模板和顺序的后续流程（包含当前）的顺序+1，用于插入新的流程
        /// </summary>
        /// <param name="form_Id">模板</param>
        /// <param name="orderId">流程顺序</param>
        /// <returns></returns>
        bool IncreaseOrder(string form_Id, decimal orderId);
    }
}