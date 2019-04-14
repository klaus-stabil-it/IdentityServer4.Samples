// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
   public class Program
   {
      private static async Task Main()
      {
         // discover endpoints from metadata
         var oidcClient = new HttpClient();

         var disco = await oidcClient.GetDiscoveryDocumentAsync("http://localhost:5000");
         if (disco.IsError)
         {
            Console.WriteLine(disco.Error);
            return;
         }

         // request token
         var tokenResponse = await oidcClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
         {
            Address = disco.TokenEndpoint,
            ClientId = "client",
            ClientSecret = "secret",

            Scope = "api1"
         });

         if (tokenResponse.IsError)
         {
            Console.WriteLine(tokenResponse.Error);
            return;
         }

         Console.WriteLine(tokenResponse.Json);
         Console.WriteLine("\n\n");

         // call api
         var apiClient = new HttpClient();
         apiClient.SetBearerToken(tokenResponse.AccessToken);
         Console.WriteLine("1: identityinfo, 2: garble\n");
         var key = Console.ReadKey().Key;
         while (key == ConsoleKey.D1 || key == ConsoleKey.D2)
         {
            var response = await apiClient.GetAsync("http://localhost:5001/" + (key == ConsoleKey.D1 ? "identity" : "garble"));
            if (!response.IsSuccessStatusCode)
            {
               Console.WriteLine(response.StatusCode);
            }
            else
            {
               var content = await response.Content.ReadAsStringAsync();
               switch (key)
               {
                  case ConsoleKey.D1:
                     Console.WriteLine(JArray.Parse(content));
                     break;
                  case ConsoleKey.D2:
                     Console.WriteLine(content);
                     break;
                  default:
                     Console.WriteLine("UPS!");
                     break;
               }
            }
            key = Console.ReadKey().Key;
         }
      }
   }
}