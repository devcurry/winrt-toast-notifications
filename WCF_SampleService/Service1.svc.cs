using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCF_SampleService
{
    public class Service1 : IService1
    {

        CompanyEntities objContext;

        public Service1()
        {
            objContext = new CompanyEntities(); 
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }


        public EmployeeInfo[] GetEmployees()
        {
            return objContext.EmployeeInfoes.ToArray();
        }
    }
}
