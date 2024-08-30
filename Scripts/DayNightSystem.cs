using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fishing.Scripts
{

    public  class DateHolder:IFormattable
    {
        public  int day { get; private set; }
        public  int month { get; private set; }
        public  int year { get; private set; }
        public  int hours { get; private set; }
        public  int minutes { get; private set; }
        public int week {  get; private set; }

        public DateHolder(int day,int week,int month, int year,int hour,int minutes)
        {
            this.day = day;
            this.month = month;
            this.year = year;
            this.hours= hour;
            this.minutes= minutes;
            this.week = week;
        }
        public override string ToString()
        {
            return $"Day: {day}, Week: {week}, Month: {month}, Year: {year}, Hour:{hours}, Minutes {minutes}";
        }
        /// <summary>
        /// EUDH-> 18:55;EUDHR-> 18:50 DD/MM EUDY-> DD/MM/YYYY
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// 

        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format.ToUpper())
            {
                case "EUDH":
                    return $"{hours.ToString("D2")}:{minutes.ToString("D2")} {day.ToString("D2")}/{month.ToString("D2")}";
                case "EUDHR":
                    return $"{hours.ToString("D2")}:{Math.Round((float)(minutes/10),MidpointRounding.ToZero)}0 {day.ToString("D2")}/{month.ToString("D2")}";
                case "EUDY":
                    return $"{day}/{month}/{year}";
                default:
                    return "INVALID FORMAT";
            }
        }
    }
    public class DateEventArgs : EventArgs
    {
        public DateHolder date;
    }
    public  class DayNightSystem
    {
        /// <summary>/// Holds all the passed days/// </summary>
        public static int allDaysSum { get; set; }
        /// <summary>/// Holds the current day in week/// </summary>
        public static int currentDay { get; private set; }
        public static int currentWeek {  get; private set; }
        public static int currentMonth { get; private set; }
        public static int currentYear { get; private set; }
        public static int currentHour { get; private set; }
        public static int currentMinutes { get; private set; }
        public  int oneWeekLength { get; private set; }
        public  int oneMonthLength {  get; private set; }
        /// <summary>/// Length of one year in months/// </summary>
        public  int oneYearLength { get; private set; }
        /// <summary>/// Length of an in-game day  described in minutes/// </summary>
        public  int oneDayLength { get; private set; }

        private  float timer { get;  set; }
        private float minuteLength { get; set; }
        private float hourLength { get; set; }
         
        public static EventHandler<DateEventArgs> OnNewDayEvent { get; set; }

        static DayNightSystem()
        {
            currentDay = 1;
            currentWeek = 1;
            currentMonth = 1;
            currentYear = 1;
        }
        public DayNightSystem(int oneDayLength= 10,int oneWeekLength=7,int oneMonthLength=30,int oneYearLength = 12)
        {
            this.oneDayLength = oneDayLength;
            this.oneWeekLength = oneWeekLength;
            this.oneMonthLength = oneMonthLength;
            this.oneYearLength = oneYearLength;
            hourLength = this.oneDayLength *60/24;
        }
        public static DateHolder GetCurrentDate() => new DateHolder(currentDay,currentWeek, currentMonth, currentYear, currentHour, currentMinutes);

        public static void SetDate(int day,int month,int year)
        {
            currentDay = day;
            currentMonth = month;
            currentYear = year;
        }
        public static void SetTime(int hour,int minutes)
        {
            currentHour = hour;
            currentMinutes = minutes;
            
        }

        public void HandleChanges()
        {
            if (currentHour >= 24) { currentDay++;allDaysSum++; currentHour = 0; }
            if (currentDay > oneWeekLength) { currentWeek++; currentDay = 0; }
            if(currentWeek*oneWeekLength>oneMonthLength) { currentMonth++;currentWeek = 0; }
            if(currentMonth>oneYearLength) { currentYear++; currentMonth = 0; }
        }
        public  void Update(GameTime gameTime)
        {
            HandleChanges();
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            currentMinutes = (int)Math.Round(timer / hourLength*60,MidpointRounding.ToZero);
            if(timer >= hourLength)
            {
                currentHour++;
                timer = 0;
            }
            
        }
    }
}
