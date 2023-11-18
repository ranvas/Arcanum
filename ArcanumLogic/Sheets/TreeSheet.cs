using ArcanumLogic.EntityFramework.Model;
using GoogleSheet.Abstractions;
using GoogleSheet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.Sheets
{
    public class TreeSheet : SheetServiceBase<Tree>
    {
        public TreeSheet(ISheetAdapter adapter, ArcanumOptions options) : base(adapter)
        {
            if (string.IsNullOrEmpty(options.GoogleSpreadSheetId))
                throw new ArgumentNullException(nameof(options.GoogleSpreadSheetId));
            SpreadSheetId = options.GoogleSpreadSheetId;
        }
        protected override string SpreadSheetId { get; set; } = string.Empty;
        protected override GoogleSheetRange Range { get; set; } =
            new GoogleSheetRange
            {
                List = "дерево исследований",
                StartColumn = "A",
                StartRow = 1,
                EndRow = 10000,
                EndColumn = "F"
            };
    }
}
