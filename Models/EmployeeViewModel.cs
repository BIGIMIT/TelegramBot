using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models;


public class EmployeeViewModel
{
    public long O_WITEM_ID { get; set; }
    public DateTime O_WITEM_DATE { get; set; }
    public string? O_WITEM_REGNUM { get; set; }
    public long O_WITEMSTATUS_ID { get; set; }
    public string? O_WITEMSTATUS_NAME { get; set; }
}