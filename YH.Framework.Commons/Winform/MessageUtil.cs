using System;
using System.Windows.Forms;
using YH.Framework.Language;

namespace YH.Framework.Commons
{
	/// <summary>
	/// 框架统一的对话框提示辅助类
	/// </summary>
	public static class MessageUtil
    {
        private static string Caption_Tips = "提示信息";
        private static string Caption_Warning = "警告信息";
        private static string Caption_Error = "错误信息";

        static MessageUtil()
        {
            Caption_Tips = JsonLanguage.Default.GetString(Caption_Tips);
            Caption_Warning = JsonLanguage.Default.GetString(Caption_Warning);
            Caption_Error = JsonLanguage.Default.GetString(Caption_Error);
        }

		/// <summary>
		/// 显示一般的提示信息
		/// </summary>
		/// <param name="message">提示信息</param>
		public static DialogResult ShowTips(string message)
        {
            return ShowTips(message, null);
        }

        /// <summary>
        /// 显示一般的提示信息
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="args">字符串里面的参数内容</param>
        /// <returns></returns>
		public static DialogResult ShowTips(string message, params object[] args)
		{
            message = JsonLanguage.Default.GetString(message);
            if (args != null)
            {
                message = string.Format(message, args);
            }

			return MessageBox.Show(message, Caption_Tips,MessageBoxButtons.OK, MessageBoxIcon.Information);
		}


        /// <summary>
        /// 显示警告信息
        /// </summary>
        /// <param name="message">警告信息</param>
        public static DialogResult ShowWarning(string message)
        {
            return ShowWarning(message, null);
        }

        /// <summary>
        /// 显示警告信息
        /// </summary>
        /// <param name="message">警告信息</param>
        /// <param name="args">字符串里面的参数内容</param>
        public static DialogResult ShowWarning(string message, params object[] args)
        {
            message = JsonLanguage.Default.GetString(message);
            if (args != null)
            {
                message = string.Format(message, args);
            }

			return MessageBox.Show(message, Caption_Warning,  MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        public static DialogResult ShowError(string message)
        {
            return ShowError(message, null);
        }
        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="args">字符串里面的参数内容</param>
        public static DialogResult ShowError(string message, params object[] args)
        {
            message = JsonLanguage.Default.GetString(message);
            if (args != null)
            {
                message = string.Format(message, args);
            }
			return MessageBox.Show(message, Caption_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

        /// <summary>
        /// 显示询问用户信息，并显示错误标志
        /// </summary>
        /// <param name="message">错误信息</param>
        public static DialogResult ShowYesNoAndError(string message)
        {
            return ShowYesNoAndError(message, null);
        }
        /// <summary>
        /// 显示询问用户信息，并显示错误标志
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="args">字符串里面的参数内容</param>
        public static DialogResult ShowYesNoAndError(string message, params object[] args)
        {
            message = JsonLanguage.Default.GetString(message);
            if (args != null)
            {
                message = string.Format(message, args);
            }
			return MessageBox.Show(message, Caption_Error, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
		}

        /// <summary>
        /// 显示询问用户信息，并显示提示标志
        /// </summary>
        /// <param name="message">错误信息</param>
        public static DialogResult ShowYesNoAndTips(string message)
        {
            return ShowYesNoAndTips(message, null);
        }

        /// <summary>
        /// 显示询问用户信息，并显示提示标志
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="args">字符串里面的参数内容</param>
        public static DialogResult ShowYesNoAndTips(string message, params object[] args)
        {
            message = JsonLanguage.Default.GetString(message);
            if (args != null)
            {
                message = string.Format(message, args);
            }
			return MessageBox.Show(message, Caption_Tips, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
		}

        /// <summary>
        /// 显示询问用户信息，并显示警告标志
        /// </summary>
        /// <param name="message">警告信息</param>
        public static DialogResult ShowYesNoAndWarning(string message)
        {
            return ShowYesNoAndWarning(message, null);
        }

        /// <summary>
        /// 显示询问用户信息，并显示警告标志
        /// </summary>
        /// <param name="message">警告信息</param>
        /// <param name="args">字符串里面的参数内容</param>
        public static DialogResult ShowYesNoAndWarning(string message, params object[] args)
        {
            message = JsonLanguage.Default.GetString(message);
            if (args != null)
            {
                message = string.Format(message, args);
            }
            return MessageBox.Show(message, Caption_Warning,  MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 显示询问用户信息，并显示提示标志
        /// </summary>
        /// <param name="message">错误信息</param>
        public static DialogResult ShowYesNoCancelAndTips(string message)
        {
            return ShowYesNoCancelAndTips(message, null);
        }

        /// <summary>
        /// 显示询问用户信息，并显示提示标志
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="args">字符串里面的参数内容</param>
        public static DialogResult ShowYesNoCancelAndTips(string message, params object[] args)
        {
            message = JsonLanguage.Default.GetString(message);
            if (args != null)
            {
                message = string.Format(message, args);
            }
            return MessageBox.Show(message, Caption_Tips, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示一个YesNo选择对话框
        /// </summary>
        /// <param name="message">对话框的选择内容提示信息</param>
        /// <returns>如果选择Yes则返回true，否则返回false</returns>
        public static bool ConfirmYesNo(string message)
        {
            message = JsonLanguage.Default.GetString(message);
            return MessageBox.Show(message, Caption_Tips, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        /// <summary>
        /// 显示一个YesNoCancel选择对话框
        /// </summary>
        /// <param name="message">对话框的选择内容提示信息</param>
        /// <returns>返回选择结果的的DialogResult值</returns>
        public static DialogResult ConfirmYesNoCancel(string message)
        {
            return MessageBox.Show(message, Caption_Tips, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 询问一个输入字符串
        /// </summary>
        /// <param name="prompt">提示信息</param>
        /// <param name="initValue">初始值</param>
        /// <param name="isPassword">是否密码字符串</param>
        /// <returns>询问到的字符串</returns>
        public static string QueryInputStr(string prompt, string initValue = "", bool isPassword = false)
        {
            prompt = JsonLanguage.Default.GetString(prompt);

            QueryInputDialog dlg = new QueryInputDialog();
            dlg.Text = prompt;
            dlg.lblPrompt.Text = prompt.EndsWith(":") || prompt.EndsWith("：") ? prompt : prompt + ":";
            dlg.txtInput.Text = initValue;
            dlg.IsEncryptInput = isPassword;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.txtInput.Text;
            }
            return initValue;
        }
	}
}
