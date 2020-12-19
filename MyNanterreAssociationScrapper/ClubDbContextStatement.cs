using MyNanterreAssociationScrapper.Models;
using System;
using System.Configuration;

namespace MyNanterreAssociationScrapper
{
    static class ClubDbContextStatement
    {
        private static readonly string _dbCreatorId = ConfigurationManager.AppSettings.Get("dbCreatorId");
        public static string GetClubTypeIdStm(ClubType clubType)
        {
            string stm =
               "SELECT id " +
               "FROM club_type " +
               $"WHERE name ='{clubType.Name.Replace("'", "''")}'";
            return stm;
        }

        public static string GetClubTypeInsertStm(ClubType clubType)
        {
            string stm =
                "INSERT INTO club_type(name)" +
                $"VALUES ('{clubType.Name.Replace("'", "''")}');";

            stm +=
                "SELECT id " +
                "FROM club_type " +
                $"WHERE name ='{clubType.Name.Replace("'", "''")}';";

            return stm;
        }

        public static string GetClubExistStm(Club club)
        {
            string stm =
                "SELECT COUNT(*) " +
                "FROM club " +
                $"WHERE name ='{FormatStr(club.Name)}'";
            return stm;
        }

        public static string GetClubInsertStm(Club club)
        {
            string stm =
                "INSERT INTO club(name, " +
                "description, " +
                "is_certificate, " +
                "is_validate, " +
                "creator_id, " +
                "club_type_id, " +
                "website, " +
                "mail, " +
                "contact," +
                "nanterreWebsiteUrl," +
                "image)" +

               $"VALUES ('{FormatStr(club.Name)}', " +
               $"'{FormatStr(club.Description)}', " +
               $"1, " +
               $"1, " +
               $"{_dbCreatorId}, " +
               $"'{club.ClubType.Id}', ";
                stm += !String.IsNullOrEmpty(club.WebsiteUrl) ? "'" + FormatStr(club.WebsiteUrl) + "'," : "NULL, ";
                stm += !String.IsNullOrEmpty(club.Mail) ? "'" + FormatStr(club.Mail) + "'," : "NULL, ";
                stm += !String.IsNullOrEmpty(club.Contact) ? "'" + FormatStr(club.Contact) + "'," : "NULL, ";
                stm += 
                $"'{FormatStr(club.Url)}', " +
                $"'{club.ImageAsString}'); ";

            return stm;
        }

        public static string GetClubUpdateStm(Club club)
        {
            string stm =
                "UPDATE club " +
                "SET " +

                "is_certificate = 1, " +
                "is_validate = 1, " +
                $"description = '{FormatStr(club.Description)}', " +
                $"club_type_id = '{club.ClubType.Id}',  ";
                stm += "website=" + (!String.IsNullOrEmpty(club.WebsiteUrl) ? "'" + FormatStr(club.WebsiteUrl) + "'," : "NULL, ");
                stm += "mail=" + (!String.IsNullOrEmpty(club.Mail) ? "'" + FormatStr(club.Mail) + "'," : "NULL, ") ;
                stm += "contact=" + (!String.IsNullOrEmpty(club.Contact) ? "'" + FormatStr(club.Contact) + "'," : "NULL, ");
                stm +=
                $"club_type_id = '{club.ClubType.Id}',  " +
                $"nanterreWebsiteUrl='{FormatStr(club.Url)}' " +

                $"WHERE name='{FormatStr(club.Name)}' ;";

            return stm;
        }

        private static string FormatStr(string str)
        {
            return str.Replace("'", "''").Replace("’", "''");
        }
    }
}
