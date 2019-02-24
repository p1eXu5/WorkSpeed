using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.Entities
{
    public class RankViewModel
    {
        private readonly Rank _rank;

        public RankViewModel ( Rank rank )
        {
            _rank = rank ?? throw new ArgumentNullException(nameof(rank), @"Rank cannot be null.");
        }

        public int Number => _rank.Number;
    }
}
