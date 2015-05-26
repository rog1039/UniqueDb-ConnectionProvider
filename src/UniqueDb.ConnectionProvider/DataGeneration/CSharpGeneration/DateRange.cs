using System;

namespace UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration
{
    public class DateRange : GenericRange<DateTime>
    {
        public DateRange(DateTime lowerBound, DateTime upperBound) : base(lowerBound, upperBound)
        {
        }
    }
}