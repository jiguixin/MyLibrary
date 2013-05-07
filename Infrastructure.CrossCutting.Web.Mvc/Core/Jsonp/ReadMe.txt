参考：http://www.cnblogs.com/dowinning/archive/2012/04/19/2458624.html
http://www.cnblogs.com/keyindex/archive/2012/04/20/2459626.html

<h2>
    Index</h2>
<script>
    var GetCustomers = function (data) {
        alert(data[0].Address );
    };


    var url = "http://localhost:1853/home/GetCustomers";

    var script = document.createElement('script');
    script.setAttribute('src', url);
    document.getElementsByTagName('head')[0].appendChild(script); 
</script>


<script>
    var GetCustomer = function (data) {
        alert(data.Address+" "+data.Name+" "+data.ID);
    };


    var url = "http://localhost:1853/home/Customer/2";

    var script = document.createElement('script');
    script.setAttribute('src', url);
    document.getElementsByTagName('head')[0].appendChild(script); 
</script>

<script>
    var GetPI = function (data) {
        alert(data);
    };


    var url = "http://localhost:1853/home/Calculate?callback=GetPI";

    var script = document.createElement('script');
    script.setAttribute('src', url);
    document.getElementsByTagName('head')[0].appendChild(script); 
</script>

public ActionResult GetCustomers()
        {
            var customers = new[]{
                 new Customer{ Address="长安街1", Id=1, Name="张三"},
                 new Customer{ Address="长安街2", Id=2, Name="李四"},
                 new Customer{ Address="长安街3", Id=3, Name="dudu"},
                 new Customer{ Address="长安街4", Id=4, Name="DotDot"},
                 new Customer{ Address="长安街5", Id=5, Name="随它去吧"}

            };

            return Jsonp(customers);
        }



        public ActionResult Customer(int id)
        {
            var customers = new[]{
                 new Customer{ Address="长安街1", Id=1, Name="张三"},
                 new Customer{ Address="长安街2", Id=2, Name="李四"},
                 new Customer{ Address="长安街3", Id=3, Name="dudu"},
                 new Customer{ Address="长安街4", Id=4, Name="DotDot"},
                 new Customer{ Address="长安街5", Id=5, Name="随它去吧"}

            };

            var customer = customers.FirstOrDefault(c => c.Id == id);

            return JsonpView(customer);

        }


        public ActionResult Calculate(string callback)
        {
            var PI = Math.PI;
            return Jsonp(PI, callback);
        }