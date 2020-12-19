using System.Collections.Generic;

namespace MyNanterreAssociationScrapper.Models
{
	public class ClubType
	{
        #region PROPERTIES
        public int? Id { get; set; }
		public string  Name { get; set; }
		public List<Club> Clubs { get; set; }
		#endregion

		public ClubType(string name)
		{
			Name = name;
			Clubs = new List<Club>();
		}
    }
}