using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Exceptions;

namespace KarateClub.Domain.ValueObjs
{
    public record Period
    {
        public DateTime StartDate {  get; }
        public DateTime EndDate {  get; }

        public Period(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new DomainException("Start date cannot be greater than end date");
            if (endDate < startDate) throw new DomainException("End date cannot be less than start date");
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
