using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardKeeper.API.Models;

namespace CardKeeper.API.Services.Interfaces
{
    public interface IImportRoundService
    {
        public Task<Scorecard> ImportRound(string imagePath, string playerName, string teeColor);
    }
}