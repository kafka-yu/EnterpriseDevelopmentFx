//--------------------------版权信息----------------------------
//       
//                 文件名: CodeTimers                 
//                 CLR Version: 2.0.50727.4927
//                 项目命名空间: MyToolLib
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/7/2 9:41:27
//                 最后更新 : 2010/7/6 0:24:34 v3.0
//                                2010/12/19 17:05 v4.0
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;

using NkjSoft.Extensions;
using System.ComponentModel;
using System.IO;
namespace NkjSoft.Tools.ModelBuilder
{
    /// <summary>
    /// 一个简单的代码生成器，用于生成针对 NkjSoft.ORM 框架中使用的实体层类型。无法继承此类。
    /// </summary>
    public sealed partial class Generator
    {
        #region --- 属性 ---
        /// <summary>
        /// 获取当前所有的数据库名称。
        /// <remarks>只有在调用了 Connect 方法,并成功连接了数据库之后，此属性才会有相应的数据库列表。</remarks>
        /// </summary>
        [Description("获取当前所有的数据库名称。只有在调用了 Connect 方法,并成功连接了数据库之后，此属性才会有相应的数据库列表。")]
        [ReadOnly(true)]
        [Category("数据")]
        [DisplayName("数据库")]
        [Browsable(false)]
        public string[] DataBases { get; private set; }

        /// <summary>
        /// 获取 QueryContext 类名。只有在生成 DataContext 之后。
        /// </summary>
        [ReadOnly(true)]
        [Category("数据")]
        [Description("获取 QueryContext 类名。只有在生成 DataContext 之后。")]
        [DisplayName("QueryContext 类名")]
        [Browsable(false)]
        public string QueryContextName { get; private set; }

        /// <summary>
        /// 获取或设置当前使用的服务器交互对象。
        /// </summary>
        /// <value>The data base server.</value>
        [Browsable(false)]
        public DataBaseServer Server { get; set; }

        /// <summary>
        /// 获取或设置是否同时生成DataContext。
        /// </summary>
        [Description("获取或设置是否同时生成 DataContext。")]
        [DisplayName("生成上下文")]
        [Category("查询上下文")]
        [DefaultValue(true)]
        public bool NeedGenerateDataContext { get; set; }


        /// <summary>
        /// 获取或设置查询上下文的命名空间。
        /// </summary>
        [Description("获取或设置查询上下文的命名空间。\r\n默认(<default>)使用数据库的名字+DataContext后缀。")]
        [DisplayName("命名空间")]
        [DefaultValue("<default>")]
        [Category("查询上下文")]
        [NkjSoft.Validation.Validators.RequireValidator(KeyName = "DataContext 命名空间")]
        public string DataContextNameSpace
        {
            get { return _DataContextNameSpace; }
            set
            {
                _DataContextNameSpace = Templates.OnPropertyChanged(value);
                OnPropertyChanged(new GeneratorPropertyChangedEventArgs(Templates.ModelClassSessionName, "DataContextNameSpace", _DataContextNameSpace));

            }
        }



        private string _DataContextNameSpace;

        /// <summary>
        /// 获取或设置需要生成实体的表名列表。
        /// </summary>
        [Description("获取或设置需要生成实体的表对象列表。")]
        [Category("数据")]
        [ReadOnly(true)]
        [Browsable(false)]
        public List<Table> Tables { get; set; }

        /// <summary>
        /// 获取或设置实体类保存的文件夹路径。
        /// </summary>
        [Description("获取或设置实体类保存的文件夹路径。默认(<default>)路径为 : d:\\model")]
        [DisplayName("保存路径")]
        [DefaultValue("<default>")]
        [Category("输出")]
        [NkjSoft.Validation.Validators.RequireValidator(KeyName = "实体类保存路径")]
        [Editor(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ModelFileFolder
        {
            get { return _ModelFileFolder; }
            set
            {
                _ModelFileFolder = Templates.OnPropertyChanged(value);
                OnPropertyChanged(new GeneratorPropertyChangedEventArgs(Templates.ModelClassSessionName, "ModelFileFolder", _ModelFileFolder));

            }
        }
        private string _ModelFileFolder;
        /// <summary>
        /// 获取一个值，该值指示是否已经成功连接了数据库服务器。
        /// </summary>
        [Browsable(false)]
        [DisplayName("")]
        [Category("结果")]
        [Description("获取一个值，该值指示是否已经成功连接了数据库服务器。")]
        public bool HasConnected { get; set; }

        /// <summary>
        /// 获取或设置用于生成 DataContext 文件的模板内容。
        /// </summary>
        [Description("获取或设置用于生成 DataContext 文件的模板内容。\r\n如果指定的是文件路径,则从文件中加载内容；否则提取默认资源内部的模版（请保留“<UseResource>”）。")]
        [DisplayName("DataContext")]
        [Category("模版")]
        [DefaultValue("<UseResource>")]
        [NkjSoft.Validation.Validators.RequireValidator(KeyName = "DataContext 模版")]
        // [Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Browsable(false)]
        public string DataContextTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置用于生成实体类文件的模板内容。
        /// </summary> 
        [Description("获取或设置用于生成实体类文件的模板内容。\r\n如果指定的是文件路径,则从文件中加载内容；否则提取默认资源内部的模版（请保留“<UseResource>”）。")]
        [DisplayNameAttribute("实体类")]
        [Category("模版")]
        [DefaultValue("<UseResource>")]
        [NkjSoft.Validation.Validators.RequireValidator(KeyName = "实体类 模版")]
        // [Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]//使用多行文本编辑器加载模版文件内容。
        [Browsable(false)]
        public string ModelClassTemplate
        {
            get;
            set;
        }

        private bool generatorInOneFile; //UNDONE 2010-12-17 :悲剧 ,名字写错了..||| 只好将错就错 
        /// <summary>
        /// 获取或设置一个值,表示是否将生成的每个实体类保存在一个 .cs 文件中。
        /// </summary>
        [DisplayName("单文件模式")]
        [Description("获取或设置一个值,表示是否将生成的每个实体类保存在一个 .cs 文件中。\r\n默认为 false ,建议以单独类文件保存最终的生成结果。")]
        [Category("输出")]
        [DefaultValue(false)]
        public bool GeneratorInOneFile
        {
            get { return generatorInOneFile; }
            set
            {
                generatorInOneFile = value;
                if (value)
                    fileWriterHandler = fileWriter.AppendContentTo;
                else
                    fileWriterHandler = fileWriter.WriteFileTo;

                OnPropertyChanged(new GeneratorPropertyChangedEventArgs(Templates.ModelClassSessionName, "GeneratorInOneFile", generatorInOneFile.ToString()));

            }
        }

        #endregion

        /// <summary>
        /// 在生成一个实体类之后发生。
        /// </summary>
        [Description("当生成一个实体类内容,并在类内容被成功写入到指定的文件后触发。")]
        [DisplayName("OnGenerated")]
        [Category("生成")]
        public event EventHandler<GeneratorEventArgs> Generated;

        /// <summary>
        /// 当model类生成选项变化时发生。
        /// </summary>
        [Description("当model类生成选项变化时发生。")]
        [DisplayName("PropertyChanged")]
        public event GeneratorPropertyChangedEventHandler OptionValueChanged;

        /// <summary>
        /// 
        /// </summary>
        private Func<string, string, bool> fileWriterHandler;

        private ModelFileWriter fileWriter;


        #region --- 构造函数 ---


        /// <summary>
        /// 通过模板文件、数据库服务器交互对象实例化一个新的 <see cref="Generator"/> 对象。
        /// </summary>
        /// <param name="dataServer">The data server.</param>
        public Generator(DataBaseServer dataServer)
        {
            this.Server = dataServer;
            fileWriter = new ModelFileWriter();

            //默认每个实体类文件在单独的文件中保存..
            fileWriterHandler = fileWriter.WriteFileTo;
            GeneratorInOneFile = false;
            this.NeedGenerateDataContext = true;

        }

        /// <summary>
        /// 默认实例化一个新的 <see cref="Generator"/> 对象。该实例将拥有支持对 MS-SQLServer 的数据库交互能力。
        /// </summary>
        public Generator()
            : this(new ModelBuilder.DataBaseImpl.SqlDataBaseServer())
        {
            this.ModelClassTemplate = Templates.DefaultDefaultModelTemplateTag;
            this.DataContextTemplate = Templates.DefaultDefaultModelTemplateTag;

        }


        #endregion

        private string modelNameSpaces = null;
        private string dataBaseName = null;

        /// <summary>
        /// 执行生成实体类的方法。该方法接收三个参数，表示命名空间、数据库名称，及是否自动生成数据库查询环境类。
        /// </summary>
        /// <param name="ns">主命名空间</param>
        /// <param name="dbName">数据库名称 .</param>
        /// <param name="generdateDataContext">是否自动生成数据库查询环境类.</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">没有指定需要生成实体的表名。Tables 为空</exception>
        /// <exception cref="System.Exception">还没有连接数据库服务器</exception>
        public bool GenerateModels(string ns, string dbName, bool generdateDataContext)
        {
            if (!HasConnected)
                throw new Exception("还没有连接数据库服务器!");
            if (Tables == null || Tables.Count == 0)
                throw new NullReferenceException("Tables 为空!");
            modelNameSpaces = ns;
            dataBaseName = dbName;
            try
            {
                bool isOk = generate(ns);
                if (generdateDataContext)
                    isOk = GenerateDataContext();
                return isOk;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 生成查询上下文类。
        /// </summary>
        /// <remarks>建议使用此方法生成查询上下文类，免去手工编写。</remarks> 
        /// <returns></returns>
        public bool GenerateDataContext()
        {
            if (Tables.Count == 0)
                return false;
            StringBuilder propertyBuilder = new StringBuilder(string.Empty);
            StringBuilder innerPropertyBuilder = new StringBuilder(string.Empty);
            string tag = "{ get; set; }";
            foreach (var item in Tables)
            {
                propertyBuilder.Append(@"
        /// <summary>
        /// {0}
        /// </summary>".FormatWith(item));
                propertyBuilder.AppendFormat("\r\n        public IEntityTable<{0}> {0} {1}", item, tag);
                innerPropertyBuilder.AppendFormat("\r\n            {0} = Provider.GetTable<{0}>();", item);
            }

            StringBuilder dataContextBuilder = new StringBuilder(DataContextTemplate.IfEmptyReplace(Templates.DefaultDataContextTemplate));
            dataContextBuilder.Replace("$DataBaseName$", dataBaseName).Replace("$ModelNameSpace$", modelNameSpaces)
                 .Replace("$InnerPropertyArea$", innerPropertyBuilder.ToString())
                 .Replace("$PropertyArea$", propertyBuilder.ToString());
            QueryContextName = "{0}DataContext".FormatWith(dataBaseName);
            Log(Tables.Count, QueryContextName, dataContextBuilder.ToString());
            return true;
        }

        /// <summary>
        /// Generates the specified ns.
        /// </summary>
        /// <param name="ns">The ns.</param>
        /// <returns></returns> 
        private bool generate(string ns)
        {
            int status = 0;
            ModelClassTemplate = ModelClassTemplate.IfEmptyReplace(Templates.DefaultModelTemplate);
            string mainTemp = ModelClassTemplate.Replace("$NameSpace$", ns)
                                          .Replace("$Author$", Environment.UserName)
                                          .Replace("$DateTime.Now$", DateTime.Now.ToString());

            foreach (var table in Tables)
            {
                status++;
                string temp = mainTemp.Replace("$ClassName$", table.Name);

                //获取 属性模板
                string propertyTemplate = ModelClassTemplate.Substring(ModelClassTemplate.IndexOf("@@Start") + 7, ModelClassTemplate.IndexOf("@@End") - ModelClassTemplate.IndexOf("@@Start") - 7);
                var columns = table.Columns; //Server.LoadDataTableInfo(table);
                if (columns != null)
                {
                    string result = BuildModelClass(temp, propertyTemplate, columns);
                    Log(status, table.Name, result);
                }
            }
            return status == Tables.Count;
        }

        /// <summary>
        /// 记录生成过程。
        /// </summary>
        /// <param name="status"></param>
        /// <param name="table"></param>
        /// <param name="result"></param>
        private void Log(int status, string table, string result)
        {
            string filePath = string.Format("{0}\\{1}.cs", ModelFileFolder, table);
            //将内容写入文件..
            bool isOk = fileWriter.WriteFileTo(filePath, result);

            OnGenerated(new GeneratorEventArgs(status, table, this.ModelFileFolder, isOk, result, filePath));
        }

        /// <summary>
        /// Called when [generated].
        /// </summary>
        /// <param name="e">The e.</param>
        private void OnGenerated(GeneratorEventArgs e)
        {
            if (Generated != null)
            {
                Generated(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="NkjSoft.Tools.ModelBuilder.GeneratorPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(GeneratorPropertyChangedEventArgs e)
        {
            if (OptionValueChanged != null)
                OptionValueChanged(this, e);
        }

        //...对属性模板进行处理
        //..
        //.. 

        /// <summary>
        /// Builds the model class.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="propertyTemplate">The property template.</param>
        /// <param name="columns">The columns.</param>
        /// <returns></returns>
        private string BuildModelClass(string template, string propertyTemplate, params Column[] columns)
        {
            StringBuilder classBuilder = new StringBuilder(template.Substring(0, template.IndexOf("@@Start")));
            StringBuilder innerCtorBuilder = new StringBuilder();//构造函数 创建器...创建默认值的属性 内部赋值
            var fieldsBuilder = new StringBuilder();
            foreach (Column row in columns)
            {
                StringBuilder propertyBuilder = new StringBuilder(propertyTemplate);
                string dotNetType = ExchangeDotNetType(row);
                fieldsBuilder.AppendFormat("\r\n        private {0} _{1}; ".FormatWith(dotNetType, row.ColumnName));

                propertyBuilder.Replace("$Remark$", row.Remark ?? row.ColumnName)//row.Remark
                    .Replace("$ColumnName$", row.ColumnName)
                    .Replace("$DotNetType$", dotNetType)  // 属性类型（CLR类型）
                    .Replace("$PropertyName$", row.ColumnName) //属性名字
                    .Replace("$ColumnMapping$", row.ColumnMappingResult());
                propertyBuilder.AppendLine();
                classBuilder.Append(propertyBuilder.ToString());
                if (row.DefaultValue.IfNullDefault("Null") != "Null")
                {
                    innerCtorBuilder.AppendFormat("\r\n             {0}", buildCtor(row));
                }
            }
            classBuilder.Replace("$InnerCtor$", innerCtorBuilder.ToString());
            classBuilder.Replace("$Fields$", fieldsBuilder.ToString());
            classBuilder.Append(template.Substring(template.IndexOf("@@End") + 5));
            return classBuilder.ToString();
        }

        private static readonly List<string> t_Sql_Func = new List<string>() { "datetime" };
        private static readonly List<string> t_ints = new List<string>() { "int", "float", "decimal", "bit", "tinyint", "smallint" };
        private static readonly List<string> t_strings = new List<string>() { "varchar", "char", "nvarchar", "nchar", "text", "demo", "wvarchar" };


        /// <summary>
        /// 生成构造函数以及内部部分
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        private string buildCtor(Column column)
        {
            string defValue = column.DefaultValue.Replace("(", string.Empty).Replace(")", string.Empty);
            string datatypeLower = column.DataType.ToLowerInvariant();
            if (t_Sql_Func.Contains(datatypeLower))
            {
                defValue = "DateTime.Now";
            }
            else if (t_ints.Contains(datatypeLower)) //int ..
            {
                if (datatypeLower == "bit")
                    defValue = defValue.Replace("0", "false").Replace("1", "true");
                else
                    defValue = column.DefaultValue;
            }
            else if (t_strings.Contains(datatypeLower))//string 
            {
                defValue = "\"{0}\"".FormatWith(defValue);
            }
            /*
             
             ID = 1;
             {0} = {1} ;
             */

            return "_{0} = {1} ; ".FormatWith(column.ColumnName, defValue);
        }

        /// <summary>
        /// 进行sql数据库类型和.Net CLR 之间类型的转换。
        /// </summary>
        /// <param name="col">The col.</param>
        /// <returns></returns>
        public static string ExchangeDotNetType(Column col)
        {
            string dotNetType = "string";
            switch (col.DataType.ToLowerInvariant())
            {
                case "smallint":
                case "int":
                    dotNetType = col.Nullable.ToBoolean() ? "int?" : "int";
                    break;
                case "datetime":
                    dotNetType = col.Nullable.ToBoolean() ? "DateTime?" : "DateTime";
                    break;
                case "float":
                    dotNetType = col.Nullable.ToBoolean() ? "float?" : "float";
                    break;
                case "decimal":
                    dotNetType = col.Nullable.ToBoolean() ? "decimal?" : "decimal";
                    break;
                case "timestamp":
                    dotNetType = "byte[]";
                    break;
                case "bit":
                    dotNetType = col.Nullable.ToBoolean() ? "bool?" : "bool";
                    break;
                case "uniqueidentifier":
                    dotNetType = "Guid";
                    break;
                case "tinyint":
                    dotNetType = col.Nullable.ToBoolean() ? "Int16?" : "Int16";
                    break;
                case "varchar":
                case "char":
                case "nvarchar":
                case "nchar":
                case "text":
                case "demo":
                default:
                    break;
            }
            return dotNetType;
        }

        /// <summary>
        /// 进行.Net CLR 类型和sql数据库之间类型的转换。
        /// </summary>
        /// <param name="sqlDbType">Type of the SQL db.</param>
        /// <returns></returns>
        public static string ExchangeDbType(string sqlDbType)
        {
            string dbType = "VarChar";
            switch (sqlDbType.ToLowerInvariant())
            {
                case "int":
                    dbType = "int?";
                    break;
                case "datetime":
                    dbType = "datetime";
                    break;
                case "float":
                    dbType = "float";
                    break;
                case "decimal":
                    dbType = "decimal";
                    break;
                case "varchar":
                    dbType = "varchar";
                    break;
                case "char":
                    dbType = "char";
                    break;
                case "nvarchar":
                    dbType = "nvarchar";
                    break;
                case "nchar":
                    dbType = "nchar";
                    break;
                case "text":
                    dbType = "text";
                    break;
                case "bit":
                    dbType = "int";
                    break;
                case "uniqueidentifier":
                    dbType = "uniqueidentifier";
                    break;
                default:
                    break;
            }
            return dbType;
        }

        /// <summary>
        /// 连接数据库。
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <exception cref="System.NullReferenceException">DataBaseServer为空;请设置数据库连接服务!</exception>
        /// <returns></returns>
        public bool Connect(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString.Trim()))
                throw new ArgumentNullException("connectionString", "要求连接字符串!");
            try
            {
                isOK();
                this.Server.ConnectionString = connectionString;
                this.DataBases = Server.LoadDataBases().ToArray();
                HasConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                HasConnected = false;
                throw ex;
            }
        }

        /// <summary>
        /// 使用当前配置的连接字符串进行数据库连接。
        /// </summary>
        /// <exception cref="System.NullReferenceException">数据库连接字符串未设置!</exception>
        /// <returns></returns>
        public bool Connect()
        {
            isOK();
            if (string.IsNullOrEmpty(this.Server.ConnectionString))
                throw new NullReferenceException("数据库连接字符串未设置!");
            return Connect(this.Server.ConnectionString);
        }

        /// <summary>
        /// Determines whether this instance is OK.
        /// </summary>
        private void isOK()
        {
            if (this.Server == null)
                throw new NullReferenceException("Server");
        }

        /// <summary>
        /// Loads the model class template.
        /// </summary>
        /// <returns></returns>
        public string LoadModelClassTemplate()
        {
            if (this.ModelClassTemplate == Templates.DefaultDefaultModelTemplateTag || File.Exists(this.ModelClassTemplate) == false)
            {
                return Templates.DefaultModelTemplate;
            }
            else
            {
                return File.ReadAllText(this.ModelClassTemplate);
            }
        }


        /// <summary>
        /// Loads the model data context class template.
        /// </summary>
        /// <returns></returns>
        public string LoadModelDataContextClassTemplate()
        {
            if (this.ModelClassTemplate == Templates.DefaultDefaultModelTemplateTag || File.Exists(this.DataContextTemplate) == false)
            {
                return Templates.DefaultDataContextTemplate;
            }
            else
            {
                return File.ReadAllText(this.DataContextTemplate);
            }
        }

        /// <summary>
        /// Generates the config.  
        /// </summary>
        /// <returns></returns>
        public string GenerateConfig()
        {
            StringBuilder sb = new StringBuilder(@" 
             <appSettings>
                    <add key = ""$DbProvider$"" value=""$DbProviderValue$""/>
             </appSettings> 
             <connectionStrings>
                    <add name= ""$DbConnectionName$"" connectionString = ""$DbConnectionString$""/>
             </connectionStrings>");
            var tname = Server.DbProviderType.ToString();
            sb.Replace("$DbProvider$", NkjSoft.ORM.ProviderFactory.PROVIDER_NODE_NAME)
                .Replace("$DbProviderValue$", tname)
                .Replace("$DbConnectionName$", tname)
                .Replace("$DbConnectionString$", Server.ConnectionString);
            return sb.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        class ModelFileWriter
        {
            /// <summary>
            /// Writes the file to.
            /// </summary>
            /// <param name="filePath">The file path.</param>
            /// <param name="content">The content.</param>
            public bool WriteFileTo(string filePath, string content)
            {
                try
                {
                    System.IO.File.WriteAllText(filePath, content);
                    return true;
                }
                catch (System.IO.IOException ioException)
                {
                    throw ioException;
                }
            }

            /// <summary>
            /// Appends the content to.
            /// </summary>
            /// <param name="filePath">The file path.</param>
            /// <param name="content">The content.</param>
            /// <returns></returns>
            public bool AppendContentTo(string filePath, string content)
            {
                try
                {
                    System.IO.File.AppendAllText(filePath, content);
                    return true;
                }
                catch (System.IO.IOException ioException)
                {
                    throw ioException;
                }
            }
        }
    }

    //TODO: Added at 2010-12-19 
    /// <summary>
    /// 
    /// </summary>




    /// <summary>
    /// 为生成器生成了实体类之后的事件处理函数提供数据。
    /// </summary>
    public class GeneratorEventArgs : EventArgs
    {

        /// <summary>
        /// 获取 保存的路径。
        /// </summary>
        public string FilePath
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取此时生成的实体类在所有实体对象集合的索引。
        /// </summary>
        /// <value>The index of the table.</value>
        public int TableIndex { get; private set; }

        /// <summary>
        /// 获取一个值,表示当前生成的实体类文件是否成功被写入文件。
        /// </summary>
        public bool WriteToFileSuccessful { get; private set; }

        /// <summary>
        /// 获取当前生成的实体类对应的数据库表名。
        /// </summary>
        /// <value>The name of the table.</value>
        public string TableName { get; private set; }

        /// <summary>
        /// 获取实体类文件保存的文件路径。
        /// </summary>
        public string ModelFilePathToSave { get; private set; }

        /// <summary>
        /// 实例化一个新的 <see cref="GeneratorEventArgs"/> 对象。
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="table">The table.</param>
        /// <param name="saveFolder">The p.</param>
        /// <param name="isOk">if set to <c>true</c> [is ok].</param>
        /// <param name="result">生成的结果。</param>
        /// <param name="filePath">The file path.</param>
        public GeneratorEventArgs(int status, string table, string saveFolder, bool isOk, string result, string filePath)
        {
            // TODO: Complete member initialization
            this.TableIndex = status;
            this.TableName = table;
            this.ModelFilePathToSave = saveFolder;
            this.WriteToFileSuccessful = isOk;
            this.Result = result;
            this.FilePath = filePath;
        }

        /// <summary>
        /// 获取当前生成的类文件的内容。
        /// </summary>
        /// <value>The result.</value>
        public string Result { get; private set; }
    }
}


