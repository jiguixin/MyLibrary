----TraceSource配置方法------- 

<configuration>
  
  <system.diagnostics>
      <sources>
        <source name="TraceSourceApp" switchName="sourceSwitch" switchType="System.Diagnostics.SourceSwitch">
          <!--switchValue="Warning" 也可以在此处直接指定switch的值-->
          <listeners>
            <add name="console" type="System.Diagnostics.ConsoleTraceListener">
              <!--initializeData 主要是用于从那个等级开始记录,如只会记录大于Information的消息-->
              <filter type="System.Diagnostics.EventTypeFilter" initializeData="Information"/>
            </add>
            <add name="myListener"/>
            <remove name="Default"/>
          </listeners>
        </source>
      </sources>
      <switches>
        <!--value 主要是用于从那个等级开始记录,如只会记录大于Information的消息-->
        <add name="sourceSwitch" value="Information"/>
      </switches>
      <sharedListeners>
        <add name="myListener" type="System.Diagnostics.TextWriterTraceListener" initializeData="myListener.log">
          <filter type="System.Diagnostics.EventTypeFilter" initializeData="Error"/>
        </add>
      </sharedListeners>
      <trace autoflush="true" indentsize="4">
        <listeners>
          <add name="myListener" />
        </listeners>
      </trace>
    </system.diagnostics>


</configuration>
