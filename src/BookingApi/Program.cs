using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BookingApi
{
	class Program
	{
		async static Task Main(string[] args)
		{
			Console.WriteLine("Booking Api");

			var interactiveLogon = false;

			// TODO: Set following variables correctly
			var clientId = "";
			var tenantId = "";
			var loginHint = "booking-service-account@demodomain.onmicrosoft.com";
			using var password = new NetworkCredential(string.Empty, 
				"booking-service-account's password here").SecurePassword;

			var scopes = new List<string> 
			{
				"User.Read", "Bookings.Read.All",
				"Bookings.Manage.All", "Bookings.ReadWrite.All",
				"BookingsAppointment.ReadWrite.All", "offline_access"
			};
			var app = PublicClientApplicationBuilder.Create(clientId)
				.WithTenantId(tenantId)
				.WithRedirectUri("http://localhost")
				.Build();
			TokenCacheHelper.EnableSerialization(app.UserTokenCache);
			AuthenticationResult authenticationResult;

			try
			{
				authenticationResult = await app.AcquireTokenSilent(scopes, loginHint).ExecuteAsync();
			}
			catch (MsalUiRequiredException)
			{
				if (interactiveLogon)
				{
					// Use interactive login
					authenticationResult = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
				}
				else
				{
					// Use username and password
					authenticationResult = await app.AcquireTokenByUsernamePassword(scopes, loginHint, password).ExecuteAsync();
				}
			}

			var accessToken = authenticationResult.AccessToken;
			var expires = authenticationResult.ExpiresOn;

			Console.WriteLine("Expires:");
			Console.WriteLine(expires);
			Console.WriteLine(accessToken);
		}
	}
}
