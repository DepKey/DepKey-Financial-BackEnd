using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for JournalMovement
/// </summary>
public class JournalMovement
{
    public JournalMovement()
    {
        JournalMovementHistories = new List<JournalMovementHistory>();
    }

    public int? ID { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? CreationDate { get; set; }
    public double? Debit { get; set; }
    public double? Credit { get; set; }
    public string MovementDescription { get; set; }
    public int? JournalID { get; set; }
    public int? AccountID { get; set; }
    public int? JournalMovementID { get; set; }

    public  Journal Journal { get; set; } 
    public  List<JournalMovementHistory> JournalMovementHistories { get; set; }
}