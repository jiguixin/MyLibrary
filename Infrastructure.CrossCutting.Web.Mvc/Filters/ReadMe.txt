
创建了Filters 必须在Global.asax中加上相应的Filters
 public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RequestLogFilter());

            // Remove comment to make TraceActionFilterAttribute global
            //  filters.Add(new TraceActionFilterAttribute());

            var provider = new RequestTimingFilterProvider();
            provider.Add(c => c.HttpContext.IsDebuggingEnabled ? new RequestTimingFilter() : null);
            FilterProviders.Providers.Add(provider);
        }

