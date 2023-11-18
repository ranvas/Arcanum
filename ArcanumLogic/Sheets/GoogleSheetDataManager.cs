using GoogleSheet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanumLogic.Sheets
{
    public class GoogleSheetDataManager
    {
        public AccountSheet AccountSheet { get; set; }
        public ImagineSheet ImagineSheet { get; set; }
        public TreeSheet TreeSheet { get; set; }
        public FabricaSheet  FabricaSheet { get; set; }

        public GoogleSheetDataManager(AccountSheet account, ImagineSheet imagines, TreeSheet treeSheet, FabricaSheet fabricaSheet)
        {
            AccountSheet = account;
            ImagineSheet = imagines;
            TreeSheet = treeSheet;
            FabricaSheet = fabricaSheet;
        }
    }
}
