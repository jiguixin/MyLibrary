using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.Utility;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using FileIO = System.IO;
using System.Reflection;

namespace Infrastructure.Data.Seedwork.Declaration
{
    /// <summary>
    /// Base class for all Model object.
    /// </summary>
    [Serializable]
    public abstract class ModelBase : IDataErrorInfo
    {
        #region const
        public const string ENTERPRISE_STORE_CODE = "00000";

        public const int SYSTEM_STATION_TYPE_ID = 1;
        public const string SYSTEM_STATION_CODE = "00";

        public const int SYSTEM_ROLE_ID = 1;

        public const int SYSTEM_EMPL_ID = 1;
        public const string SYSTEM_EMPL_CODE = "00000";
        public const string SYSTEM_EMPL_NUM = "00000";

        public const int DEFAULT_CONTACT_TYPE = 1;      // 电话联系方式类别
        public const int MOU_MINUTE = 1;                // 代表分钟的MOU
        public const int SYSTEM_BIZ_HOUR_ID = 1;
        public const int SYSTEM_PAY_TYPE_CACH = 1;      // 现金付款方式

        public static DateTime DATE_MIN = new DateTime(1900, 1, 1);
        public static DateTime DATE_MAX = new DateTime(2999, 12, 31);
        public static string DOB_MASK = "0000年00月00日";

        public const string SQL_NEXT_DORDER = "SELECT CASE ISNUMERIC(MAX(DisplayOrder)) WHEN 0 THEN 0 ELSE MAX(DisplayOrder) END FROM ";

        /// <summary>
        /// 将有StartAt、EndAt的字段（格式为hh:mm）转为含当日日期的时间，在WHERE中使用。
        /// 例如StationLabor.StartAt, StationLabor.EndAt, StationFixed, StationTimed等
        /// </summary>
        public const string SQL_STARTAT_ENDAT =
                    @"CONVERT(DATETIME, CONVERT(VARCHAR, GETDATE(), 1) + ' ' + StartAt) <= GETDATE()
                      AND CONVERT(DATETIME, CASE WHEN StartAt>EndAt THEN CONVERT(VARCHAR, DATEADD(d, 1, GETDATE()), 1) + ' ' + EndAt ELSE CONVERT(VARCHAR, GETDATE(), 1) + ' ' + EndAt END) >= GETDATE()";
        #endregion

        #region schema
        /// <summary>
        ///                     　表名              　表字段名，PropertySchema（包含了IsRequired、MaxLeng、默认值、数据库constraint等）
        /// </summary>
        public static Dictionary<string, Dictionary<string, PropertySchema>> _schemaInfo = new Dictionary<string, Dictionary<string, PropertySchema>>();

        /// <summary>
        /// </summary>
        /// <param name="derivedModelType">ModelBase派生类类型</param>
        /// <param name="dtSchema"></param>
        /// <param name="master">master里的key是"表名_字段名"</param>
        public static void AddToSchema(Type mDerivedType, DataTable dtSchema, Dictionary<string, PropertySchema> master)
        {
            #region var
            string masterkey = null, desiredTblName = null, desiredColName = null;
            bool isRequired = false;
            int? maxLen = null;
            PropertySchema schemaInfo = null;
            #endregion

            if (dtSchema.Columns.Count == 0)
                return;

            // 这个表已经装过一次了
            if (_schemaInfo.ContainsKey(dtSchema.TableName))
                return;

            Dictionary<string, PropertySchema> fields = new Dictionary<string, PropertySchema>();
            for (int i = 0; i < dtSchema.Columns.Count; i++)
            {
                // 组合成key：表名_字段名，查找master是否包含内容
                masterkey = string.Format("{0}_{1}", dtSchema.TableName, dtSchema.Columns[i].ColumnName);

                isRequired = !dtSchema.Columns[i].AllowDBNull;
                maxLen = null;
                if (dtSchema.Columns[i].MaxLength >= 0)  // 非字符串类型找不到maxleng
                    maxLen = dtSchema.Columns[i].MaxLength;

                // 如果已经有这个PropertySchema，更新
                if (master.ContainsKey(masterkey))
                {
                    schemaInfo = master[masterkey];

                    fields.Add(dtSchema.Columns[i].ColumnName, schemaInfo);
                }
                else
                {
                    fields.Add(dtSchema.Columns[i].ColumnName, schemaInfo = new PropertySchema());
                }

                schemaInfo.IsReqired = isRequired;
                schemaInfo.MaxLength = maxLen;

                #region 从ModelBase派生类上找对应的属性，设置默认值
                if (!string.IsNullOrEmpty(schemaInfo.DefaultValueString))
                {
                    // 先用字段名看能否在mDerivedType上找到对应的属性
                    PropertyInfo pi = mDerivedType.GetProperty(dtSchema.Columns[i].ColumnName);
                    if (pi != null)
                        schemaInfo.DefaultValue = Util.ConvertByType(pi, schemaInfo.DefaultValueString);
                    else
                    {
                        // 不然就要用TableDescriptionAttribute和ColumnDescriptionAttribute匹配mDerivedType上所有的属性
                        PropertyInfo[] pis = mDerivedType.GetProperties();
                        foreach (PropertyInfo loop in pis)
                        {
                            // 如果属性上有定义ColumnDescriptionAttribute，优先使用
                            ColumnDescriptionAttribute[] cols = (ColumnDescriptionAttribute[])loop.GetCustomAttributes(typeof(ColumnDescriptionAttribute), false);
                            if (cols != null && cols.Length > 0)
                            {
                                if (!string.IsNullOrEmpty(cols[0].TableName))
                                    desiredTblName = cols[0].TableName;

                                if (!string.IsNullOrEmpty(cols[0].ColumnName))
                                    desiredColName = cols[0].ColumnName;
                            }

                            // 如果属性上没有指明用哪个表，使用TableDescriptionAttribute上的值
                            if (string.IsNullOrEmpty(desiredTblName))
                            {
                                TableDescriptionAttribute[] tables = (TableDescriptionAttribute[])mDerivedType.GetCustomAttributes(typeof(TableDescriptionAttribute), false);
                                if (tables != null && tables.Length > 0)
                                {
                                    foreach (TableDescriptionAttribute attr in tables)
                                    {
                                        if (attr.Description == dtSchema.TableName)
                                        {
                                            desiredTblName = attr.Description;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(desiredTblName) && !string.IsNullOrEmpty(desiredColName)
                                && desiredTblName == dtSchema.TableName && desiredColName == dtSchema.Columns[i].ColumnName)
                            {
                                schemaInfo.DefaultValue = Util.ConvertByType(loop, schemaInfo.DefaultValueString);
                                break;
                            }

                            if (string.IsNullOrEmpty(desiredTblName) || !string.IsNullOrEmpty(desiredColName))
                                Debug.WriteLine(string.Format("============================= {0}为什么没有对应字段{1}的属性？=======================================", mDerivedType.Name, dtSchema.Columns[i].ColumnName));
                        }
                    }
                }
                #endregion
            }

            if (fields != null && fields.Count > 0)
                _schemaInfo.Add(dtSchema.TableName, fields);
        }

        /// <summary>
        /// 通过TableDescriptionAttribute和ColumnDescriptionAttribute，查找ModelBase派生类上的属性对应的表名和字段名
        /// </summary>
        public static void MappedTableNColumn(Type callerType, string propertyName, out string tblName, out string colName)
        {
            tblName = colName = null;

            TableDescriptionAttribute[] tables = (TableDescriptionAttribute[])callerType.GetCustomAttributes(typeof(TableDescriptionAttribute), false);
            if (tables != null && tables.Length > 0)
            {
                PropertyInfo pi = callerType.GetProperty(propertyName);
                Debug.Assert(pi != null);

                // 如果有ColumnDesc，用
                ColumnDescriptionAttribute[] cols = (ColumnDescriptionAttribute[])pi.GetCustomAttributes(typeof(ColumnDescriptionAttribute), false);
                if (cols != null && cols.Length > 0)
                {
                    if (!string.IsNullOrEmpty(cols[0].TableName))
                        tblName = cols[0].TableName;
                    else
                        tblName = tables[0].Description;

                    if (!string.IsNullOrEmpty(cols[0].ColumnName))
                        colName = cols[0].ColumnName;
                }

                if (string.IsNullOrEmpty(tblName))
                    tblName = tables[0].Description;

                if (string.IsNullOrEmpty(colName))
                    colName = propertyName;
            }

            Debug.Assert(ModelBase._schemaInfo.ContainsKey(tblName) && ModelBase._schemaInfo[tblName].ContainsKey(colName), string.Format("你是不是忘了写{0}的TableDescriptionAttribute或ColumnDescriptionAttribute", callerType));
        }

        public static bool IsRequiredA(string tblName, string colName)
        {
            if (!_schemaInfo.ContainsKey(tblName))
                return false;

            if (!_schemaInfo[tblName].ContainsKey(colName))
                return false;

            return _schemaInfo[tblName][colName].IsReqired;
        }

        /// <summary>
        /// 如果你的派生类有特殊逻辑，可以重写此方法。不然，就是使用的数据库设置作为是否必填检验
        /// </summary>
        public virtual bool IsRequired(string propertyName)
        {
            string tblName, colName;
            MappedTableNColumn(this.GetType(), propertyName, out tblName, out colName);
            return ModelBase.IsRequiredA(tblName, colName);
        }

        public static int? MaxLengA(string tblName, string colName)
        {
            if (_schemaInfo.ContainsKey(tblName))
            {
                if (_schemaInfo[tblName].ContainsKey(colName))
                {
                    if (_schemaInfo[tblName][colName].MaxLength.HasValue)
                        return _schemaInfo[tblName][colName].MaxLength;
                }
            }

            return null;
        }

        /// <summary>
        /// 如果你的派生类有特殊逻辑，可以重写此方法（例如，图片最大大小）。不然，就是使用的数据库设置作为文本类型字段最大长度限制
        /// </summary>
        public virtual int? MaxLeng(string propertyName)
        {
            string tblName, colName;
            MappedTableNColumn(this.GetType(), propertyName, out tblName, out colName);
            return ModelBase.MaxLengA(tblName, colName);
        }

        //public static bool EvalExprA(string tblName, string colName, object val)
        //{
        //    Debug.Assert(_schemaInfo.ContainsKey(tblName)
        //        && _schemaInfo[tblName].ContainsKey(colName)
        //        && _schemaInfo[tblName][colName].IExpr != null
        //        && _schemaInfo[tblName][colName].Context != null
        //        && val != null);

        //    _schemaInfo[tblName][colName].Context.Variables[colName] = val;

        //    return (bool)_schemaInfo[tblName][colName].IExpr.Evaluate();
        //}

        //public bool EvalExpr(string propertyName, object val)
        //{
        //    string tblName, colName;
        //    MappedTableNColumn(this.GetType(), propertyName, out tblName, out colName);
        //    return ModelBase.EvalExprA(tblName, colName, val);
        //}

        public static T DefaultOfA<T>(string tblName, string colName)
        {
            Debug.Assert(_schemaInfo.ContainsKey(tblName)
                && _schemaInfo[tblName].ContainsKey(colName)
                && _schemaInfo[tblName][colName].DefaultValue != null, string.Format("查一下数据库{0}表中是否没有设置{1}的默认值！", tblName, colName));

            return (T)_schemaInfo[tblName][colName].DefaultValue;
        }

        public T DefaultOf<T>(string propertyName)
        {
            string tblName, colName;
            MappedTableNColumn(this.GetType(), propertyName, out tblName, out colName);
            return ModelBase.DefaultOfA<T>(tblName, colName);
        }

        public static PropertySchema SchemaOf(string tbleName, string colName)
        {
            if (ModelBase._schemaInfo.ContainsKey(tbleName))
            {
                if (ModelBase._schemaInfo[tbleName].ContainsKey(colName))
                    return ModelBase._schemaInfo[tbleName][colName];
            }

            return null;
        }
        #endregion

        #region in-Memory Member
        public abstract bool IsNew { get; }

        /// <summary>
        /// Some Model object uses SysId(int), some uses SysGuid(Guid), some may not have primary key
        /// </summary>
        public abstract object Key { get; }

        public abstract string Text { get; }
        #endregion

        #region ctor
        protected ModelBase()
        {
        }
        #endregion

        #region IDataErrorInfo
        string IDataErrorInfo.Error { get { return null; } }
        
        string IDataErrorInfo.this[string propertyName]
        {
            get { return GetValidationError(propertyName); }
        }

        protected abstract string GetValidationError(string propertyName);
        #endregion

        #region Overridable
        /// <summary>
        /// Save model member value to database. Called only by Enterprise.SysControl
        /// </summary>
        public virtual Response SaveUpdate(SqlConnection cnn, LoginClientInfo lcInfo)
        {
            throw new NotImplementedException(this.GetType().ToString());
        }

        /// <summary>
        /// 批量地保存多个Model对象
        /// </summary>
        /// <param name="models">其中第一个是本实例（this）</param>
        public virtual Response BatchSave(SqlConnection cnn, LoginClientInfo lcInfo, ref List<ModelBase> models)
        {
            throw new NotImplementedException(this.GetType().ToString());
        }

        public virtual Response SaveStatus(SqlConnection cnn, LoginClientInfo lcInfo, string keys)
        {
            throw new NotImplementedException(this.GetType().ToString());
        }

        /// <summary>
        /// 只在物理删除时使用
        /// </summary>
        public virtual Response HardDelete(SqlConnection cnn, LoginClientInfo lcInfo)
        {
            throw new NotImplementedException(this.GetType().ToString());
        }
        #endregion

        #region Public Helper
        /// <summary>
        /// 从DataRow中读取array of bytes (Picture.PictureBytes)
        /// </summary>
        public static byte[] GetBytes(DataRow dr, string colName)
        {
            if (dr[colName] != DBNull.Value)
                return (byte[])dr[colName];
            return null;
        }

        /// <summary>
        /// 在内存中进行图片的binary拷贝
        /// </summary>
        public static byte[] ByteCopy(byte[] byteSrc)
        {
            byte[] result = null;

            using (FileIO.MemoryStream ms = new FileIO.MemoryStream(byteSrc))
            {
                result = ms.ToArray();
                ms.Close();
            };

            return result;
        }

        /// <summary>
        /// 从DataRow中读取/写入值，值可能是空的情况。
        /// </summary>
        public static T Get<T>(DataRow dr, string colName)
        {
            // 只有表中包含这个列，我们才取值，不然就返回类型的默认值
            if (dr.Table.Columns.Contains(colName) && dr[colName] != DBNull.Value)
                return (T)dr[colName];

            return default(T);
        }

        /// <summary>
        /// 把百分比的值转成格式化的字符串，如0.9=>90%
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public static string GetPercent(DataRow dr, string colName)
        {
            if (dr.Table.Columns.Contains(colName) && dr[colName] != DBNull.Value)
                return string.Format("{0:P0}", dr[colName]);

            return null;
        }

        /// <summary>
        /// 把Money转成格式化的字符串，如12.36=>￥12.36
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public static string GetPrice(DataRow dr, string colName)
        {
            if (dr.Table.Columns.Contains(colName) && dr[colName] != DBNull.Value)
                return string.Format("{0:C}", dr[colName]);

            return null;
        }

        public static string GetDOB(DataRow dr, string dobColName, string calendarKindColName)
        {
            if (dr.Table.Columns.Contains(dobColName) && dr[dobColName] != DBNull.Value
                && dr.Table.Columns.Contains(calendarKindColName) && dr[calendarKindColName] != DBNull.Value)
            {
                DateTime dob = dr[dobColName].ToDateTime();
                CalendarKind k = (CalendarKind)dr[calendarKindColName];

                // 可能没有年
                if (dob.Year == ModelBase.DATE_MIN.Year)
                    return string.Format("{0}月{1}日", TextHelper.SupZero(dob.Month.ToString(), 2, true), TextHelper.SupZero(dob.Day.ToString(), 2, true));
                else
                {
                    if (k == CalendarKind.Lunar)
                        return ChineseLunisolar.GetChineseDateTime(dob);

                    return dob.ToString("yyyy年MM月dd日");
                }
            }

            return null;
        }

        public static void ToDOB(string dateVal, string dobColName, CalendarKind calendarKindVal, string calendarKindColName, DataRow dr)
        {
            ConfirmColumn(dr, dobColName);
            ConfirmColumn(dr, calendarKindColName);

            if (dateVal.IsEmptyString() || dateVal == ModelBase.DOB_MASK)
                dr[dobColName] = DBNull.Value;
            else
            {
                DateTime result;
                DateTime dob = DateTime.Parse(dateVal);
                if (dob.Year == DateTime.Today.Year)    // 如果界面没有输入，是0000。DateTime.Parse的时候，是今年
                    result = new DateTime(ModelBase.DATE_MIN.Year, dob.Month, dob.Day);
                else
                    result = dob;

                dr[dobColName] = result.ToShortDateString();
                dr[calendarKindColName] = (byte)calendarKindVal;
            }
        }

        /// <summary>
        /// 向DataRow中写入值，值可能是空的情况。
        /// </summary>
        public static void To<T>(T value, DataRow dr, string colName)
        {
            // 因为是保存，必须验证表中是否有这个列
            ConfirmColumn(dr, colName);

            if (value != null)
            {
                if (value.GetType() == typeof(string) && value.ToString().IsEmptyString())
                    dr[colName] = DBNull.Value;
                else
                    dr[colName] = value;
            }
            else
                dr[colName] = DBNull.Value;
        }
        
        /// <summary>
        /// 这个方法是用来判断输入的Code值是否为0
        /// </summary>
        /// <param name="code">用户输入的Code值</param>
        /// <returns>如果为0则返回false，否则返回true</returns>
        public static bool IsValidCode(string code)
        {
            int iVal = 0;

            if (!string.IsNullOrEmpty(code) && code.IsNumberString())
                return int.TryParse(code, out iVal);

            return false;
        }

        public static bool IsGlobalRecord(string code)
        {
            return (code.Substring(0, ApplicationSetting.STORE_CODE.Length) == ModelBase.ENTERPRISE_STORE_CODE);
        }

        public static bool IsTypeModelBase(Type t)
        {
            if (t == typeof(ModelBase) || t.BaseType == typeof(ModelBase))
                return true;
            else if (t.BaseType != null)
                return IsTypeModelBase(t.BaseType);

            return false;
        }

        public static bool IsEnterprise(LoginClientInfo lcInfo)
        {
            return (EnterpriseAppModuleDataProvider.GetOptions().Contains(lcInfo.ClientData.AppModuleData));
        }        

        #region CRProperty Related
        public static string CRTARGETTABLE_PROPERTY_NAME = "CRTargetTable";
        public static string ATTACHEDLABELS_PROPERTY_NAME = "AttachedLabels";
        public static string CONTACTTARGETTABLE_PROPERTY_NAME = "ContactTargetTable";
        public static string PROOFTARGETTABLE_PROPERTY_NAME = "ProofTargetTable";
        public static string CLAIMDATETARGETTABLE_PROPERTY_NAME = "ClaimDateTargetTable";

        public static PropertyInfo GetCRTargetTablePI(Type modelType)
        {
            // 为保证门店的model能取到从HQ的model继承的static property，必须specify FlattenHierarchy
            return modelType.GetProperty(ModelBase.CRTARGETTABLE_PROPERTY_NAME, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        }

        public static PropertyInfo GetAttachedLabelPI(ModelBase model)
        {
            // 为保证门店的model能取到从HQ的model继承的static property，必须specify FlattenHierarchy
            return model.GetType().GetProperty(ModelBase.ATTACHEDLABELS_PROPERTY_NAME, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        }

        public static CRTableKind GetCRTargetTableByProperty(Type modelType)
        {
            PropertyInfo pi = ModelBase.GetCRTargetTablePI(modelType);
            Debug.Assert(pi != null, "你的model上没有定义静态CRTargetTable属性！");

            // 因为CRTargetTable都是ModelBase派生类中的静态属性，所以不需要实例
            return (CRTableKind)pi.GetValue(null, null);
        }

        public static List<CRPropertyValue> GetAttachedLabelProperty(ModelBase model)
        {
            PropertyInfo pi = ModelBase.GetAttachedLabelPI(model);
            Debug.Assert(pi != null, "你的model上没有定义AttachedLabels属性！");
            return (List<CRPropertyValue>)pi.GetValue(model, null);
        }
        
        /// <summary>
        /// 在Workspace中做query时，调用此方法，装载需要的外挂表。
        /// 返回表的顺序：
        /// [0] - CRPropertyValue
        /// [1] - Contact
        /// [2] - Proof, [3] - Proof -> Picture
        /// [4] - ClaimDate
        /// </summary>
        //public static string QueryAttachedSQL(Type modelType, object sysKey)
        //{
        //    #region var
        //    CRTableKind? crKind = null;
        //    ContactTableKind? contactKind = null;
        //    ProofTableKind? proofKind = null;
        //    ClaimDateKind? claimDKind = null;

        //    PropertyInfo pi = null;
        //    #endregion

        //    StringBuilder sb = new StringBuilder();

        //    #region CRProperty
        //    pi = ModelBase.GetCRTargetTablePI(modelType);
        //    if (pi != null)
        //    {
        //        crKind = (CRTableKind)pi.GetValue(null, null);

        //        if (crKind.HasValue)
        //            sb.Append(string.Format(
        //                "SELECT * FROM CRPropertyValue WHERE TargetTable={0}{1};",
        //                (byte)crKind.Value,
        //                sysKey != null ? " AND TargetPK='" + sysKey.ToString() + "'" : string.Empty));
        //    }
        //    #endregion

        //    #region Contact
        //    pi = modelType.GetProperty(ModelBase.CONTACTTARGETTABLE_PROPERTY_NAME, BindingFlags.Static | BindingFlags.Public);
        //    if (pi != null)
        //    {
        //        contactKind = (ContactTableKind)pi.GetValue(null, null);

        //        if (contactKind.HasValue)
        //            sb.Append(string.Format(
        //                "SELECT * FROM Contact WHERE TargetTable={0}{1};",
        //                (byte)contactKind.Value,
        //                sysKey != null ? " AND TargetPK='" + sysKey.ToString() + "'" : string.Empty));
        //    }
        //    #endregion

        //    #region Proof
        //    pi = modelType.GetProperty(ModelBase.PROOFTARGETTABLE_PROPERTY_NAME, BindingFlags.Static | BindingFlags.Public);
        //    if (pi != null)
        //    {
        //        proofKind = (ProofTableKind)pi.GetValue(null, null);

        //        if (proofKind.HasValue)
        //            sb.Append(string.Format(
        //                "SELECT PF.*, PICT.PictureBytes AS PhotoBytes FROM Proof AS PF LEFT JOIN Picture AS PICT ON PF.PhotoId=PICT.SysId WHERE PF.TargetTable={0}{1};",
        //                (byte)proofKind.Value,
        //                sysKey != null ? " AND TargetPK='" + sysKey.ToString() + "'" : string.Empty));
        //    }
        //    #endregion

        //    #region ClaimDate
        //    pi = modelType.GetProperty(ModelBase.CLAIMDATETARGETTABLE_PROPERTY_NAME, BindingFlags.Static | BindingFlags.Public);
        //    if (pi != null)
        //    {
        //        claimDKind = (ClaimDateKind)pi.GetValue(null, null);

        //        if (claimDKind.HasValue)
        //            sb.Append(string.Format(
        //                "SELECT * FROM ClaimDate WHERE TargetTable={0}{1};",
        //                (byte)claimDKind.Value,
        //                sysKey != null ? " AND TargetPK='" + sysKey.ToString() + "'" : string.Empty));
        //    }
        //    #endregion

        //    return sb.ToString();
        //}

        /// <summary>
        /// 保存时Model数据时，调用此方法，得到query外挂表的SELECT statement。
        /// 返回表的顺序：
        /// [0] - CRProperty, [1] - CRPropertyOption, [2] - CRPropertyValue
        /// [3] - ContactType, [4] - Contact
        /// [5] - ProofType, [6] - Proof, [7] - Proof -> Picture
        /// [8] - ClaimDate
        /// </summary>
        /// <param name="model">要进行保存的model对象</param>
        /// <returns>SQL SELECT语句</returns>
//        public static string UpdateAttachedSQL(ModelBase model)
//        {
//            #region var
//            Type modelType = model.GetType();
//            CRTableKind? crKind = null;
//            ContactTableKind? contactKind = null;
//            ProofTableKind? proofKind = null;
//            ClaimDateKind? claimDKind = null;
//            bool? proofIsOrg = null;

//            PropertyInfo pi = null;
//            #endregion

//            StringBuilder sb = new StringBuilder();

//            #region CRProperty
//            pi = ModelBase.GetCRTargetTablePI(model.GetType());
//            if (pi != null)
//            {
//                crKind = (CRTableKind)pi.GetValue(null, null);

//                if (crKind.HasValue)
//                {
//                    sb.Append(string.Format(
//                        @"SELECT SysId, Title, TargetTables FROM CRProperty;SELECT SysId, Title, CRPropertyId FROM CRPropertyOption;
//                        SELECT * FROM CRPropertyValue WHERE TargetTable={0} AND TargetPK{1};",
//                        (byte)crKind.Value,
//                        model.Key != null ? "='" + model.Key.ToString() + "'" : " IS NULL"));
//                }
//            }
//            #endregion

//            #region Contact
//            pi = modelType.GetProperty(ModelBase.CONTACTTARGETTABLE_PROPERTY_NAME, BindingFlags.Static | BindingFlags.Public);
//            if (pi != null)
//            {
//                contactKind = (ContactTableKind)pi.GetValue(null, null);
//                if (contactKind.HasValue)
//                    sb.Append(string.Format(
//                        @"SELECT * FROM ContactType;
//                        SELECT * FROM Contact WHERE TargetTable={0} AND TargetPK{1};",
//                        (byte)contactKind.Value,
//                        model.Key != null ? "='" + model.Key.ToString() + "'" : " IS NULL"));
//            }
//            #endregion

//            #region Proof
//            pi = modelType.GetProperty(ModelBase.PROOFTARGETTABLE_PROPERTY_NAME, BindingFlags.Static | BindingFlags.Public);
//            if (pi != null)
//            {
//                proofKind = (ProofTableKind)pi.GetValue(null, null);

//                if (proofKind.HasValue)
//                {
//                    proofIsOrg = (bool)modelType.GetProperty("ProofIsOrg", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);

//                    sb.Append(string.Format(
//                        @"SELECT * FROM ProofType WHERE IsOrganization={0};
//                        SELECT * FROM Proof WHERE TargetTable={1} AND TargetPK{2};
//                        SELECT * FROM Picture WHERE SysId IN (SELECT PhotoId FROM Proof WHERE TargetTable={1} AND TargetPK{2});",
//                        proofIsOrg.Value ? 1 : 0,
//                        (byte)proofKind.Value,
//                        model.Key != null ? "='" + model.Key.ToString() + "'" : " IS NULL"));
//                }
//            }
//            #endregion

//            #region ClaimDate
//            pi = modelType.GetProperty(ModelBase.CLAIMDATETARGETTABLE_PROPERTY_NAME, BindingFlags.Static | BindingFlags.Public);
//            if (pi != null)
//            {
//                claimDKind = (ClaimDateKind)pi.GetValue(null, null);
//                if (claimDKind.HasValue)
//                    sb.Append(string.Format(
//                        "SELECT * FROM ClaimDate WHERE TargetTable={0}{1};",
//                        (byte)claimDKind.Value,
//                        model.Key != null ? "='" + model.Key.ToString() + "'" : " IS NULL"));
//            }
//            #endregion

//            return sb.ToString();
//        }        
        #endregion        
        #endregion

        #region Internal Helper
        private static void ConfirmColumn(DataRow dr, string colName)
        {
#if DEBUG
            if (!dr.Table.Columns.Contains(colName))
            {
                StackTrace st = new StackTrace();
                if (st.FrameCount > 2)
                {
                    // get calling frame, not myself
                    StackFrame sf = st.GetFrame(2);
                    MethodBase mb = sf.GetMethod();

                    Debug.Fail(string.Format("{0}.{1}产生错误：表中不包含{2}列.", mb.DeclaringType.Name, mb.Name, colName));
                }
            }
#endif
        }
        #endregion

        #region Obsolete
        /// <summary>
        /// 读schema表里面的必填、字符串类型最大长度、校验、默认值等
        /// </summary>
        /// <param name="colNames">数据库中字段名称，用“,”分隔</param>
        [Obsolete("请不要再用这个方法，去掉ModelBase派生类中的schema部分")]
        public static void GetSchema(DataTable dtSchema, Dictionary<string, ModelSchema> master, Dictionary<string, ModelSchema> eval)
        {
            #region var
            StackTrace stackTrace = new StackTrace();
            string callingType = stackTrace.GetFrame(1).GetMethod().DeclaringType.Name;
            string key = null;
            bool isRequired = false;
            int? maxLen = null;
            ModelSchema schema = null;
            #endregion

            if (dtSchema.Columns.Count == 0)
                return;

            eval.Clear();
            for (int i = 0; i < dtSchema.Columns.Count; i++)
            {
                // 组合成ModelSchema的key：表名_字段名
                key = string.Format("{0}_{1}", dtSchema.TableName, dtSchema.Columns[i].ColumnName);

                isRequired = !dtSchema.Columns[i].AllowDBNull;
                maxLen = null;
                if (dtSchema.Columns[i].MaxLength >= 0)  // 非字符串类型找不到maxleng
                    maxLen = dtSchema.Columns[i].MaxLength;

                if (master.ContainsKey(key))
                {
                    schema = master[key];
                    master.Remove(key);

                    schema.IsReqired = isRequired;
                    schema.MaxLength = maxLen;

                    // 转为类上认识的key：只包含column name
                    eval.Add(dtSchema.Columns[i].ColumnName, schema);
                }
                else
                    eval.Add(dtSchema.Columns[i].ColumnName, new ModelSchema() { IsReqired = isRequired, MaxLength = maxLen });
            }
        }

        /// <summary>
        /// 使用这个方法时，T不能是Nullable
        /// </summary>
        [Obsolete("请不要再用这个方法，去掉ModelBase派生类中的schema部分")]
        public static T DefaultOf<T>(string colName, Dictionary<string, ModelSchema> eval)
        {
            if (eval.ContainsKey(colName) && !string.IsNullOrEmpty(eval[colName].DefaultValue))
            {
                if (typeof(T) == typeof(bool))
                    return (eval[colName].DefaultValue == "1") ? (T)Convert.ChangeType(true, typeof(T)) : (T)Convert.ChangeType(false, typeof(T));

                return (T)Convert.ChangeType(eval[colName].DefaultValue, typeof(T));
            }

            return default(T);
        }

        /// <summary>
        /// 与UpdateAttachedSQL()相同作用，只是用在物理删除Model数据时，调用此方法，得到query外挂表的SELECT statement。
        /// 返回表的顺序：
        /// [0] - CRPropertyValue
        /// [1] - Contact
        /// [2] - Proof, [3] - Proof -> Picture
        /// [4] - ClaimDate
        /// </summary>
        [Obsolete("不支持这个方法了，因为所有的Model.Delete都要被去掉了")]
        public static string DeleteAttachedSQL(ModelBase model)
        {
            #region var
            Type modelType = model.GetType();
            CRTableKind? crKind = null;
            //ContactTableKind? contactKind = null;
            //ProofTableKind? proofKind = null;
            //ClaimDateKind? claimDKind = null;

            PropertyInfo pi = null;
            #endregion

            StringBuilder sb = new StringBuilder();

            Debug.Assert(model.Key != null, "你在删除数据，怎么又没有主键呢？");

            //#region CRProperty
            //pi = ModelBase.GetCRTargetTablePI(model.GetType());
            ////modelType.GetProperty(ModelBase.CRTARGETTABLE_PROPERTY_NAME, BindingFlags.Static | BindingFlags.Public);
            //if (pi != null)
            //{
            //    crKind = (CRTableKind)pi.GetValue(null, null);

            //    if (crKind.HasValue)
            //        sb.Append(string.Format("SELECT * FROM CRPropertyValue WHERE TargetTable={0} AND TargetPK='{1}';", (byte)crKind.Value, model.Key));
            //}
            //#endregion

            #region Contact
            //pi = modelType.GetProperty(ModelBase.CONTACTTARGETTABLE_PROPERTY_NAME, BindingFlags.Static | BindingFlags.Public);
            //if (pi != null)
            //{
            //    contactKind = (ContactTableKind)pi.GetValue(null, null);
            //    if (contactKind.HasValue)
            //        sb.Append(string.Format("SELECT * FROM Contact WHERE TargetTable={0} AND TargetPK='{1}';", (byte)contactKind.Value, model.Key));
            //}
            #endregion

            //#region Proof
            //pi = modelType.GetProperty(ModelBase.PROOFTARGETTABLE_PROPERTY_NAME, BindingFlags.Static | BindingFlags.Public);
            //if (pi != null)
            //{
            //    proofKind = (ProofTableKind)pi.GetValue(null, null);

            //    if (proofKind.HasValue)
            //        sb.Append(string.Format("SELECT * FROM Proof WHERE TargetTable={0} AND TargetPK='{1}';SELECT * FROM Picture WHERE SysId IN (SELECT PhotoId FROM Proof WHERE TargetTable={0} AND TargetPK='{1}');", (byte)proofKind.Value, model.Key));
            //}
            //#endregion

            //#region ClaimDate
            //pi = modelType.GetProperty(ModelBase.CLAIMDATETARGETTABLE_PROPERTY_NAME, BindingFlags.Static | BindingFlags.Public);
            //if (pi != null)
            //{
            //    claimDKind = (ClaimDateKind)pi.GetValue(null, null);
            //    if (claimDKind.HasValue)
            //        sb.Append(string.Format("SELECT * FROM ClaimDate WHERE TargetTable={0} AND TargetPK='{1}';", (byte)claimDKind.Value, model.Key));
            //}
            //#endregion

            return sb.ToString();
        }
        #endregion
    }

    /// <summary>
    /// 这个类只在Client端作为ComboBox的选项用，所以不需要Serializable。
    /// 而且都是只读字段（即，操作员不能修改任何成员变量），所以不需要schema
    /// </summary>
    public abstract class HierarchicalModelBase : ModelBase
    {
        #region in-Memory Member
        public abstract string HeaderText { get; }

        public HierarchicalModelBase Parent { get; private set; }
        public List<HierarchicalModelBase> Children { get; private set; }

        public int IndentLevel { get; private set; }

        /// <summary>
        /// 根节点的PK
        /// </summary>
        public object RootKey { get; protected set; }

        /// <summary>
        /// 所有祖先节点的PK，一般用于内存中Linq查找
        /// </summary>
        public IList<object> AncestorKeys { get; private set; }

        /// <summary>
        /// 所有末级节点的PK，一般用于内存中Linq查找
        /// </summary>
        public IList<object> LeafKeys { get; private set; }

        /// <summary>
        /// 所有末级节点的SysGuid/SysId，用“,”隔开，一般用于SQL查询
        /// </summary>
        public string LeafKeyStr
        {
            get
            {
                if (LeafKeys.Count > 0)
                {
                    if (LeafKeys[0] is Guid)
                        return TextHelper.ListToString<Guid>(LeafKeys.OfType<Guid>());
                    else
                        return TextHelper.ListToString<int>(LeafKeys.OfType<int>());
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 包含所有父级节点+自己的路径
        /// </summary>
        public string FullPath { get; protected set; }
        #endregion

        #region ctor
        protected HierarchicalModelBase(HierarchicalModelBase parent)
        {
            Parent = parent;
            Children = new List<HierarchicalModelBase>();

            AncestorKeys = new List<object>();
            LeafKeys = new List<object>();

            if (parent != null)
                IndentLevel = parent.IndentLevel + 1;
            else
                IndentLevel = 0;
        }

        /// <summary>
        /// 这个方法是由下往上执行的
        /// </summary>
        protected void PostContruct()
        {
            StringBuilder sb = new StringBuilder();
            HierarchicalModelBase directParent = (HierarchicalModelBase)this.Parent;

            while (directParent != null)
            {
                // 把祖先的PK插入在[0]位（这是一个往上的循环）
                this.AncestorKeys.Insert(0, directParent.Key);

                sb.Insert(0, directParent.HeaderText);

                if (directParent.Parent != null)
                    sb.Insert(0, "/");

                // 我是Leaf，把我的PK加到祖先的LeafKeys中
                if (this.Children.Count == 0)
                    directParent.LeafKeys.Add(this.Key);

                // 走到根了
                if (directParent.Parent == null)
                    RootKey = directParent.Key;

                directParent = directParent.Parent;
            }

            this.AncestorKeys.Add(this.Key);

            sb.Append("/");
            sb.Append(this.HeaderText);

            this.FullPath = sb.ToString();

            if (this.Children.Count == 0)
                LeafKeys.Add(this.Key);
        }
        #endregion

        #region Public Helper
        public static bool IsTypeHierachicalModelBase(Type t)
        {
            if (t == typeof(HierarchicalModelBase) || t.BaseType == typeof(HierarchicalModelBase))
                return true;
            else if (t.BaseType != null)
                return IsTypeHierachicalModelBase(t.BaseType);

            return false;
        }
        #endregion

        protected override string GetValidationError(string propertyName)
        {
            return null;
        }
    }

    [Obsolete("请不要再使用这个类。我们已经能够自动识别ViewModelBase实际使用的Model类型。如果有特殊需要，可以实现ModifySchema方法")]
    public class ModelSchema
    {
        ///// <summary>
        ///// expression context
        ///// 给Context.Variable赋值，例如：Context.Variables["AlertMinutes"] = some value;
        ///// 其中，""中的是数据库表字段名称                            
        ///// </summary>
        //public ExpressionContext Context = null;

        ///// <summary>
        ///// Expression
        ///// 使用这个表达式数进行验证，例如：if (!(bool)IExpr.Evaluate())
        ///// </summary>
        //public IDynamicExpression IExpr = null;

        public bool IsReqired = false;

        /// <summary>
        /// 如果是varchar或char字段，允许的最大长度
        /// </summary>
        public int? MaxLength = null;

        /// <summary>
        /// 如果是decimal字段，允许的digit最多位数，相当于sql server decimal.Precision
        /// </summary>
        public byte? Capacity = null;

        /// <summary>
        /// 如果是decimal字段，允许的小数点后位数，相当于sql server decimal.Scale
        /// </summary>
        public byte? Scale = null;

        /// <summary>
        /// 从数据库中读得的字段的默认值，在新建实例的构造函数中应使用这个默认值
        /// 因为sys.syscomment.text是nvarchar，所以我们只能读成string类型
        /// </summary>
        public string DefaultValue = null;
    }
}