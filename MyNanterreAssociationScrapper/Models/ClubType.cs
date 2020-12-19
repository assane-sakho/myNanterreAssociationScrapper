using System.Collections.Generic;

namespace MyNanterreAssociationScrapper
{
	public class ClubType
	{
        #region PROPERTIES
        public int? Id { get; set; }
		public string  Name { get; set; }
		private readonly List<Club> clubs;
        #endregion

        public ClubType(string name)
		{
			Name = name;
			clubs = new List<Club>();
		}

		public void AddClub(Club club)
        {
			clubs.Add(club);
		}

		public void RemoveClub(Club club)
		{
			clubs.Remove(club);
		}

		public List<Club> GetClubs()
        {
			return clubs;
        }
    }
}