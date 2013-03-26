using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PregnancyCalculator
{
    public class Pregnancy
    {
        private const int TOTAL_DAYS = (7 * 40);
        private DateTime _dateOfLastPeriod;
        private DateTime _dueDate;
        private DateTime _firstTrimester;
        private DateTime _secondTrimester;
        private DateTime _thirdTrimester;

        public DateTime DateOfLastPeriod { get { return _dateOfLastPeriod; } set { _dateOfLastPeriod = value; } }
        public DateTime DueDate { get { return _dueDate; } set { _dueDate = value; } }
        public DateTime FirstTrimester { get { return _firstTrimester; } }
        public DateTime SecondTrimester { get { return _secondTrimester; } }
        public DateTime ThirdTrimester { get { return _thirdTrimester; } }

        public int TotalDaysAlong()
        {
            return (DateTime.Now - DateOfLastPeriod).Days;
        }

        public int TotalDaysLeft()
        {
            return (TOTAL_DAYS - TotalDaysAlong());
        }   

        private void CalculateTrimesterDates()
        {
            _firstTrimester = _dateOfLastPeriod;
            _secondTrimester = _dateOfLastPeriod.AddDays(7 * 13);
            _thirdTrimester = _dateOfLastPeriod.AddDays(7 * 25);
        }

        public void CalculateDueDateFromDateOfLastPeriod()
        {
            _dueDate = DateOfLastPeriod.AddDays(TOTAL_DAYS);
            CalculateTrimesterDates();
        }

        public void CalculateDateOfLastPeriodFromDueDate()
        {
            _dateOfLastPeriod = DueDate.AddDays(-1 * TOTAL_DAYS);
            CalculateTrimesterDates();
        }

        public int CurrentWeek()
        {
            return TotalDaysAlong() / 7;
        }

        public int WeeksLeft()
        {
            return TotalDaysLeft() / 7;
        }

        public string FriendlyCurrentTrimester()
        {
            return Helpers.Integers.Ordinal(CurrentTrimester());
        }

        public int CurrentTrimester()
        {
            if (CurrentWeek() < 13)
            {
                return 1;
            }
            else if (CurrentWeek() < 25)
            {
                return 2;
            }
            else 
                return 3;
        }

        public DateTime? DateOfNextTrimester()
        {
            if (CurrentTrimester() == 1)
                return SecondTrimester;
            else if (CurrentTrimester() == 2)
                return ThirdTrimester;
            else
                return null;
        }

    }
}
