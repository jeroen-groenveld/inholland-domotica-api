using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Models
{
    public class Date
    {
        public interface IModelUpdatedAt
        {
            DateTime? updated_at { get; set; }
        }

        public interface IModelCreatedAt
        {
            DateTime? created_at { get; set; }
        }

        public abstract class DateModel : IModelCreatedAt, IModelUpdatedAt
        {
            public DateTime? updated_at { get; set; }
            public DateTime? created_at { get; set; }
        }

        public abstract class DateModelCreatedAt : IModelCreatedAt
        {
            public DateTime? created_at { get; set; }
        }

        public abstract class DateModelUpdatedAt : IModelUpdatedAt
        {
            public DateTime? updated_at { get; set; }
        }
    }
}
