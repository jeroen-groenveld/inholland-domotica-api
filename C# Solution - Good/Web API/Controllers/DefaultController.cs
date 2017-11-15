using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API.Models;

namespace Web_API.Controllers
{
    public class DefaultController : Controller
    {
        public class PaginationResult
        {
            public IEnumerable<object> result;
            public int current_page;
            public int last_page;
            public int page_size;
            public readonly int first_page = 1;
        }

        private const int PAGE_COUNT = 20;
        private const int PAGE_COUNT_MAX = 50;

        public PaginationResult Pagination<T>(DbSet<T> model, int page = 1, int count = PAGE_COUNT) where T : class
        {
            page = (page < 1) ? 1 : page;
            count = (count > PAGE_COUNT_MAX) ? PAGE_COUNT_MAX : count;

            int last_page = (int)Math.Ceiling(model.Count() / (decimal)count);
            page = (page > last_page) ? last_page : page;

            PaginationResult res = new PaginationResult();
            res.result = model.Skip((page - 1) * count).Take(count).ToList();
            res.current_page = page;
            res.last_page = last_page;
            res.page_size = count;

            return res;
        }
    }
}
