此项目为集成所有公共类型的解决方案，以使用以后能够快速开发用。

日志：
V_0.0.1，2011-9-13 0：32 整和了NLayerV2的代码与四色原型的事例代码。
V_0.0.2，2011-9-13 17：12  增加了Infrastructure.Data.EF相应的数据访问层工程.
V_0.0.3, 2011-9-15 23:54 增加了Log4Not的日志记录方式。
V_0.0.4,2011-9-16 17:31  增加核心层的公共模块。
V_0.0.5,2011-9-18 :18:00 增加根据类型得到实例。
V_0.0.6,2011-9-19 23:49 增加AutoMapper的TypeAdapter的测试项目。
V_0.0.7,2011-9-22 15:53 增加AutoMapper的正反向测试项目，另添加IEnginee相应的东西。
V_0.0.8,2011-9-30 0:42 增加了DbExecutor公共ADO.NET数据库操作
V_0.0.9,2011-10-7 20:42 增加整个ADO.NET的数据访问与及EF的实现测式。
V_0.1.0,2011-10-20 16:02 修改了DbExecutor可以直接接收SqlParameter参数，同时引入了EF4.1的DbContext管理，但目前还不清楚最终用法。
V_0.1.1,2011-10-21 15:02 在ado.net类库中增加了Dopper组件类，经过测试发现Dopper还没有DbExecutor快，同时在EF中增加了DbContextManager类。
V_0.1.2,2011-10-26 15:42 修改IRepository接口与修改Ef的DbContext的实现方式。
V_0.1.2,2011-10-27 17:42 增加汉字转拼音帮助类。
V_0.1.3,2011-11-9 11:06 增加HttpsHelper类主要是可能完成对HTTPS的请求帮助类。
V_0.1.3,2011-11-15 16:06 在ObjectExtendMethod中增加在字符串中提取数值方法。
V_0.1.4,2012-2-9 11:06 解决了其它项目没法引用Infrastructure.Crosscutting项目，因为它的目标框架为.NET4.0,其它项目为.NET4.0 Client Prifile版本。
                       所以必须把要引用项目改成为.net4.0 。
V_0.1.5,2012-3-9 11:46 一，在解决方案中增加Infrastructure.Crosscutting.Security 主要用于权限控制.
					  二，将Resharp生成的文件、Output、OutPutTest默认不上传到GIT服务器。
					  三，增加专门的程序集目录，以便在引用时调用。
					  四，修改所有生成为Output,OutputTest
V_0.1.6,2012-3-14 16:06 增加获取本机信息增加LicenseKey.cs类
V_0.1.7,2012-3-19 18:06 增加对象验证类：ValidationManager.cs，和对像扩展方法。
V_0.1.7,2012-3-19 22:06 增加对象验证类的测试代码。
V_0.1.8,2012-4-6 15:06  增加自动更新程序与自动更新加包程序。
V_0.1.9,2012-4-23 14:56 为ASP.NET MVC项目中增加JSONP功能，为了支持跨域JS功能。
V_0.2.0,2012-4-23 17:56 为ASP.NET MVC项目中增加简单的Filters
V_0.2.1,2012-4-26 17:06 完成对EXCELHELPER的修改与增加对象与集合赋值方法。
V_0.2.2,2012-6-21 14:06 在ASP.NET 网站中FORM验证后的登录信息。