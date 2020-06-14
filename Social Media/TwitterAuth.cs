using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;

namespace Organize_Me
{
    class TwitterAuth
    {
        Form1 f;
        public TwitterAuth(Form1 f)
        {
            this.f = f;
        }
        public async void login()
        {
            var appCredentials = new TwitterCredentials("a00PUbWuBhna8ECDCNB7VPX3f", "gZGYNexcSw4BPEKWYBZ5xdOLCSAnoOMCFvoUvqiqS6ubuNSffT");

            // Init the authentication process and store the related `AuthenticationContext`.
            var appClient = new TwitterClient(appCredentials);

            // Go to the URL so that Twitter authenticates the user and gives him a PIN code.
            var authenticationRequest = await appClient.Auth.RequestAuthenticationUrlAsync();
            Process.Start(new ProcessStartInfo(authenticationRequest.AuthorizationURL)
            {
                UseShellExecute = true
            });
            string pincode=Microsoft.VisualBasic.Interaction.InputBox("Enter pin", "Twitter pin code");
            var userCredentials = await appClient.Auth.RequestCredentialsFromVerifierCodeAsync(pincode, authenticationRequest);
            // You can now save those credentials or use them as followed
            var userClient = new TwitterClient(userCredentials);
            var user = await userClient.Users.GetAuthenticatedUserAsync();

            Console.WriteLine("Congratulation you have authenticated the user: " + user.Email);
            f.txt_SignUpEmail.Text = user.Email;
            f.txt_UserFName.Text = user.Name;
            f.bunifuPages1.SetPage(1);
        }
    }
}
