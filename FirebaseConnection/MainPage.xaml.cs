using FirebaseConnection.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FirebaseConnection
{
    public partial class MainPage : ContentPage
    {
        FirebaseConnector conn = new FirebaseConnector();
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            List<Campus> x = await conn.GetCampus();
           
            if(x != null)
            {
                foreach (var item in x)
                {
                    testList.ItemsSource = item.CAMPUS_ID.ToString();
                    testList.ItemsSource = item.CAMPUS_NAME;
                    testList.ItemsSource = item.WINGS;
                }
            }
        }

        private async void ImagePull(object sender, EventArgs e)
        {
            Uri image_uri = await conn.GetImage();

            TestImage.Source = image_uri;
        }
    }
}
