using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Infrastructure.Crosscutting.Updater.Wrapper
{
	delegate TR Func<TS, TR>(TS ele);

	static class ExtensionMethod
	{
		/// <summary>
		/// 为字符串设定默认值
		/// </summary>
		/// <param name="value">要设置的值</param>
		/// <param name="defaultValue">如果要设定的值为空，则返回此默认值</param>
		/// <returns>设定后的结果</returns>
		public static string DefaultForEmpty(string value, string defaultValue)
		{
			return string.IsNullOrEmpty(value) ? defaultValue : value;
		}



		readonly static string[] SizeDefinitions = new[] {
		"字节",
		"KB",
		"MB",
		"GB",
		"TB"
		};

		/// <summary>
		/// 控制尺寸显示转换上限
		/// </summary>
		readonly static double SizeLevel = 0x400 * 0.9;

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(double size)
		{
			return ToSizeDescription(size, 2);
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <param name="digits">小数位数</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(double size, int digits)
		{
			var sizeDefine = 0;


			while (sizeDefine < SizeDefinitions.Length && size > SizeLevel)
			{
				size /= 0x400;
				sizeDefine++;
			}


			if (sizeDefine == 0) return size.ToString("#0") + SizeDefinitions[sizeDefine];
			else
			{
				return size.ToString("#0." + string.Empty.PadLeft(digits, '#')) + SizeDefinitions[sizeDefine];
			}
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(ulong size)
		{
			return ToSizeDescription((double)size);
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <param name="digits">小数位数</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(ulong size, int digits)
		{
			return ToSizeDescription((double)size, digits);
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(long size)
		{
			return ToSizeDescription((double)size);
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <param name="digits">小数位数</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(long size, int digits)
		{
			return ToSizeDescription((double)size, digits);
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(int size)
		{
			return ToSizeDescription((double)size);
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <param name="digits">小数位数</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(int size, int digits)
		{
			return ToSizeDescription((double)size, digits);
		}


		/// <summary>
		/// 同步一个不需要参数的回调到目标线程
		/// </summary>
		/// <param name="context"></param>
		/// <param name="callback"></param>
		public static void Send(SynchronizationContext context, SendOrPostCallback callback)
		{
			context.Send(callback, null);
		}

		public static IEnumerable<TResult> Select<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> func)
		{
			foreach (var item in source)
			{
				yield return func(item);
			}
		}

		public static List<T> ToList<T>(IEnumerable<T> source)
		{
			List<T> list = new List<T>();
			foreach (var item in source)
			{
				list.Add(item);
			}
			return list;
		}

	}
}
