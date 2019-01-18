using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestParallelInsert.Data;
using TestParallelInsert.Repositories;
using TestParallelInsert.Utils;

namespace TestParallelInsert.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IRepository _repository;

        public TestController(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // POST api/values
        [HttpPost("insert")]
        public  ContentResult Post()
        {
            var translations = new List<Translation>();
            var accumulator = new StringBuilder();
            for (var i = 0; i < 1000; i++)
            {
                translations.Add(new Translation
                {
                    Translated = SecureRandomUtils.AlphaNumericString(10),
                    Untranslated = SecureRandomUtils.AlphaNumericString(10)
                });
            }

            Parallel.ForEach(translations, new ParallelOptions {MaxDegreeOfParallelism = 20},
                (translation) =>
                {
                    accumulator.AppendLine(_repository.InsertData(translation.Untranslated, translation.Translated));
                });

            return Content(accumulator.ToString());
        }
    }
}