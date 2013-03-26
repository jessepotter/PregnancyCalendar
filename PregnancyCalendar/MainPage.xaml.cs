using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PregnancyCalculator;
using Helpers;
using Save;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PregnancyCalendar
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Pregnancy _currentPregnancy = new Pregnancy();

        public MainPage()
        {
            this.InitializeComponent();
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DateTime lastPeriod;
            string savedDateOfLastPeriod = Save.LocalStorage.LoadRoaming("date_of_last_period");

            try
            {
                lastPeriod = DateTime.Parse(savedDateOfLastPeriod);
                _currentPregnancy.DateOfLastPeriod = lastPeriod;
                _currentPregnancy.CalculateDueDateFromDateOfLastPeriod();
                displayPregnancy();
                txtDueDate.Text = _currentPregnancy.DueDate.Month.ToString() + "/" + _currentPregnancy.DueDate.Day.ToString() + "/" + _currentPregnancy.DueDate.Year.ToString();
            }
            catch
            {

            }

        }

        private void txtDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                DateTime lastPeriod;

                lastPeriod = DateTime.Parse(txtDate.Text);
                _currentPregnancy.DateOfLastPeriod = lastPeriod;
                _currentPregnancy.CalculateDueDateFromDateOfLastPeriod();
                LocalStorage.SaveRoaming("date_of_last_period", _currentPregnancy.DateOfLastPeriod.ToString());
                displayPregnancy();
                txtDueDate.Text = _currentPregnancy.DueDate.Month.ToString() + "/" + _currentPregnancy.DueDate.Day.ToString() + "/" + _currentPregnancy.DueDate.Year.ToString();
            }
            catch
            {
            }
        }

        private void displayPregnancy()
        {
            lblDateOfBirth.Text = "Your estimated due date is " + Date.FormatDateLong(_currentPregnancy.DueDate);
            lblCurrentDay.Text = "You are on day " + _currentPregnancy.TotalDaysAlong().ToString() + " of your pregnancy";
            lblWeek.Text = "You are in week " + _currentPregnancy.CurrentWeek().ToString() + " of your pregnancy";
            lblCurrentTrimester.Text = "You are in the " + _currentPregnancy.FriendlyCurrentTrimester() + " trimester";

            lblRemainingDays.Text = "Your due date is " + _currentPregnancy.TotalDaysLeft().ToString() + " days away";
            lblRemainingWeeks.Text = "Your due date is " + _currentPregnancy.WeeksLeft().ToString() + " weeks away";
            if (_currentPregnancy.DateOfNextTrimester() == null)
            {
                lblNextTrimesterStarts.Text = "";
            }
            else
            {
                lblNextTrimesterStarts.Text = "Your " + Helpers.Integers.Ordinal(_currentPregnancy.CurrentTrimester() + 1) + " trimester starts on " + Helpers.Date.FormatDateLong((DateTime)_currentPregnancy.DateOfNextTrimester());
            }

            UpdateNotifications();
        }

        private void UpdateNotifications()
        {
            PushTileNotification("Week " + _currentPregnancy.CurrentWeek().ToString(), "current_week", DateTimeOffset.UtcNow.AddDays(7));
            PushTileNotification("Day " + _currentPregnancy.TotalDaysAlong().ToString(), "current_day", DateTimeOffset.UtcNow.AddDays(1));
            PushTileNotification(_currentPregnancy.FriendlyCurrentTrimester() + " Trimester", "current_trimester", DateTimeOffset.UtcNow.AddDays(21));
        }

        private void txtDueDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                DateTime newDueDate = DateTime.Parse(txtDueDate.Text);
                DateTime lastPeriod;

                _currentPregnancy.DueDate = newDueDate;
                _currentPregnancy.CalculateDateOfLastPeriodFromDueDate();
                lastPeriod = _currentPregnancy.DateOfLastPeriod;
                LocalStorage.SaveRoaming("date_of_last_period", _currentPregnancy.DateOfLastPeriod.ToString());
                displayPregnancy();
                txtDate.Text = lastPeriod.Month.ToString() + "/" + lastPeriod.Day.ToString() + "/" + lastPeriod.Year.ToString();
            }
            catch
            {
            }
        }


        private void PushTileNotification(string text, string tag, DateTimeOffset expires)
        {

            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareText01);

            lblCurrentDay.Text = tileXml.ToString();

            XmlNodeList tileTextAttributes = tileXml.GetElementsByTagName("text");
            tileTextAttributes[0].InnerText = text;

            //XmlNodeList tileImageAttributes = tileXml.GetElementsByTagName("image");
            //((XmlElement)tileImageAttributes[0]).SetAttribute("src", "ms-appx:///images/redWide.png");
            //((XmlElement)tileImageAttributes[0]).SetAttribute("alt", "red graphic");

            XmlDocument squareTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareText01);
            XmlNodeList squareTileTextAttributes = squareTileXml.GetElementsByTagName("text");
            squareTileTextAttributes[0].AppendChild(squareTileXml.CreateTextNode(text));
            IXmlNode node = tileXml.ImportNode(squareTileXml.GetElementsByTagName("binding").Item(0), true);
            tileXml.GetElementsByTagName("visual").Item(0).AppendChild(node);

            TileNotification tileNotification = new TileNotification(tileXml);

            if (expires == null)
                expires =  DateTimeOffset.UtcNow.AddSeconds(10);
            tileNotification.ExpirationTime = expires;
            tileNotification.Tag = tag;


            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);

        }
    }
}