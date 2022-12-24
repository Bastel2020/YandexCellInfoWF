using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Models
{
    public class SectorsStorage
    {
        private Dictionary<int, int> _polularSectors { get; set; }
        private IOrderedEnumerable<int> _otherSectors { get; set; }
        private float _popularSectorRequestCount;
        private

        public SectorsStorage()
        {
            _otherSectors = new List<int>(Enumerable.Range(0, 256)).OrderBy(x => x);
            _polularSectors = new Dictionary<int, int>();
        }

        public IEnumerable<int> GetAllSectors() => Enumerable.Range(0, 256);

        public IEnumerable<int> GetNotPopularSectors() => _otherSectors;

        public IEnumerable<int> GetPopularSectors()
        {
            _popularSectorRequestCount++;

            var result = _polularSectors.Keys;

            if (_popularSectorRequestCount % 10 == 0)
                RemoveRareSectorsFromPopular();

            return result;
        }

        private void RemoveRareSectorsFromPopular()
        {

        }
    }
}
