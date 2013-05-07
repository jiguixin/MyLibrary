using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32.SafeHandles;
using System.Security.AccessControl;
using System.IO;
using System.Threading;

namespace Infrastructure.Crosscutting.Updater.Wrapper
{
	/// <summary>
	/// 扩展的文件流，支持进度报告等新功能
	/// </summary>
	public class ExtendFileStream : System.IO.FileStream
	{

		#region  构造函数

		public ExtendFileStream(string path, System.IO.FileMode mode)
			: base(path, mode)
		{

		}
		public ExtendFileStream(string path, System.IO.FileMode mode, System.IO.FileAccess access)
			: base(path, mode, access)
		{

		}
		public ExtendFileStream(string path, System.IO.FileMode mode, System.IO.FileAccess access, System.IO.FileShare share)
			: base(path, mode, access, share)
		{

		}
		public ExtendFileStream(string path, System.IO.FileMode mode, System.IO.FileAccess access, System.IO.FileShare share, int bufferSize)
			: base(path, mode, access, share, bufferSize)
		{

		}
		public ExtendFileStream(string path, System.IO.FileMode mode, System.IO.FileAccess access, System.IO.FileShare share, int bufferSize, System.IO.FileOptions options)
			: base(path, mode, access, share, bufferSize, options)
		{

		}
		public ExtendFileStream(string path, System.IO.FileMode mode, System.IO.FileAccess access, System.IO.FileShare share, int bufferSize, bool useAsync)
			: base(path, mode, access, share, bufferSize, useAsync)
		{

		}
		public ExtendFileStream(string path, System.IO.FileMode mode, System.Security.AccessControl.FileSystemRights rights, System.IO.FileShare share, int bufferSize, System.IO.FileOptions options, System.Security.AccessControl.FileSecurity fileSecurity)
			: base(path, mode, rights, share, bufferSize, options, fileSecurity)
		{

		}
		public ExtendFileStream(string path, System.IO.FileMode mode, System.Security.AccessControl.FileSystemRights rights, System.IO.FileShare share, int bufferSize, System.IO.FileOptions options)
			: base(path, mode, rights, share, bufferSize, options)
		{

		}
		public ExtendFileStream(IntPtr handle, System.IO.FileAccess access)
			: base(handle, access)
		{

		}
		public ExtendFileStream(IntPtr handle, System.IO.FileAccess access, bool ownsHandle)
			: base(handle, access, ownsHandle)
		{

		}
		public ExtendFileStream(IntPtr handle, System.IO.FileAccess access, bool ownsHandle, int bufferSize)
			: base(handle, access, ownsHandle, bufferSize)
		{

		}
		public ExtendFileStream(IntPtr handle, System.IO.FileAccess access, bool ownsHandle, int bufferSize, bool isAsync)
			: base(handle, access, ownsHandle, bufferSize, isAsync)
		{

		}
		public ExtendFileStream(Microsoft.Win32.SafeHandles.SafeFileHandle handle, System.IO.FileAccess access)
			: base(handle, access)
		{

		}
		public ExtendFileStream(Microsoft.Win32.SafeHandles.SafeFileHandle handle, System.IO.FileAccess access, int bufferSize)
			: base(handle, access, bufferSize)
		{

		}
		public ExtendFileStream(Microsoft.Win32.SafeHandles.SafeFileHandle handle, System.IO.FileAccess access, int bufferSize, bool isAsync)
			: base(handle, access, bufferSize, isAsync)
		{

		}

		#endregion

		#region 事件

		/// <summary>
		///<see cref="ExtendFileStream"/> 的流读取进度发生变化
		/// </summary>
		public event EventHandler ProgressChanged;

		/// <summary>
		/// 触发 <see cref="E:FSLib.IO.ExtendFileStream.ProgressChanged"/> 事件
		/// </summary>
		public virtual void OnProgressChanged()
		{
			if (ProgressChanged == null) return;

			ProgressChanged(this, EventArgs.Empty);
		}

		#endregion

		#region 函数重载

		/// <inheritdoc />
		public override int EndRead(IAsyncResult asyncResult)
		{
			int i = base.EndRead(asyncResult);
			OnProgressChanged();
			return i;
		}

		/// <inheritdoc />
		public override void Write(byte[] array, int offset, int count)
		{
			base.Write(array, offset, count);
			OnProgressChanged();
		}

		/// <inheritdoc />
		public override void WriteByte(byte value)
		{
			base.WriteByte(value);
			OnProgressChanged();
		}

		/// <inheritdoc />
		public override int Read(byte[] array, int offset, int count)
		{
			int i = base.Read(array, offset, count);
			OnProgressChanged();
			return i;
		}

		/// <inheritdoc />
		public override int ReadByte()
		{
			int i = base.ReadByte();
			OnProgressChanged();
			return i;
		}

		#endregion

	}
}
