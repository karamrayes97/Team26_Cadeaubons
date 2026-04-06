using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Repo;

namespace Cadeaubons_Domain.Services
{
    public class ThemeService
    {
        private readonly Repository _repository;

        public ThemeService(Repository repository)
        {
            _repository = repository;
        }
        public List<ThemeDTO> GetAll()
        {
            return _repository
                .Themes
                .Select(t => new ThemeDTO(t))
                .ToList();
        }
        public ThemeDTO? GetByName(string name) {
            Theme? theme = _repository
                .Themes
                .FirstOrDefault(t => t.Name.ToLower() == name.Trim().ToLower());
            return theme == null ? null : new ThemeDTO(theme);
        }
    }
}
