using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Models.DTOs
{
    public record GetFilteredDto(
        int Skip,
        int PageSize,
        string SearchValue,
        string SortColumnIndex,
        string SortColumn,
        string SortColumnDirection
    );
}
