using Firebase.Database;
using FirebaseConnection.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Storage;
using Firebase.Auth;

namespace FirebaseConnection
{
    class FirebaseConnector
    {   
        FirebaseClient firebase = new FirebaseClient("https://fir-nav-fbd51.firebaseio.com/", new FirebaseOptions { AuthTokenAsyncFactory = () => AnonLogin() });

        public async Task<List<Campus>> GetCampus()
        {
            var items = await (firebase
                .Child("Trafalgar")
                .OnceAsync<object>());


            List<Campus> c = new List<Campus>();
            c.Add(new Campus());
            var en = items.GetEnumerator();
            
            foreach (var item in items)
            {
                en.MoveNext();
                if (item.Key == "CAMPUS_ID")
                {
                    c[0].CAMPUS_ID = Int32.Parse(item.Object.ToString());
                }
                else if (item.Key == "CAMPUS_NAME")
                    c[0].CAMPUS_NAME = (string)item.Object;
                else
                {
                    c[0].WINGS = JsonConvert.DeserializeObject<List<string>>(en.Current.Object.ToString());
                }
            }

            return c;

            /*****************************
            Get back to this when feasible
            **************************/
            //return (await firebase
            //    .Child("Trafalgar")
            //    .OnceAsync<Campus>())
            //    .Select(item => new Campus
            //    {
            //        CAMPUS_ID = item.Object.CAMPUS_ID,
            //        CAMPUS_NAME = item.Object.CAMPUS_NAME,
            //        WINGS = (item.Object.WINGS)
            //    }).ToList();
        }

        public async Task<Uri> GetImage()
        {
            var query = await firebase.Child("FLOOR_DATA/TRAE2").OnceAsync<object>();

            var value = "";

            foreach (var item in query)
            {
                if (item.Key == "FLOOR_IMAGE")
                    value = item.Object.ToString();
            }

            Uri image =  new Uri(await new FirebaseStorage("fir-nav-fbd51.appspot.com").Child("Images").Child("TRAE2.jpg").GetDownloadUrlAsync());

            return image;
        }

        public static async Task<string> AnonLogin()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyDlrs12gBooCtCtg6SXVG2xP3BE-jYXk7g"));
            var auth = await authProvider.SignInAnonymouslyAsync();
            return auth.FirebaseToken;
        }
    }
}
