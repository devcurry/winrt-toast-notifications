using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using System.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Store_CS_ToastNotification
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MyRef.Service1Client Proxy;
        ObservableCollection<MyRef.EmployeeInfo> Employees;

        public MainPage()
        {
            this.InitializeComponent();
            Proxy = new MyRef.Service1Client();
        }

      
        /// <summary>
        /// This will Start the Dispatcher Timer and will make call to WCF service after each 20 Seconds.
        /// Thye Data will be ynchronized after each 20 Seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btncreatetoast_click(object sender, RoutedEventArgs e)
        {
            DispatcherTimer tim = new DispatcherTimer();
            tim.Interval = new TimeSpan(0,0,20);
            tim.Tick += tim_Tick;
            tim.Start();
        }

        /// <summary>
        /// The Tick Event whihc makes call to WCF service after each 20 seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void tim_Tick(object sender, object e)
        {
           ObservableCollection<MyRef.EmployeeInfo> NewRecord = await Proxy.GetEmployeesAsync();
          
            //If the newly received data is lesser than the Newly received data from the WCF service
            //the show the toast notification
            if (Employees.Count < NewRecord.Count || Employees!=NewRecord)
            {

                var Emp = NewRecord.LastOrDefault();
                //The ToastNotificationManager object used to raise toast notification
                XmlDocument xtempl = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
                XmlNodeList text = xtempl.GetElementsByTagName("text");
                text[0].AppendChild(xtempl.CreateTextNode("The new Employee with EmpName as " + Emp.EmpName + " is added") );

                //ToastNotification defines the metadata for the ToastNotification
                ToastNotification toast = new ToastNotification(xtempl);
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
            Employees = NewRecord;
            gvEmployees.ItemsSource = Employees;
        }

        /// <summary>
        /// On the loaded event which makes class to WCF service and display data in the GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            Employees = await Proxy.GetEmployeesAsync();
            gvEmployees.ItemsSource = Employees;
        }

    }
}
