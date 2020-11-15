using System;
using System.Collections.Generic;
using System.Web.Mvc;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using WHC.CRM.BLL;
using WHC.CRM.Entity;

namespace IOT.MVCWebMis.Controllers
{
    public class ContactGroupController : BusinessController<ContactGroup, ContactGroupInfo>
    {
        public ContactGroupController() : base()
        {
        }

        #region д������ǰ�޸Ĳ�������
        protected override void OnBeforeInsert(ContactGroupInfo info)
        {
            //��������Բ�����������޸�
            info.CreateTime = DateTime.Now;
            info.Creator = CurrentUser.ID.ToString();
            info.Company_ID = CurrentUser.Company_ID;
            info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(ContactGroupInfo info)
        {
            //��������Բ�����������޸�
            info.Editor = CurrentUser.ID.ToString();
            info.EditTime = DateTime.Now;
        }
        #endregion

        #region Bootstrap�����б�

        /// <summary>
        /// ��ȡ������б����������б�
        /// </summary>
        /// <param name="creator">��ǰ�û���ID</param>
        /// <returns></returns>
        public ActionResult GetDictJson(string creator)
        {
            List<CListItem> treeList = new List<CListItem>();
            CListItem topNode = new CListItem("��", "-1");
            treeList.Add(topNode);

            List<ContactGroupNodeInfo> groupList = BLLFactory<ContactGroup>.Instance.GetTree(creator);
            AddGroupDict(groupList, treeList);

            return ToJsonContent(treeList);
        }
        private void AddGroupDict(List<ContactGroupNodeInfo> nodeList, List<CListItem> treeList)
        {
            foreach (ContactGroupNodeInfo nodeInfo in nodeList)
            {
                CListItem subNode = new CListItem(nodeInfo.Name, nodeInfo.ID);
                treeList.Add(subNode);

                AddGroupDict(nodeInfo.Children, treeList);
            }
        }

        /// <summary>
        /// ��ȡ��ϵ�˷�����Json�ַ���
        /// </summary>
        /// <returns></returns>
        public ActionResult GetGroupJsTreeJson(string userId)
        {
            //���һ��δ�����ȫ���ͻ������
            List<JsTreeData> treeList = new List<JsTreeData>();
            JsTreeData pNode = new JsTreeData("", "������ϵ��", "fa fa-users icon-state-warning icon-lg");
            treeList.Insert(0, pNode);
            treeList.Add(new JsTreeData("", "δ������ϵ��", "fa fa-users icon-state-warning icon-lg"));

            List<ContactGroupNodeInfo> groupList = BLLFactory<ContactGroup>.Instance.GetTree(userId);
            AddContactGroupJsTree(groupList, pNode);

            return ToJsonContent(treeList);
        }

        /// <summary>
        /// ��ʼ�����󶨿ͻ����˷�����Ϣ
        /// </summary>
        public ActionResult GetMyContactGroupJsTree(string contactId, string userId)
        {
            List<ContactGroupInfo> myGroupList = BLLFactory<ContactGroup>.Instance.GetByContact(contactId);
            List<string> groupIdList = new List<string>();
            foreach (ContactGroupInfo info in myGroupList)
            {
                groupIdList.Add(info.ID);
            }

            List<ContactGroupNodeInfo> groupList = BLLFactory<ContactGroup>.Instance.GetTree(userId);

            List<JsTreeData> treeList = new List<JsTreeData>();
            foreach (ContactGroupNodeInfo nodeInfo in groupList)
            {
                bool check = groupIdList.Contains(nodeInfo.ID);
                JsTreeData treeData = new JsTreeData(nodeInfo.ID, nodeInfo.Name);
                treeData.state  = new JsTreeState(true, check);

                treeList.Add(treeData);
            }

            return ToJsonContent(treeList);
        }

        /// <summary>
        /// ��ȡ�ͻ����鲢��
        /// </summary>
        private void AddContactGroupJsTree(List<ContactGroupNodeInfo> nodeList, JsTreeData treeNode)
        {
            foreach (ContactGroupNodeInfo nodeInfo in nodeList)
            {
                JsTreeData subNode = new JsTreeData(nodeInfo.ID, nodeInfo.Name, "fa fa-user icon-state-warning icon-lg");
                treeNode.children.Add(subNode);

                AddContactGroupJsTree(nodeInfo.Children, subNode);
            }
        }
        #endregion

        /// <summary>
        /// ����ID��ȡ��������
        /// </summary>
        /// <param name="id">����ID</param>
        /// <returns></returns>
        public ActionResult GetNameByID(string id)
        {
            string name = baseBLL.GetFieldValue(id, "Name");
            name = string.IsNullOrEmpty(name) ? "��" : name;
            return ToJsonContent(name);
        }
    }
}
