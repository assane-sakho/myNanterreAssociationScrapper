using System.Collections.Generic;
using MyNanterreAssociationScrapper.Models;

namespace MyNanterreAssociationScrapper
{
    static class Program
    {
        static void Main()
        {
            List<Club> clubs = AssociationLoader.GetInstance()
                                                .SearchClubs()
                                                .CompleteClubsInformation()
                                                .GetClubs();

            ClubDbContext.GetInstance(clubs)
                         .InsertClubsToDB();
        }
    }
}
