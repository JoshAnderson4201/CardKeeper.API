using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardKeeper.API.Models;
using CardKeeper.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CardKeeper.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImportRoundController : ControllerBase
    {
        private readonly IImportRoundService _importRoundService;
        public ImportRoundController(IImportRoundService importRoundService)
        {
            _importRoundService = importRoundService;
        }

        [HttpGet]
        public async Task<Scorecard> ImportRound()
        {
            string testPath = "D:/Dev/Scorecards/test_scorecard.jpg";

            return await _importRoundService.ImportRound(testPath, "Gabby", "Red");
        }
    }
}