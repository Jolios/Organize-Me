using Facebook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organize_Me
{
    class FacebookAuth
    {
        Form1 f;
        public FacebookAuth(Form1 f)
        {
            this.f = f;
        }
        public void login()
        {
            string ExtendedPermissions = "email";
            dynamic parameters = new ExpandoObject();
            parameters.client_id = "264036641444914";
            parameters.client_secret = "c55fb165844a8b2806577fc0971170a1";
            parameters.redirect_uri = "https://www.facebook.com/connect/login_success.html";

            parameters.response_type = "token";

            parameters.display = "popup";

            if (!string.IsNullOrWhiteSpace(ExtendedPermissions))
                parameters.scope = ExtendedPermissions;

            var fb = new FacebookClient();
            Uri _loginUrl = fb.GetLoginUrl(parameters);
            Process.Start(new ProcessStartInfo(_loginUrl.AbsoluteUri)
            {
                UseShellExecute = true
            });
            f.bunifuPages1.SetPage(1);
        }
    }
}
