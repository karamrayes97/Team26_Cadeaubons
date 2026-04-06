using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadeaubons_Domain.Model;

namespace Cadeaubons_Domain.DTO
{
    public sealed record ThemeDTO(
        int Id,
        string Name,
        string Description,
        string IconPath,
        string PrimaryColor
    )
    {
        public ThemeDTO(Theme theme ) : this(theme.Id, theme.Name, theme.Description, theme.IconPath, theme.PrimaryColor) { }
    }
}
