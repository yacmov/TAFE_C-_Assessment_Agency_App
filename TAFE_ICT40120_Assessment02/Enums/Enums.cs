using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAFE_ICT40120_Assessment02.Enums
{
    /// <summary>
    /// for conditions depending on mainbuttons
    /// </summary>
    public enum MAINBUTTONS
    {
        NOTICE,
        NORMAL,
        CONTRACTOR,
        JOB,
        HISTORY,
        COMPLETED,
        CANCEL,
    }


    public enum DAYMONTHYEAR
    {
        DAY,
        MONTH,
        YEAR,
    }

    public enum NOTICEBOARD
    {
        NOTICE = 0,
        NORMAL = 1,
    }

    public enum CONTRACTOR_STATUS
    {
        All = 0,
        Available = 1,
        Assigned = 2,
        Holiday = 3,
        ETC = 4,
    }

    public enum LEVEL
    {
        L1, L2, L3
    }

    public enum JOB_STATUS
    {
        All = 0,
        Pending = 1,
        Assigned = 2,
        Completed = 3,
        CANCEL = 4,
    }

    public enum SORT_CONTRACTOR_BY_DETAILS
    {
        By_Id = 0,
        By_Level = 1,
        By_Wage = 2,

    }

    public enum SORT_JOB_BY_DETAILS
    {
        By_Due = 0,
        By_Cost = 1,
        By_Title = 2,
        By_Assigned = 3,
        By_Status = 4,

    }

    public enum MIN_MAX
    {
        MIN = 0,
        MAX = 1,
    }

    public enum SORT_HISTORY_BY_DETAILS
    {
        Default = 0,
        By_Date = 1,
        By_Cost = 2,
        By_Title = 3,
    }

}
