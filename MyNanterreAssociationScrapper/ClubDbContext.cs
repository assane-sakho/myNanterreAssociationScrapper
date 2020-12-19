using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MyNanterreAssociationScrapper
{
    class ClubDbContext
    {
        #region PROPERTIES
        private readonly MySqlConnection _mySqlConnection;

        private static ClubDbContext _instance;
        private readonly List<Club> _clubs;
        #endregion

        private ClubDbContext(List<Club> clubs)
        {
            _mySqlConnection = new MySqlConnection(ConfigurationManager.AppSettings.Get("connectionString"));
            _clubs = clubs;
        }

        public static ClubDbContext GetInstance(List<Club> clubs)
        {
            if (_instance == null)
                _instance = new ClubDbContext(clubs);
            return _instance;
        }

        public void InsertClubsToDB()
        {
            _mySqlConnection.Open();
            
            ExecuteNonQuery("SET GLOBAL max_allowed_packet=16777216;");

            List<ClubType> clubTypes = _clubs.Select(club => club.ClubType).Distinct().ToList();

            foreach (ClubType clubType in clubTypes)
            {
                SetClubTypeId(clubType);

                foreach (var club in clubType.GetClubs())
                    InsertOrUpdateClubToDB(club);
            }

            _mySqlConnection.Close();
        }

        private void SetClubTypeId(ClubType clubType)
        {
            string stm = ClubDbContextStatement.GetClubTypeIdStm(clubType);

            MySqlCommand command = new MySqlCommand(stm, _mySqlConnection);

            object result = command.ExecuteScalar();

            if (result != null)
                clubType.Id = (int)result;
            else
                InsertClubTypeToDb(clubType);
        }

        private void InsertClubTypeToDb(ClubType clubType)
        {
            clubType.Id = ExecuteScalar(ClubDbContextStatement.GetClubTypeInsertStm(clubType));
        }

        public void InsertOrUpdateClubToDB(Club club)
        {
            if (ClubExistInDb(club))
                UpdateClubInDb(club);
            else
                InsertClubToDB(club);
        }

        private bool ClubExistInDb(Club club) => ExecuteScalar(ClubDbContextStatement.GetClubExistStm(club)) > 0;

        private void InsertClubToDB(Club club) => ExecuteNonQuery(ClubDbContextStatement.GetClubInsertStm(club));

        private void UpdateClubInDb(Club club) => ExecuteNonQuery(ClubDbContextStatement.GetClubUpdateStm(club));

        private void ExecuteNonQuery(String stm)
        {
            MySqlCommand command = new MySqlCommand(stm, _mySqlConnection);

            command.ExecuteNonQuery();
        }

        private int ExecuteScalar(String stm)
        {
            MySqlCommand command = new MySqlCommand(stm, _mySqlConnection);

            return Convert.ToInt32(command.ExecuteScalar());
        }
    }
}
