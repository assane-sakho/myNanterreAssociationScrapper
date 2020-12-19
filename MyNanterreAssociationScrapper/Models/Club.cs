using System;

namespace MyNanterreAssociationScrapper.Models
{
	public class Club
	{
        #region PROPERTIES
        public ClubType ClubType { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public string Description { get; set; }
		public string ImageUrl { get; set; }
		public string ImageAsString { get; set; }
		public string Contact { get; set; }
		public string Mail { get; set; }
		public string WebsiteUrl { get; set; }
        #endregion

        #region SETTER
        public Club(string name, string url, ClubType clubType)
		{
			Name = name;
			Url = url;
			ClubType = clubType;
			Description = String.Empty;
			ImageUrl = String.Empty;
			ImageAsString = String.Empty;
			Contact = String.Empty;
			Mail = String.Empty;
			WebsiteUrl = String.Empty;
		}

		public Club SetDescription(string description)
        {
			Description = description;
			return this;
		}

		public Club SetImageUrl(string imageUrl)
		{
			ImageUrl = imageUrl;
			return this;
		}

		public Club SetContact(string contact)
		{
			Contact = contact;
			return this;
		}

		public Club SetMail(string mail)
		{
			Mail = mail;
			return this;
		}

		public Club SetWebsiteUrl(string websiteUrl)
		{
			WebsiteUrl = websiteUrl;
			return this;
		}

        public void SetImageAsString(string imagAsString)
		{
			ImageAsString = imagAsString;
		}
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}