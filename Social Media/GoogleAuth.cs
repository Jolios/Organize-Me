using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Bunifu.Core.forms.licensing;

namespace Organize_Me
{
    
    public class GoogleAuth
    {
        Form1 f;

        public GoogleAuth(Form1 f)
        {
            this.f = f;
        }

        public async void login()
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { Google.Apis.Oauth2.v2.Oauth2Service.Scope.UserinfoProfile, Google.Apis.Oauth2.v2.Oauth2Service.Scope.UserinfoEmail },
                    "user", CancellationToken.None);
            }

            // Create the service.
            var service = new Oauth2Service(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Organize-Me",
            });
            var userInfo = await service.Userinfo.Get().ExecuteAsync();
            f.txt_SignUpEmail.Text = userInfo.Email;
            f.FirstName = userInfo.GivenName;
            f.LastName = userInfo.FamilyName;
            await credential.RevokeTokenAsync(CancellationToken.None);
            f.bunifuPages1.SetPage(1);

        }


    }
}