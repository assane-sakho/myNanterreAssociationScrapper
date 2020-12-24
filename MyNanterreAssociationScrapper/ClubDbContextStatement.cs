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
                "contact, " +
                "nanterreWebsiteUrl" +
                $"{(String.IsNullOrEmpty(club.ImageUrl) ? ")" : ", image_url)")}" +

                $"VALUES ('{FormatStr(club.Name)}', " +
                $"'{FormatStr(club.Description)}', " +
                $"1, " +
                $"1, " +
                $"{_dbCreatorId}, " +
                $"'{club.ClubType.Id}', " +
                $"{(String.IsNullOrEmpty(club.WebsiteUrl) ? "'" + FormatStr(club.WebsiteUrl) + "'" : "NULL")}, " +
                $"{(String.IsNullOrEmpty(club.Mail) ? "'" + FormatStr(club.Mail) + "'" : "NULL")}, " +
                $"{(String.IsNullOrEmpty(club.Contact) ? "'" + FormatStr(club.Contact) + "'" : "NULL")}, " +
                $"'{FormatStr(club.Url)}'" +
                $"{(String.IsNullOrEmpty(club.ImageUrl) ? ");" : ", '" + club.ImageUrl + "');")}";

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
                $"club_type_id = '{club.ClubType.Id}',  " +
                $"website = {(String.IsNullOrEmpty(club.WebsiteUrl) ? "'" + FormatStr(club.WebsiteUrl) + "'" : "NULL")}, " +
                $"mail = {(String.IsNullOrEmpty(club.Mail) ? "'" + FormatStr(club.Mail) + "'" : "NULL")}, " +
                $"contact = {(String.IsNullOrEmpty(club.Contact) ? "'" + FormatStr(club.Contact) + "'" : "NULL")}, " +
                $"club_type_id = '{club.ClubType.Id}',  " +
                $"nanterreWebsiteUrl='{FormatStr(club.Url)}' ";

            if(!String.IsNullOrEmpty(club.ImageUrl))
                stm += $", image_url = '{club.ImageUrl}' ";

            stm += $"WHERE name='{FormatStr(club.Name)}' ;";

            return stm;
        }

        private static string FormatStr(string str)
        {
            return str.Replace("'", "''").Replace("’", "''");
        }
    }
}
