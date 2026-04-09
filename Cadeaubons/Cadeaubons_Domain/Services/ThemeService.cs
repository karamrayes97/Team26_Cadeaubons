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

        public ThemeDTO Add(string name, string description, string iconPath, string primaryColor)
        {
            if (GetByName(name) != null)
                throw new InvalidOperationException("A theme with this name already exists.");

            Theme theme = new Theme
            {
                Name = name,
                Description = description,
                IconPath = iconPath,
                PrimaryColor = primaryColor
            };

            _repository.Themes.Add(theme);
            _repository.SaveChanges();

            return new ThemeDTO(theme);
        }

        public ThemeDTO Update(int id, string name, string description, string iconPath, string primaryColor)
        {
            Theme? theme = _repository.Themes.FirstOrDefault(t => t.Id == id);
            if (theme == null)
                throw new InvalidOperationException("Theme not found.");

            Theme? existing = _repository.Themes.FirstOrDefault(t => t.Name.ToLower() == name.Trim().ToLower() && t.Id != id);
            if (existing != null)
                throw new InvalidOperationException("Another theme with this name already exists.");

            theme.Name = name;
            theme.Description = description;
            theme.IconPath = iconPath;
            theme.PrimaryColor = primaryColor;

            _repository.SaveChanges();

            return new ThemeDTO(theme);
        }

        public void Delete(int id)
        {
            Theme? theme = _repository.Themes.FirstOrDefault(t => t.Id == id);
            if (theme == null)
                throw new InvalidOperationException("Theme not found.");

            _repository.Themes.Remove(theme);
            _repository.SaveChanges();
        }
    }
}
