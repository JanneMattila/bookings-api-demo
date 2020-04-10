using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
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

			var scopes = new List<string> {
				"User.Read", "Bookings.Read.All",
				"Bookings.Manage.All", "Bookings.ReadWrite.All",
				"BookingsAppointment.ReadWrite.All", "offline_access"
			};
			var app = PublicClientApplicationBuilder.Create(clientId)
				.WithRedirectUri("http://localhost")
				.Build();

			var authenticationResult = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
			var accessToken = authenticationResult.AccessToken;

			var graphserviceClient = new GraphServiceClient(
				new DelegateAuthenticationProvider(
					(requestMessage) =>
					{
						requestMessage.Headers.Authorization =
							new AuthenticationHeaderValue("Bearer", accessToken);
						return Task.FromResult(0);
					}));
		}
	}
}
