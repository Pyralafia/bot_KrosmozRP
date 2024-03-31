using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Misc
{
    internal class GoogleSheetsHelper
    {
        const string APPLICATION_NAME = "KrosmozRpBot";
        public SheetsService Service { get; set; }
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };

        public GoogleSheetsHelper()
        {
            InitializeService();
        }

        private void InitializeService()
        {

        }
    }
}
