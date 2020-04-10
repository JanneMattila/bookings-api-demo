using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingApi
{
	class Program
	{
		async static Task Main(string[] args)
		{
			Console.WriteLine("Booking Api");

			// TODO: Set following variables correctly
			var clientId = "";
			var tenantId = "";
			var loginHint = "booking-service-account@demodomain.onmicrosoft.com";

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
				authenticationResult = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
			}

			var accessToken = authenticationResult.AccessToken;
			var expires = authenticationResult.ExpiresOn;

			Console.WriteLine("Expires:");
			Console.WriteLine(expires);
			Console.WriteLine(accessToken);
		}
	}
}
