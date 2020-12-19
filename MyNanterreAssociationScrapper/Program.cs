using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using MyNanterreAssociationScrapper.Models;

namespace MyNanterreAssociationScrapper
{
    static class Program
    {
        private static HttpClient client;
        private static string nanterreLogoBase64;
        static async Task Main()
        {
            client = new HttpClient();

            string currentDirectory = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            string nanterreLogoPath = Path.Combine(currentDirectory, @"Resources\", "favicon.png");

            nanterreLogoBase64 = Convert.ToBase64String(File.ReadAllBytes(nanterreLogoPath));


            List<Club> clubs = AssociationLoader.GetInstance()
                                                .SearchClubs()
                                                .CompleteClubsInformation()
                                                .GetClubs();
            foreach (Club club in clubs)
                await SetClubImageBase64Async(club);

            ClubDbContext.GetInstance(clubs)
                         .InsertClubsToDB();
        }

        private static async Task SetClubImageBase64Async(Club club)
        {
            string imageString;

            if(club.ImageUrl != String.Empty)
                using (var response = await client.GetAsync(club.ImageUrl))
                {
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync()
                                                              .ConfigureAwait(false);
                    
                    imageString = Convert.ToBase64String(imageBytes);
                }
            else
                imageString = nanterreLogoBase64;

            club.SetImageAsString(imageString);
        }
    }
}
