using MyNanterreAssociationScrapper.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MyNanterreAssociationScrapper
{
    class AssociationLoader
    {
        #region PROPERTIES
        private static readonly string _baseURl = ConfigurationManager.AppSettings.Get("baseUrl");

        private readonly List<ClubType> _clubTypes;
        private List<Club> _clubs;
        private readonly IWebDriver _driver;

        private static AssociationLoader Instance;

        #endregion

        private AssociationLoader()
        {
            _clubTypes = new List<ClubType>();
            ChromeOptions chromeOptions = new ChromeOptions();
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            chromeDriverService.HideCommandPromptWindow = true;
            chromeDriverService.SuppressInitialDiagnosticInformation = true;

            chromeOptions.AddArgument("headless");
            chromeOptions.AddArgument("--silent");
            chromeOptions.AddArgument("log-level=3");

            _driver = new ChromeDriver(chromeDriverService, chromeOptions);
        }

        public static AssociationLoader GetInstance()
        {
            if (Instance == null)
                Instance = new AssociationLoader();

            return Instance;
        }
        public AssociationLoader SearchClubs()
        {
            _driver.Navigate().GoToUrl(_baseURl);

            ReadOnlyCollection<IWebElement> associationCategories = _driver.FindElements(By.ClassName("paragraphe--2"));

            string associationCategoryTitle;

            ClubType clubType;
            ReadOnlyCollection<IWebElement> associations;

            foreach (IWebElement associationCategory in associationCategories)
            {
                associationCategoryTitle = associationCategory.FindElement(By.ClassName("paragraphe__titre--2")).Text;

                clubType = new ClubType(associationCategoryTitle);
                _clubTypes.Add(clubType);

                associations = associationCategory.FindElement(By.ClassName("paragraphe__contenu--2"))
                                                  .FindElements(By.ClassName("lien_interne"));

                foreach (IWebElement association in associations)
                    clubType.Clubs.Add(new Club(association.Text, association.GetAttribute("href"), clubType));
            }

            _clubs = _clubTypes.SelectMany(clubType => clubType.Clubs).ToList();

            return this;
        }

        public AssociationLoader CompleteClubsInformation()
        {
            string description;
            string imageUrl;
            string contact;
            string websiteUrl;
            string mail;

            IWebElement objetWebElement;
            IWebElement photoWebElement;
            IWebElement infosWebElement;

            string legend;
            string infosXpath;

            List<Club> clubsToRemove = new List<Club>();

            foreach (Club club in _clubs)
            {
                _driver.Navigate().GoToUrl(club.Url);

                try
                {
                    if (club.Name == String.Empty)
                        club.Name = _driver.FindElement(By.TagName("h1")).Text;

                    objetWebElement = _driver.FindElement(By.Id("objet"));

                    if (_driver.FindElements(By.ClassName("photo")).Any())
                    {
                        photoWebElement = _driver.FindElement(By.ClassName("photo"));
                        legend = photoWebElement.FindElement(By.TagName("p")).Text;
                        imageUrl = photoWebElement.FindElement(By.TagName("img")).GetAttribute("src");
                    }
                    else
                    {
                        legend = String.Empty;
                        imageUrl = String.Empty;
                    }

                    description = !String.IsNullOrEmpty(legend) ? objetWebElement.Text.Replace(legend, "") : objetWebElement.Text;
                  
                    infosXpath = "//*[@id=\"avec_nav_sans_encadres\"]/div/dl";

                    infosWebElement = _driver.FindElement(By.XPath(infosXpath));

                    if (infosWebElement.Text.Contains("Coordonnées :"))
                        contact = _driver.FindElement(By.XPath($"{infosXpath}/dd[1]")).Text;
                    else
                        contact = String.Empty;

                    if (infosWebElement.Text.Contains("Mél :"))
                        mail = _driver.FindElement(By.ClassName("mail")).Text;
                    else
                        mail = String.Empty;

                    if (infosWebElement.Text.Contains("Site web :"))
                        websiteUrl = _driver.FindElement(By.XPath($"{infosXpath}/dd[2]")).Text;
                    else
                        websiteUrl = String.Empty;

                    club.SetDescription(description)
                        .SetImageUrl(imageUrl)
                        .SetContact(contact)
                        .SetMail(mail)
                        .SetWebsiteUrl(websiteUrl);
                }
                catch (Exception ex)
                {
                    clubsToRemove.Add(club);
                    Console.WriteLine($"{club.Name} : {ex.Message}");
                }
            }

            foreach (Club clubToRemove in clubsToRemove)
            {
                clubToRemove.ClubType.Clubs.Remove(clubToRemove);
                _clubs.Remove(clubToRemove);
            }

            CloseDriver();

            return this;
        }

        public List<Club> GetClubs()
        {
            return _clubs;
        }

        private void CloseDriver()
        {
            _driver.Close();
            _driver.Quit();
        }
    }
}
