using System;

namespace YH.Security.Common
{
	public class MyException : Exception
	{
		public MyException() : base("")
		{
		}

		public MyException(string message) : base(message)
		{
		}
	}
}
