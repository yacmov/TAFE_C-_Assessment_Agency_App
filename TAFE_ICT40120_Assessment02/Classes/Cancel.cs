using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAFE_ICT40120_Assessment02.Enums;

namespace TAFE_ICT40120_Assessment02.Classes
{
    /// <summary>
    /// Cancel Job handling class
    /// </summary>
    public class Cancel
    {
        public Job cancelJob;

        public override string ToString()
        {
            int count = 25;
            int subCount = 15;
            if (cancelJob.Title.Length >= count)
            {
                cancelJob.TitleShort = $"{cancelJob.Title.Substring(0, count)}...";
            }
            else if (subCount <= cancelJob.Title.Length && cancelJob.Title.Length <= count)
            {
                cancelJob.TitleShort = $"{cancelJob.Title}        ";
            }
            else
            {
                cancelJob.TitleShort = $"{cancelJob.Title}                  ";
            }
            return $" {cancelJob.Level}, {cancelJob.TitleShort,-29}\t{cancelJob.DeadLine.ToString("dd/MM/yy")}  ${cancelJob.Cost.ToString("000.###")}\t{cancelJob.Status,-10}\t";
        }
    }
}
