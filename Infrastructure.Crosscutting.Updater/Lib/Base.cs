using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Crosscutting.Updater.Lib
{
	public delegate void Action<T1, T2>(T1 t1, T2 t2);

	public abstract class Base
	{
		/// <summary>
		/// 进度变化时的回调函数
		/// </summary>
		public Action<int, int> ProgressReportor { get; set; }

		/// <summary>
		/// 获得在操作过程中遇到的错误
		/// </summary>
		public Exception Exception { get; protected set; }


		/// <summary>
		/// 报告进度
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		protected virtual void ReportProgress(int x, int y)
		{
			if (ProgressReportor != null) ProgressReportor(x, y);
		}
	}
}
