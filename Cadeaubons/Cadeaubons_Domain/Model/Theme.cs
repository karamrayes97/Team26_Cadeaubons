using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Model
{
    [Table("Themes")]
    public class Theme
    {
        private int _id;
        [Key]
        [Column("Id")]
        public int Id
        {
            get => _id;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Id must be zero or positive.", nameof(Id));

                _id = value;
            }
        }

        private string _name = string.Empty;
        [Column("Name")]
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name is required.", nameof(Name));

                _name = value.Trim();
            }
        }

        [Column("Description")]
        public string Description { get; set; } = string.Empty;

        private string _iconPath = string.Empty;
        [Column("IconPath")]
        public string IconPath
        {
            get => _iconPath;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Icon path is required.", nameof(IconPath));

                _iconPath = value.Trim();
            }
        }

        private string _primaryColor = string.Empty;
        [Column("PrimaryColor")]
        public string PrimaryColor
        {
            get => _primaryColor;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Primary color is required.", nameof(PrimaryColor));

                _primaryColor = value.Trim();
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Theme theme &&
                string.Equals(theme.Name, Name, StringComparison.OrdinalIgnoreCase);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(StringComparer.OrdinalIgnoreCase.GetHashCode(Name ?? string.Empty));
        }

        public override string ToString()
        {
            return $"Theme - Id: {Id}, Name: {Name}, Primary color: {PrimaryColor}";
        }
    }
}
