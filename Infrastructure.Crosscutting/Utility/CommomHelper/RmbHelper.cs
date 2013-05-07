using System;
namespace Infrastructure.Crosscutting.Utility.CommomHelper
{
	/// <summary> 
	/// 人民币操作类 ：转换人民币大小金额。 
	/// 1、CmycurD(decimal num)转换人民币大小金额 ，返回大写形式
	/// 2、CmycurD(string numstr)转换人民币大小金额  (一个重载，将字符串先转换成数字在调用CmycurD
	/// </summary> 
	public class RmbHelper
	{
		/// <summary> 
		/// 转换人民币大小金额 
		/// </summary> 
		/// <param name="num">金额</param> 
		/// <returns>返回大写形式</returns> 
		public static string CmycurD(decimal num)
		{
			string text = "零壹贰叁肆伍陆柒捌玖";
			string text2 = "万仟佰拾亿仟佰拾万仟佰拾元角分";
			string text3 = "";
			string str = "";
			string str2 = "";
			int num2 = 0;
			num = Math.Round(Math.Abs(num), 2);
			string text4 = ((long)(num * 100m)).ToString();
			int length = text4.Length;
			if (length > 15)
			{
				return "溢出";
			}
			text2 = text2.Substring(15 - length);
			for (int i = 0; i < length; i++)
			{
				string text5 = text4.Substring(i, 1);
				int startIndex = Convert.ToInt32(text5);
				if (i != length - 3 && i != length - 7 && i != length - 11 && i != length - 15)
				{
					if (text5 == "0")
					{
						str = "";
						str2 = "";
						num2++;
					}
					else
					{
						if (text5 != "0" && num2 != 0)
						{
							str = "零" + text.Substring(startIndex, 1);
							str2 = text2.Substring(i, 1);
							num2 = 0;
						}
						else
						{
							str = text.Substring(startIndex, 1);
							str2 = text2.Substring(i, 1);
							num2 = 0;
						}
					}
				}
				else
				{
					if (text5 != "0" && num2 != 0)
					{
						str = "零" + text.Substring(startIndex, 1);
						str2 = text2.Substring(i, 1);
						num2 = 0;
					}
					else
					{
						if (text5 != "0" && num2 == 0)
						{
							str = text.Substring(startIndex, 1);
							str2 = text2.Substring(i, 1);
							num2 = 0;
						}
						else
						{
							if (text5 == "0" && num2 >= 3)
							{
								str = "";
								str2 = "";
								num2++;
							}
							else
							{
								if (length >= 11)
								{
									str = "";
									num2++;
								}
								else
								{
									str = "";
									str2 = text2.Substring(i, 1);
									num2++;
								}
							}
						}
					}
				}
				if (i == length - 11 || i == length - 3)
				{
					str2 = text2.Substring(i, 1);
				}
				text3 = text3 + str + str2;
				if (i == length - 1 && text5 == "0")
				{
					text3 += '整';
				}
			}
			if (num == 0m)
			{
				text3 = "零元整";
			}
			return text3;
		}
		/// <summary> 
		/// 转换人民币大小金额  (一个重载，将字符串先转换成数字在调用CmycurD)
		/// </summary> 
		/// <param name="num">用户输入的金额，字符串形式未转成decimal</param> 
		/// <returns></returns> 
		public static string CmycurD(string numstr)
		{
			string result;
			try
			{
				decimal num = Convert.ToDecimal(numstr);
				result = RmbHelper.CmycurD(num);
			}
			catch
			{
				result = "非数字形式！";
			}
			return result;
		}
	}
}
